using CommunityToolkit.Mvvm.ComponentModel;
using ProjectManagement.Data;
using ProjectCycleManage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace ProjectCycleManage.ViewModel
{
    public partial class MainVM:ObservableObject
    {
        [ObservableProperty]
        private string _loginpersonname = "李世永";

        /// <summary>
        /// 登陆者级别
        /// </summary>
        [ObservableProperty]
        private int _loginpersonnamegrade = 2;

        private OverviewVM _overviewvm;
        public OverviewVM OverView
        {
            get => _overviewvm;
            set => _overviewvm = value;
        }

        private Timer _monitoringTimer;
        private readonly HashSet<int> _alertedProjects;
        private ProjectContext _context;

        public MainVM(string name , int personnamegrade)
        {
            Loginpersonnamegrade = personnamegrade;
            Loginpersonname = name;

            _overviewvm = new OverviewVM(Loginpersonnamegrade , Loginpersonname);
            _alertedProjects = new HashSet<int>();
            _context = new ProjectContext();
            
            // 启动项目监控
            StartProjectMonitoring();
        }

        /// <summary>
        /// 启动项目监控
        /// </summary>
        private void StartProjectMonitoring()
        {
            _monitoringTimer = new Timer(async _ => await CheckProjectsAsync(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        /// <summary>
        /// 停止项目监控
        /// </summary>
        public void StopProjectMonitoring()
        {
            _monitoringTimer?.Dispose();
            _monitoringTimer = null;
        }

        /// <summary>
        /// 获取当前登录用户的ID
        /// </summary>
        private async Task<int?> GetCurrentUserIdAsync()
        {
            try
            {
                var user = await _context.PeopleTable
                    .FirstOrDefaultAsync(p => p.PeopleName == _loginpersonname);
                
                return user?.PeopleId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取当前用户ID出错: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 检查用户是否具有审批权限
        /// </summary>
        private async Task<bool> CheckApprovalPermissionAsync(int projectId, int currentUserId)
        {
            try
            {
                // 1. 获取项目信息
                var project = await _context.Projects
                    .Include(p => p.type)
                    .FirstOrDefaultAsync(p => p.ProjectsId == projectId);

                if (project == null)
                    return false;

                // 2. 检查审批权限 - 查询 typeapprflowpersseqtable 表
                var hasPermission = await _context.TypeApprFlowPersSeqTable
                    .AnyAsync(x => x.equipmenttypeId == project.equipmenttypeId && x.ReviewerPeopleId == currentUserId);

                if (!hasPermission)
                {
                    // 丢弃结果
                    return false;
                }

                // 3. 检查项目流程状态
                var currentYear = DateTime.Now.Year;
                var validFlowStatus = new[] { 101, 103, 105, 107, 109, 111 };

                var hasValidFlow = await _context.Projects
                    .Where(p => p.Year == currentYear && p.ProjectsId == projectId)
                    .AnyAsync(p => validFlowStatus.Contains(p.ProjInforId ?? 0));

                if (!hasValidFlow)
                    return false;

                // 4. 检查审批记录
                var inspectionRecord = await _context.InspectionRecord
                    .Where(ir => ir.ProjectsId == projectId && ir.projId == project.ProjInforId)
                    .OrderByDescending(ir => ir.Sequence)
                    .FirstOrDefaultAsync();

                if (inspectionRecord == null)
                {
                    // 不存在审批记录，检查是否为第一顺位审批人
                    var firstApprover = await _context.TypeApprFlowPersSeqTable
                        .Where(x => x.equipmenttypeId == project.equipmenttypeId)
                        .OrderBy(x => x.Sequence)
                        .Select(x => x.ReviewerPeopleId)
                        .FirstOrDefaultAsync();

                    return currentUserId == firstApprover;
                }
                else
                {
                    // 存在审批记录，检查前一顺位是否通过
                    var currentUserSeq = await _context.TypeApprFlowPersSeqTable
                        .Where(x => x.equipmenttypeId == project.equipmenttypeId && x.ReviewerPeopleId == currentUserId)
                        .Select(x => x.Sequence)
                        .FirstOrDefaultAsync();

                    if (currentUserSeq <= 1)
                    {
                        // 已经是第一顺位，丢弃结果
                        return false;
                    }

                    // 查找前一顺位审批人的审批结果
                    var previousSeq = currentUserSeq - 1;
                    var previousApproverId = await _context.TypeApprFlowPersSeqTable
                        .Where(x => x.equipmenttypeId == project.equipmenttypeId && x.Sequence == previousSeq)
                        .Select(x => x.ReviewerPeopleId)
                        .FirstOrDefaultAsync();

                    var previousApproval = await _context.InspectionRecord
                        .Where(ir => ir.ProjectsId == projectId &&
                                    ir.projId == project.ProjInforId &&
                                    ir.CheckPeopleId == previousApproverId)
                        .OrderByDescending(ir => ir.CheckTime)
                        .FirstOrDefaultAsync();

                    if (previousApproval == null || previousApproval.CheckResult != "PASS")
                    {
                        // 前一顺位未通过或不存在，丢弃结果
                        return false;
                    }

                    // 检查当前用户是否已经审批过
                    var currentUserApproval = await _context.InspectionRecord
                        .Where(ir => ir.ProjectsId == projectId &&
                                    ir.projId == project.ProjInforId &&
                                    ir.CheckPeopleId == currentUserId)
                        .FirstOrDefaultAsync();

                    // 如果当前用户已经审批过，则不能再次审批
                    return currentUserApproval == null;
                }
            }
            catch (Exception ex)
            {
                // 记录日志
                Console.WriteLine($"审批权限检查出错: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 检查项目状态
        /// </summary>
        private async Task CheckProjectsAsync()
        {
            try
            {
                var currentUserId = await GetCurrentUserIdAsync();
                if (currentUserId == null)
                {
                    Console.WriteLine("无法获取当前用户ID，跳过检查");
                    return;
                }

                var currentYear = DateTime.Now.Year;
                var alertStatuses = new[] { 101, 103, 105, 107, 109, 111 };

                var alertProjects = await _context.Projects
                    .Where(p => p.Year == currentYear && alertStatuses.Contains(p.ProjInforId ?? 0))
                    .Include(p => p.ProjectLeader)
                    .Include(p => p.type)
                    .Include(p => p.ProjectPhaseStatus)
                    .Select(p => new ProjectAlertInfo
                    {
                        ProjectId = p.ProjectsId,
                        ProjectName = p.ProjectName,
                        ProjectLeader = p.ProjectLeader.PeopleName,
                        StatusId = p.ProjInforId ?? 0,
                        StatusName = p.ProjectPhaseStatus.ProjectPhaseStatusName,
                        ProjectType = p.type.TypeName,
                        ApplicationTime = p.ApplicationTime,
                    })
                    .ToListAsync();

                if (alertProjects.Any())
                {
                    var validAlertProjects = new List<ProjectAlertInfo>();

                    // 对每个项目进行完整的审批流程检查
                    foreach (var project in alertProjects)
                    {
                        // 检查是否已经提醒过
                        if (_alertedProjects.Contains(project.ProjectId))
                            continue;

                        // 检查审批权限和流程条件
                        var canApprove = await CheckApprovalPermissionAsync(project.ProjectId, currentUserId.Value);
                        
                        if (canApprove)
                        {
                            validAlertProjects.Add(project);
                            _alertedProjects.Add(project.ProjectId);
                        }
                    }

                    if (validAlertProjects.Any())
                    {
                        // 在UI线程上显示审批弹窗
                        Application.Current?.Dispatcher.Invoke(() =>
                        {
                            ShowApprovalDialog(validAlertProjects);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"项目监控出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 显示审批提醒对话框
        /// </summary>
        private void ShowApprovalDialog(List<ProjectAlertInfo> alertProjects)
        {
            var message = new StringBuilder();
            message.AppendLine("以下项目需要您审批：\n");

            foreach (var project in alertProjects)
            {
                message.AppendLine($"项目ID: {project.ProjectId}");
                message.AppendLine($"项目名称: {project.ProjectName}");
                message.AppendLine($"项目负责人: {project.ProjectLeader}");
                message.AppendLine($"当前状态: {project.StatusName}");
                message.AppendLine($"项目类型: {project.ProjectType}");
                message.AppendLine($"申请时间: {project.ApplicationTime?.ToString("yyyy-MM-dd") ?? "未知"}");
                message.AppendLine("------------------------\n");
            }

            MessageBox.Show(message.ToString(), "审批提醒", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 显示提醒对话框
        /// </summary>
        private void ShowAlertDialog(List<ProjectAlertInfo> alertProjects)
        {
            var message = new StringBuilder();
            message.AppendLine("以下项目需要关注：\n");

            foreach (var project in alertProjects)
            {
                message.AppendLine($"项目ID: {project.ProjectId}");
                message.AppendLine($"项目名称: {project.ProjectName}");
                message.AppendLine($"项目负责人: {project.ProjectLeader}");
                message.AppendLine($"当前状态: {project.StatusName}");
                message.AppendLine($"项目类型: {project.ProjectType}");
                message.AppendLine("------------------------\n");
            }

            MessageBox.Show(message.ToString(), "项目状态提醒", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
