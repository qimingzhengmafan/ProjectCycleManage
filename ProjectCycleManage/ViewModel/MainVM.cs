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
using CommunityToolkit.Mvvm.Input;

namespace ProjectCycleManage.ViewModel
{
    public partial class MainVM:ObservableObject
    {
        /// <summary>
        /// 登陆人
        /// </summary>
        [ObservableProperty]
        private string _loginpersonname;

        /// <summary>
        /// 登陆者级别
        /// </summary>
        [ObservableProperty]
        private int _loginpersonnamegrade;

        private OverviewVM _overviewvm;
        public OverviewVM OverView
        {
            get => _overviewvm;
            set
            {
                _overviewvm = value;
                OnPropertyChanged();
            }
        }

        private NewProjectVM _newprojectvm;

        public NewProjectVM NewProject
        {
            get => _newprojectvm;
            set => _newprojectvm = value;
        }

        private DataChartVM _datachartvm;
        public DataChartVM DataChart
        {
            get => _datachartvm;
            set => _datachartvm = value;
        }

        private Timer _monitoringTimer;
        private readonly HashSet<int> _alertedProjects;
        private readonly Dictionary<int, DateTime> _alertTimes;
        private ProjectContext _context;
        DateTime lastdatetime;

        public MainVM(string name , int personnamegrade)
        {
            Loginpersonnamegrade = personnamegrade;
            Loginpersonname = name;


            _overviewvm = new OverviewVM(Loginpersonnamegrade , Loginpersonname);
            _newprojectvm= new NewProjectVM(Loginpersonnamegrade, Loginpersonname);

            _alertedProjects = new HashSet<int>();
            _alertTimes = new Dictionary<int, DateTime>();
            _context = new ProjectContext();
            
            // 启动项目监控
            StartProjectMonitoring();

            Vis_overview = Visibility.Visible;
            Vis_newproject = Visibility.Collapsed;
        }



        #region 项目监控
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
        /// 获取项目的最后提醒时间
        /// </summary>
        private DateTime GetLastAlertTime(int projectId)
        {
            return _alertTimes.ContainsKey(projectId) ? _alertTimes[projectId] : DateTime.MinValue;
        }

        /// <summary>
        /// 更新项目的提醒时间
        /// </summary>
        private void UpdateAlertTime(int projectId, DateTime alertTime)
        {
            _alertTimes[projectId] = alertTime;
        }

        /// <summary>
        /// 检查项目状态是否发生了变化（进入新的审核阶段）
        /// </summary>
        private async Task<bool> CheckIfProjectStatusChangedAsync(int projectId, DateTime lastAlertTime)
        {
            try
            {
                // 如果从未提醒过，不需要检查状态变化
                if (lastAlertTime == DateTime.MinValue)
                    return false;

                // 获取项目当前的状态信息
                var currentProject = await _context.Projects
                    .Where(p => p.ProjectsId == projectId)
                    .Select(p => new { p.ProjInforId, p.LastSubmitTime })
                    .FirstOrDefaultAsync();

                if (currentProject == null)
                    return false;

                // 检查项目是否有新的提交时间（表示进入了新的审核阶段）
                if (currentProject.LastSubmitTime.HasValue && currentProject.LastSubmitTime > lastAlertTime)
                {
                    // 清空提醒列表，让项目可以重新提醒
                    ClearProjectFromAlertList(projectId);
                    return true;
                }

                // 检查项目状态是否发生了变化（ProjInforId变化）
                // 获取最后一次提醒时的项目状态
                var lastAlertStatus = await GetProjectStatusAtTimeAsync(projectId, lastAlertTime , currentProject.ProjInforId.GetValueOrDefault());
                
                // 如果当前状态与最后一次提醒时的状态不同，说明状态发生了变化
                if (lastAlertStatus.HasValue && lastAlertStatus != currentProject.ProjInforId)
                {
                    // 清空提醒列表，让项目可以重新提醒
                    ClearProjectFromAlertList(projectId);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"检查项目状态变化出错: {ex.Message}");
                Console.WriteLine($"检查项目状态变化出错: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 从提醒列表中清除项目
        /// </summary>
        private void ClearProjectFromAlertList(int projectId)
        {
            _alertedProjects.Remove(projectId);
            if (_alertTimes.ContainsKey(projectId))
            {
                _alertTimes.Remove(projectId);
            }
        }

        /// <summary>
        /// 获取项目在指定时间点的状态
        /// </summary>
        private async Task<int?> GetProjectStatusAtTimeAsync(int projectId, DateTime targetTime, int ProjInforId)
        {
            try
            {
                // 查询项目状态变更历史记录
                var statusHistory = await _context.InspectionRecord
                    .Where(ir => ir.ProjectsId == projectId && ir.CheckTime <= targetTime && ir.projId == ProjInforId)
                    .OrderByDescending(ir => ir.CheckTime)
                    .Select(ir => new { ir.projId, ir.CheckTime })
                    .FirstOrDefaultAsync();

                if (statusHistory != null)
                {
                    return statusHistory.projId;
                }
                else
                {
                    return null;
                }

                //// 如果没有历史记录，返回项目当前状态
                //var project = await _context.Projects
                //    .Where(p => p.ProjectsId == projectId)
                //    .Select(p => p.ProjInforId)
                //    .FirstOrDefaultAsync();

                //return project;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取项目历史状态出错: {ex.Message}");
                Console.WriteLine($"获取项目历史状态出错: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 检查项目是否需要提醒
        /// </summary>
        private async Task<bool> CheckIfNeedsAlertAsync(int projectId, int currentUserId)
        {
            try
            {
                // 获取项目的完整信息，包括当前状态和提交时间
                var project = await _context.Projects
                    .Where(p => p.ProjectsId == projectId)
                    .Select(p => new { p.LastSubmitTime, p.ProjInforId })
                    .FirstOrDefaultAsync();

                if (project == null)
                    return false;

                // 获取当前用户对该项目的最后一次审批记录
                var lastApproval = await _context.InspectionRecord
                    .Where(ir => ir.ProjectsId == projectId && ir.CheckPeopleId == currentUserId)
                    .OrderByDescending(ir => ir.CheckTime)
                    .FirstOrDefaultAsync();

                // 获取最后一次提醒的时间和状态
                lastdatetime = GetLastAlertTime(projectId);
                
                // 检查项目状态是否发生了变化（进入新的审核阶段）
                bool isNewStatus = await CheckIfProjectStatusChangedAsync(projectId, lastdatetime);
                
                // 如果项目从未被提醒过，需要提醒
                if (!_alertedProjects.Contains(projectId))
                {
                    _alertedProjects.Add(projectId);
                    UpdateAlertTime(projectId, DateTime.Now); // 设置提醒时间
                    return true;
                }

                // 如果项目状态发生了变化（进入新的审核阶段），需要重新提醒
                if (isNewStatus)
                {
                    // 清空提醒列表，让项目可以重新提醒
                    ClearProjectFromAlertList(projectId);
                    return true;
                }

                // 如果项目已经被提醒过，检查是否被重新提交
                if (lastApproval != null && lastApproval.CheckResult == "Rejection")
                {
                    // 如果项目有重新提交时间，并且重新提交时间晚于最后一次驳回时间
                    if (project.LastSubmitTime.HasValue && project.LastSubmitTime > lastApproval.CheckTime)
                    {
                        // 检查是否已经提醒过这次重新提交
                        if (lastdatetime < project.LastSubmitTime)
                        {
                            return true; // 需要重新提醒
                        }
                    }
                }

                // 其他情况不需要提醒
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"检查提醒需求出错: {ex.Message}");
                Console.WriteLine($"检查提醒需求出错: {ex.Message}");
                return false;
            }
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
                MessageBox.Show($"获取当前用户ID出错: {ex.Message}");
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

                // 4. 检查审批记录 - 重新获取最新的项目状态
                var currentProjectStatus = await _context.Projects
                    .Where(p => p.ProjectsId == projectId)
                    .Select(p => p.ProjInforId)
                    .FirstOrDefaultAsync();
                
                var inspectionRecord = await _context.InspectionRecord
                    .Where(ir => ir.ProjectsId == projectId && ir.projId == currentProjectStatus)
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
                        // 检查当前用户是否已经审批过（第一顺位也需要检查重新提交）
                        var currentUserApproval1 = await _context.InspectionRecord
                            .Where(ir => ir.ProjectsId == projectId &&
                                        ir.projId == currentProjectStatus &&
                                        ir.CheckPeopleId == currentUserId)
                            .OrderByDescending(ir => ir.CheckTime)
                            .FirstOrDefaultAsync();

                        if (currentUserApproval1 != null)
                        {
                            // 如果当前用户已经审批过，检查审批结果
                            if (currentUserApproval1.CheckResult == "PASS")
                            {
                                // 如果之前已经通过，丢弃结果（不能重复审批）
                                return false;
                            }
                            else if (currentUserApproval1.CheckResult == "Rejection")
                            {
                                // 如果之前驳回过，检查项目是否重新提交
                                var projectInfo = await _context.Projects
                                    .Where(p => p.ProjectsId == projectId)
                                    .Select(p => new { p.LastSubmitTime })
                                    .FirstOrDefaultAsync();

                                if (projectInfo != null && projectInfo.LastSubmitTime.HasValue)
                                {
                                    // 如果项目重新提交的时间晚于驳回时间，说明是第二次提交，需要重新审批
                                    if (projectInfo.LastSubmitTime > currentUserApproval1.CheckTime)
                                    {
                                        return true; // 需要重新审批
                                    }
                                }

                                // 项目未重新提交，不能再次审批
                                return false;
                            }
                        }

                        // 如果是第一顺位且没有审批记录，可以审批
                        return true;
                    }

                    // 查找前一顺位审批人的审批结果
                    var previousSeq = currentUserSeq - 1;
                    var previousApproverId = await _context.TypeApprFlowPersSeqTable
                        .Where(x => x.equipmenttypeId == project.equipmenttypeId && x.Sequence == previousSeq)
                        .Select(x => x.ReviewerPeopleId)
                        .FirstOrDefaultAsync();

                    var previousApproval = await _context.InspectionRecord
                        .Where(ir => ir.ProjectsId == projectId &&
                                    ir.projId == currentProjectStatus &&
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
                                    ir.projId == currentProjectStatus &&
                                    ir.CheckPeopleId == currentUserId)
                        .OrderByDescending(ir => ir.CheckTime)
                        .FirstOrDefaultAsync();

                    if (currentUserApproval != null)
                    {
                        // 如果当前用户已经审批过，检查审批结果
                        if (currentUserApproval.CheckResult == "PASS")
                        {
                            // 如果之前已经通过，丢弃结果（不能重复审批）
                            return false;
                        }
                        else if (currentUserApproval.CheckResult == "Rejection")
                        {
                            // 如果之前驳回过，检查项目是否重新提交
                            var projectInfo = await _context.Projects
                                .Where(p => p.ProjectsId == projectId)
                                .Select(p => new { p.LastSubmitTime })
                                .FirstOrDefaultAsync();

                            if (projectInfo != null && projectInfo.LastSubmitTime.HasValue)
                            {
                                //MessageBox.Show(projectInfo.LastSubmitTime.ToString() + "----------" + currentUserApproval.CheckTime.ToString());
                                
                                // 如果项目重新提交的时间晚于驳回时间，说明是第二次提交，需要重新审批
                                if (projectInfo.LastSubmitTime > currentUserApproval.CheckTime)
                                {
                                    return true; // 需要重新审批
                                }
                            }

                            // 项目未重新提交，不能再次审批
                            return false;
                        }
                    }

                    // 如果当前用户没有审批过，可以审批
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("审批权限检查出错");
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
                    MessageBox.Show("无法获取当前用户ID，跳过检查");
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
                        // 检查审批权限和流程条件
                        var canApprove = await CheckApprovalPermissionAsync(project.ProjectId, currentUserId.Value);

                        if (canApprove)
                        {
                            // 检查是否需要提醒
                            bool needsAlert = await CheckIfNeedsAlertAsync(project.ProjectId, currentUserId.Value);
                            
                            if (needsAlert)
                            {
                                validAlertProjects.Add(project);
                                // 设置提醒时间为当前时间
                                UpdateAlertTime(project.ProjectId, DateTime.Now);
                            }
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
                MessageBox.Show("$\"项目监控出错: {ex.Message}\"");
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
            OverView = new OverviewVM(Loginpersonnamegrade, Loginpersonname);
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
        #endregion

        #region UI_Visibility_Fun
        [ObservableProperty]
        private Visibility _vis_overview;

        [ObservableProperty]
        private Visibility _vis_newproject;

        [RelayCommand]
        private void OverViewFun()
        {
            Vis_overview = Visibility.Visible;
            Vis_newproject = Visibility.Collapsed;
            
            OverView = new OverviewVM(Loginpersonnamegrade, Loginpersonname);
        }

        [RelayCommand]
        private void NewProjectFun()
        {
            Vis_overview = Visibility.Collapsed;
            Vis_newproject = Visibility.Visible;
        }
        #endregion



    }
}
