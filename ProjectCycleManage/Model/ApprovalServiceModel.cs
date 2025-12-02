using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;

namespace ProjectCycleManage.Model
{
    public class ApprovalServiceModel
    {
        private readonly ProjectContext _context;

        public ApprovalServiceModel(ProjectContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 检查需要提醒的项目状态
        /// </summary>
        /// <returns>需要提醒的项目列表</returns>
        public async Task<List<ProjectAlertInfo>> CheckProjectAlertsAsync()
        {
            try
            {
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

                return alertProjects;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"项目状态检查出错: {ex.Message} -- Approvalse");
                Console.WriteLine($"项目状态检查出错: {ex.Message}");
                return new List<ProjectAlertInfo>();
            }
        }

        /// <summary>
        /// 检查当前用户是否具有审批权限
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="currentUserId">当前登录用户ID</param>
        /// <returns>是否可以审批</returns>
        public async Task<bool> CheckApprovalPermissionAsync(int projectId, int currentUserId)
        {
            try
            {
                // 1. 获取项目信息
                var project = await _context.Projects
                    .Include(p => p.ProjInfor)
                    .FirstOrDefaultAsync(p => p.ProjectsId == projectId);

                if (project == null)
                    return false;

                // 2. 检查审批权限 - 查询 ypeappflowpersseqtable 表
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
                        .Where(x => x.equipmenttypeId == project.typeId && x.ReviewerPeopleId == currentUserId)
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
                        .Where(x => x.equipmenttypeId == project.typeId && x.Sequence == previousSeq)
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
        /// 执行审批操作
        /// </summary>
        public async Task<ApprovalResult> ExecuteApprovalAsync(int projectId, int currentUserId, string checkResult, string checkOpinion)
        {
            var canApprove = await CheckApprovalPermissionAsync(projectId, currentUserId);

            if (!canApprove)
            {
                return new ApprovalResult { Success = false, Message = "无审批权限" };
            }

            try
            {
                var project = await _context.Projects
                    .FirstOrDefaultAsync(p => p.ProjectsId == projectId);

                if (project == null)
                    return new ApprovalResult { Success = false, Message = "项目不存在" };

                // 获取当前用户的审批顺序
                var userSeq = await _context.TypeApprFlowPersSeqTable
                    .Where(x => x.equipmenttypeId == project.equipmenttypeId && x.ReviewerPeopleId == currentUserId)
                    .Select(x => x.Sequence)
                    .FirstOrDefaultAsync();

                // 创建审批记录
                var inspectionRecord = new InspectionRecord
                {
                    ProjectsId = projectId,
                    CheckPeopleId = currentUserId,
                    CheckTime = DateTime.Now,
                    CheckResult = checkResult,
                    CheckOpinion = checkOpinion,
                    projId = project.ProjInforId ?? 0,
                    Sequence = userSeq.GetValueOrDefault()
                };

                _context.InspectionRecord.Add(inspectionRecord);
                await _context.SaveChangesAsync();

                return new ApprovalResult { Success = true, Message = "审批成功" };
            }
            catch (Exception ex)
            {
                return new ApprovalResult { Success = false, Message = $"审批失败: {ex.Message}" };
            }
        }
    }

    /// <summary>
    /// 审批结果
    /// </summary>
    public class ApprovalResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// 项目提醒信息
    /// </summary>
    public class ProjectAlertInfo
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectLeader { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string ProjectType { get; set; }
        public DateTime? ApplicationTime { get; set; }
        public int ProjectPhaseStatusId { get; set; }
    }

    /// <summary>
    /// 简化版项目监控服务
    /// </summary>
    public class SimpleProjectMonitor
    {
        private readonly ProjectContext _context;
        private Timer _monitoringTimer;
        private readonly HashSet<int> _alertedProjects;

        public SimpleProjectMonitor(ProjectContext context)
        {
            _context = context;
            _alertedProjects = new HashSet<int>();
        }

        /// <summary>
        /// 启动监控服务
        /// </summary>
        public void StartMonitoring()
        {
            _monitoringTimer = new Timer(async _ => await CheckProjectsAsync(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        /// <summary>
        /// 停止监控服务
        /// </summary>
        public void StopMonitoring()
        {
            _monitoringTimer?.Dispose();
            _monitoringTimer = null;
        }

        /// <summary>
        /// 检查项目状态
        /// </summary>
        private async Task CheckProjectsAsync()
        {
            try
            {
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
                    // 检查是否有新的需要提醒的项目
                    var newAlertProjects = alertProjects.Where(p => !_alertedProjects.Contains(p.ProjectId)).ToList();

                    if (newAlertProjects.Any())
                    {
                        // 更新已提醒项目列表
                        foreach (var project in newAlertProjects)
                        {
                            _alertedProjects.Add(project.ProjectId);
                        }

                        // 在UI线程上显示弹窗
                        Application.Current?.Dispatcher.Invoke(() =>
                        {
                            ShowAlertDialog(newAlertProjects);
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

    /// <summary>
    /// 用户服务类
    /// </summary>
    public class UserService
    {
        private readonly ProjectContext _context;

        public UserService(ProjectContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 根据用户名获取用户ID
        /// </summary>
        public async Task<int?> GetUserIdByNameAsync(string userName)
        {
            try
            {
                var user = await _context.PeopleTable
                    .FirstOrDefaultAsync(p => p.PeopleName == userName);

                return user?.PeopleId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取用户ID出错: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 验证用户登录
        /// </summary>
        public async Task<bool> ValidateUserAsync(string userName, string password)
        {
            try
            {
                // 这里需要根据你的实际用户验证逻辑来实现
                // 目前先简单验证用户名是否存在
                var user = await _context.PeopleTable
                    .FirstOrDefaultAsync(p => p.PeopleName == userName);

                return user != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"用户验证出错: {ex.Message}");
                return false;
            }
        }
    }
}
