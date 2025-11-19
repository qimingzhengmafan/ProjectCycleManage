using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using OpenTK.Graphics.OpenGL;
using ProjectCycleManage.Model;
using ProjectCycleManage.View;
using ProjectManagement.Data;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectCycleManage.ViewModel
{
    public partial class OverviewVM: ObservableObject
    {
        //ProjectName
        //RunningStatus
        //ProjectLeader
        //StartTime
        //ProjectStatus
        //ProjectStatusProgressdouble
        //private ObservableCollection<ProjectCardVM> _projectshowarea = new ObservableCollection<ProjectCardVM>();


        private ObservableCollection<InformationCardVM> _informationshowarea = new ObservableCollection<InformationCardVM>();

        private ObservableCollection<ProjectListItem> _projectList = new ObservableCollection<ProjectListItem>();

        [ObservableProperty]
        private string _stageprojectname;

        [ObservableProperty]
        private double _stageprogress;

        [ObservableProperty]
        private string _stagename;

        // 阶段图标颜色状态
        [ObservableProperty]
        private string _stage1Color = "#E0E0E0";

        [ObservableProperty]
        private string _stage2Color = "#E0E0E0";

        [ObservableProperty]
        private string _stage3Color = "#E0E0E0";

        [ObservableProperty]
        private string _stage4Color = "#E0E0E0";

        [ObservableProperty]
        private string _stage5Color = "#E0E0E0";

        // 阶段名称
        [ObservableProperty]
        private string _stage1Name = "项目评审";

        [ObservableProperty]
        private string _stage2Name = "设备采购";

        [ObservableProperty]
        private string _stage3Name = "预验收";

        [ObservableProperty]
        private string _stage4Name = "验收";

        [ObservableProperty]
        private string _stage5Name = "完成";

        [ObservableProperty]
        private string _loginpersonname;

        [ObservableProperty]
        private string _projectstage;
        /// <summary>
        /// 登录者等级
        /// </summary>
        [ObservableProperty]
        private int _loginpersonnamegrade;

        [ObservableProperty]
        private string _currentProjectId;

        [ObservableProperty]
        private int _totalProjects;

        [ObservableProperty]
        private int _completedProject;

        //InProgress
        [ObservableProperty]
        private int _inProgress;

        //UnderReview
        [ObservableProperty]
        private int _underReview;

        //Pause
        [ObservableProperty]
        private int _pause;

        //CompletionRate
        [ObservableProperty]
        private double _completionRate;

        //public ObservableCollection<ProjectCardVM> ProjectShowAreaCard
        //{
        //    get => _projectshowarea;
        //    set => _projectshowarea = value;
        //}

        /// <summary>
        /// 左侧项目列表
        /// </summary>
        public ObservableCollection<ProjectListItem> ProjectList
        {
            get => _projectList;
            set
            {
                _projectList = value;
                OnPropertyChanged();
            }
        }

        // 阶段图标点击命令
        [RelayCommand]
        private void Stage1Clicked()
        {
            // 处理阶段1点击逻辑
            //UpdateStageColors(1);
            MessageBox.Show("1");
        }

        [RelayCommand]
        private void Stage2Clicked()
        {
            // 处理阶段2点击逻辑
            //UpdateStageColors(2);
            MessageBox.Show("2");
        }

        [RelayCommand]
        private void Stage3Clicked()
        {
            // 处理阶段3点击逻辑
            //UpdateStageColors(3);
            MessageBox.Show("3");
        }

        [RelayCommand]
        private void Stage4Clicked()
        {
            // 处理阶段4点击逻辑
            //UpdateStageColors(4);
            MessageBox.Show("4");
        }

        [RelayCommand]
        private void Stage5Clicked()
        {
            // 处理阶段5点击逻辑
            //UpdateStageColors(5);
            MessageBox.Show("5");
        }

        [RelayCommand]
        private async Task SubmitApproval()
        {
            if (string.IsNullOrEmpty(CurrentProjectId))
            {
                MessageBox.Show("请先选择项目");
                return;
            }

            var ProjectPhaseStatusId = await ConfirmationStatusAsync();
            if (ProjectPhaseStatusId == null)
            {
                MessageBox.Show("前道信息缺失，请先补全前道信息");
                return;
            }
            //未启动
            else if (ProjectPhaseStatusId == 101)
            {
                MessageBox.Show("前道信息填写错误，请重新填写");
                return;
            }
            //暂停
            else if (ProjectPhaseStatusId == 103)
            {
                MessageBox.Show("当前项目暂停中，无需提交");
                return;

            }
            //已完成
            else if (ProjectPhaseStatusId == 104)
            {
                MessageBox.Show("当前项目已完成，无需提交");
                return;
            }

            await ExecuteApprovalProcessAsync();
        }

        [RelayCommand]
        private async Task RejectApproval()
        {
            using var context = new ProjectContext();
            
            // 检查审批权限和流程状态
            var (hasApprovalPermission, isInApprovalFlow) = await CheckApprovalPermissionAndFlowStatusAsync(context);
            
            if (!hasApprovalPermission)
            {
                MessageBox.Show("您没有审批权限！");
                return;
            }
            
            if (!isInApprovalFlow)
            {
                MessageBox.Show("当前项目不在审批流程中！");
                return;
            }
            
            // 检查当前用户是否为第一顺位审批人
            var isFirstApprover = await CheckIfFirstApproverAsync(context);
            
            if (!isFirstApprover)
            {
                // 检查前序审批结果
                var previousResult = await CheckPreviousApprovalResultAsync(context);
                
                if (previousResult != "PASS")
                {
                    MessageBox.Show("前序审批未通过，无法进行驳回！");
                    return;
                }
            }
            
            // 检查是否已有审批记录
            var existingRecord = await CheckInspectionRecordAsync(context);
            
            if (existingRecord != null)
            {
                // 如果当前用户已经审批过，检查是否是第二次提交
                if (existingRecord.CheckResult == "PASS")
                {
                    MessageBox.Show("您已经审批通过此项目，无法再次驳回！");
                    return;
                }
                else if (existingRecord.CheckResult == "Rejection")
                {
                    // 如果是驳回状态，检查项目是否重新提交
                    var projectId = Convert.ToInt32(CurrentProjectId);
                    var project = await context.Projects
                        .FirstOrDefaultAsync(p => p.ProjectsId == projectId);
                    
                    if (project != null && project.LastSubmitTime.HasValue)
                    {
                        // 如果项目重新提交的时间晚于驳回时间，说明是第二次提交，可以重新驳回
                        if (project.LastSubmitTime > existingRecord.CheckTime)
                        {
                            // 允许重新驳回
                            //MessageBox.Show("项目已重新提交，可以重新驳回！");
                        }
                        else
                        {
                            MessageBox.Show("您已经驳回过此项目！");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("您已经驳回过此项目！");
                        return;
                    }
                }
            }
            
            // 写入驳回结果
            await WriteRejectionResultAsync(context);
            
            MessageBox.Show("项目已驳回！");
        }

        [RelayCommand]
        private async Task TestWriteApprovalRecord()
        {
            if (string.IsNullOrEmpty(CurrentProjectId))
            {
                MessageBox.Show("请先选择项目");
                return;
            }

            try
            {
                using var context = new ProjectContext();
                await WriteApprovalResultAsync(context);
                MessageBox.Show("测试审批记录写入成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"测试审批记录写入失败：{ex.Message}");
            }
        }

        private async Task ExecuteApprovalProcessAsync()
        {
            try
            {
                using var context = new ProjectContext();
                
                // 1. 检查审批资格和流程状态
                var (hasApprovalPermission, isInApprovalFlow) = await CheckApprovalPermissionAndFlowStatusAsync(context);
                //isInApprovalFlow是否为指定流程
                //hasApprovalPermission是否具有审批资格
                // 根据检查结果处理四种分支情况
                if (!hasApprovalPermission && !isInApprovalFlow)
                {
                    // 不具有审批资格且不在审批流程：更新projects表的ProjInforId字段（当前值+1）
                    await UpdateProjectProjInforIdAsync(context);
                    

                    MessageBox.Show("项目流程已更新");
                    return;
                }
                else if (!hasApprovalPermission && isInApprovalFlow)
                {
                    //在审批流程，但不具备审批资格
                    MessageBox.Show("审批中，请催一催审批再点击");
                    return;
                }
                else if (hasApprovalPermission && !isInApprovalFlow)
                {

                    MessageBox.Show("未到审批流程");
                    return;
                }
                
                // 具有审批资格且在审批流程：继续下一步
                
                // 2. 检查当前登录人员是否已经审批过
                var currentUserId = await GetCurrentUserIdAsync(context);
                var projectId = Convert.ToInt32(CurrentProjectId);
                
                // 获取当前项目信息
                var project = await context.Projects
                    .Include(p => p.ProjectStage)
                    .FirstOrDefaultAsync(p => p.ProjectsId == projectId);
                
                if (project == null)
                {
                    MessageBox.Show("项目不存在");
                    return;
                }
                
                // 检查当前登录人员是否已经有审批记录
                var currentUserApprovalRecord = await context.InspectionRecord
                    .OrderByDescending(ir => ir.CheckTime)
                    .FirstOrDefaultAsync(ir => ir.ProjectsId == projectId 
                                             && ir.projId == project.ProjInforId 
                                             && ir.CheckPeopleId == currentUserId);


                if (currentUserApprovalRecord != null)
                {
                    // 当前登录人员已经有审批记录
                    if (currentUserApprovalRecord.CheckResult == "PASS")
                    {
                        MessageBox.Show("审批完成，无需重复审批");
                        return;
                    }
                    else if (currentUserApprovalRecord.CheckResult == "Rejection")
                    {
                        // 如果是拒绝状态，可以继续审批
                        //MessageBox.Show("继续审批流程");
                    }
                }
                
                // 3. 检查审批记录
                var inspectionRecord = await CheckInspectionRecordAsync(context);
                
                if (inspectionRecord == null)
                {
                    // 不存在审批记录：确认当前登录人是否为第一顺位审批人
                    var isFirstApprover = await CheckIfFirstApproverAsync(context);
                    if (!isFirstApprover)
                    {
                        // 存在审批记录：检查前序审批结果
                        var previousApprovalResult = await CheckPreviousApprovalResultAsync(context);
                        if (previousApprovalResult != "PASS")
                        {
                            MessageBox.Show("未到审批流程");
                            return;
                        }
                        //MessageBox.Show("未到审批流程");
                        //return;
                    }

                    // 如果是第一顺位审批人，直接进入步骤6（写入审批结果）
                }
                else
                {
                    // 存在审批记录：检查前序审批结果
                    var previousApprovalResult = await CheckPreviousApprovalResultAsync(context);
                    if (previousApprovalResult != "PASS")
                    {
                        MessageBox.Show("未到审批流程");
                        return;
                    }
                }
                
                // 5. 写入审批结果
                await WriteApprovalResultAsync(context);
                MessageBox.Show("审批完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"审批流程执行失败：{ex.Message}");
            }
        }


        private async Task<int?> ConfirmationStatusAsync()
        {
            using var context = new ProjectContext();
            int ID = Convert.ToInt32(CurrentProjectId);

            var project = await context.Projects
                .Include(p => p.ProjectPhaseStatus)
                .FirstOrDefaultAsync(ir => ir.ProjectsId == ID);

            return project.ProjectPhaseStatusId;
        }


        public ObservableCollection<InformationCardVM> InformationCardArea
        {
            get => _informationshowarea;
            set => _informationshowarea = value;
        }
        
        public OverviewVM(int loginpeoplegrade ,string loginpeoplename)
        {
            Loginpersonnamegrade = loginpeoplegrade;
            Loginpersonname = loginpeoplename;

            Stageprogress = 0.0;

            
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                //ProjectShowAreaCard.Clear();
                ProjectList.Clear();
            }));

            Task.Run(() =>
            {
                // 创建数据库上下文
                using var context = new ProjectContext();
                
                // 获取当前登录人的ID
                var currentUser = context.PeopleTable
                    .FirstOrDefault(p => p.PeopleName == loginpeoplename);
                
                if (currentUser == null)
                {
                    // 如果找不到当前用户，不加载任何项目
                    return;
                }

                // 检查当前用户是否在typeapprflowpersseqtable的ReviewerPeopleId中
                var isReviewer = context.TypeApprFlowPersSeqTable
                    .Any(t => t.ReviewerPeopleId == currentUser.PeopleId && t.Mark != "Dele");

                IQueryable<Projects> projectsQuery;

                if (isReviewer)
                {
                    // 如果是审核人，显示当前年份全部项目
                    projectsQuery = context.Projects
                        .Where(p => p.Year == DateTime.Now.Year);
                }
                else
                {
                    // 如果不是审核人，只显示当前用户负责或跟进的项目
                    projectsQuery = context.Projects
                        .Where(p => p.Year == DateTime.Now.Year &&
                                  (p.ProjectLeaderId == currentUser.PeopleId || 
                                   p.projectfollowuppersonId == currentUser.PeopleId));
                }

                // 执行查询
                var projectsdata = projectsQuery
                    .Include(p => p.ProjectStage)
                    .Include(p => p.type)
                    .Include(p => p.ProjectPhaseStatus)
                    .Include(p => p.ProjectLeader)
                    .ToList();

                // 输出结果
                foreach (var project in projectsdata)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        //ProjectShowAreaCard.Add(new ProjectCardVM()
                        //{
                        //    Projectsid = project.ProjectsId.ToString(),
                        //    Projectname = project.ProjectName,
                        //    Projectleader = project.ProjectLeader.PeopleName,
                        //    Projectstatus = project.ProjectStage.ProjectStageName,
                        //    Projectstatusprogressdouble = (double)project.FileProgress.GetValueOrDefault(),
                        //    Runningstatus = project.ProjectPhaseStatus.ProjectPhaseStatusName,
                        //    Starttimme = project.ApplicationTime,
                        //    ViewDetailsaction = GetProjectsDatas
                        //});

                        // 添加到左侧项目列表
                        var listItem = new ProjectListItem()
                        {
                            ProjectId = project.ProjectsId.ToString(),
                            ProjectName = project.ProjectName,
                            ProjectLeader = project.ProjectLeader.PeopleName,
                            StartTime = project.ApplicationTime ?? DateTime.MinValue,
                            StatusText = project.ProjectPhaseStatus.ProjectPhaseStatusName,
                            StatusBackground = GetStatusBackground(project.ProjectPhaseStatus.ProjectPhaseStatusName),
                            StatusForeground = GetStatusForeground(project.ProjectPhaseStatus.ProjectPhaseStatusName),
                            IsSelected = false
                        };
                        listItem.OnSelected = SelectProject;
                        ProjectList.Add(listItem);
                    }));
                    
                }
            });


            Task.Run(() =>
            {
                using var context = new ProjectContext();
                var TotalProjectNum = context.Projects
                    .Where(p => p.Year == DateTime.Now.Year)
                    .Count();

                TotalProjects = TotalProjectNum;
                
                CompletedProject = context.Projects
                    .Where( p => p.Year == DateTime.Now.Year && p.ProjectPhaseStatusId == 104)
                    .Count();

                CompletionRate = Math.Round((double)CompletedProject / (double)TotalProjects * 100.0 , 0);
                
            });

            Task.Run(() =>
            {
                using var context = new ProjectContext();
                var excludedIds = new HashSet<int> { 101, 103, 105, 107, 109, 111 };
                var currentYear = DateTime.Now.Year;

                //InProgress = context.Projects
                //    .Where(p => p.Year == currentYear && !excludedIds.Contains(p.ProjInforId.GetValueOrDefault()))
                //    .Count();

                InProgress = context.Projects
                    .Where(p => p.Year == currentYear && p.ProjectPhaseStatusId == 102)
                    .Count();

                //UnderReview = context.Projects
                //    .Where(p => p.Year == currentYear && excludedIds.Contains(p.ProjInforId.GetValueOrDefault()))
                //    .Count();


                UnderReview = context.Projects
                    .Where(p => p.Year == currentYear && p.ProjectPhaseStatusId == 105)
                    .Count();
                Pause = context.Projects
                    .Where(p => p.Year == currentYear && p.ProjectPhaseStatusId == 103)
                    .Count();

            });

        }

        /// <summary>
        /// 根据项目状态获取背景色
        /// </summary>
        private string GetStatusBackground(string status)
        {
            switch (status)
            {
                case "进行中":
                    return "#FF4CAF50"; // 绿色
                case "已完成":
                    return "#FF2196F3"; // 蓝色
                case "暂停":
                    return "#f44336"; 
                case "审核中":
                    return "#FF9800";
                default:
                    return "#FFE0E0E0"; // 默认浅灰色
            }
        }

        /// <summary>
        /// 根据项目状态获取前景色
        /// </summary>
        private string GetStatusForeground(string status)
        {
            switch (status)
            {
                case "进行中":
                case "已完成":
                case "已暂停":
                case "已取消":
                    return "#FFFFFFFF"; // 白色
                default:
                    return "#FF000000"; // 默认黑色
            }
        }

        /// <summary>
        /// 更新阶段图标颜色
        /// </summary>
        /// <param name="clickedStage">被点击的阶段编号（0表示初始化）</param>
        private void UpdateStageColors(int clickedStage)
        {
            // 根据进度条位置确定哪些阶段应该显示为绿色
            var currentProgress = Stageprogress;
            
            // 重置所有阶段颜色为灰色
            Stage1Color = "#E0E0E0";
            Stage2Color = "#E0E0E0";
            Stage3Color = "#E0E0E0";
            Stage4Color = "#E0E0E0";
            Stage5Color = "#E0E0E0";
            
            // 根据进度条位置设置绿色阶段
            if (currentProgress >= 10) Stage1Color = "#4CAF50";
            if (currentProgress >= 30) Stage2Color = "#4CAF50";
            if (currentProgress >= 50) Stage3Color = "#4CAF50";
            if (currentProgress >= 70) Stage4Color = "#4CAF50";
            if (currentProgress >= 90) Stage5Color = "#4CAF50";
            
            // 如果点击的阶段在当前进度之前，将其设置为绿色（仅在点击时）
            if (clickedStage > 0)
            {
                if (clickedStage == 1 && currentProgress >= 10) Stage1Color = "#4CAF50";
                if (clickedStage == 2 && currentProgress >= 30) Stage2Color = "#4CAF50";
                if (clickedStage == 3 && currentProgress >= 50) Stage3Color = "#4CAF50";
                if (clickedStage == 4 && currentProgress >= 70) Stage4Color = "#4CAF50";
                if (clickedStage == 5 && currentProgress >= 90) Stage5Color = "#4CAF50";
            }
        }

        /// <summary>
        /// 选择项目
        /// </summary>
        private void SelectProject(ProjectListItem selectedItem)
        {
            // 取消所有项目的选中状态
            foreach (var item in ProjectList)
            {
                item.IsSelected = false;
            }

            // 设置当前项目为选中状态
            selectedItem.IsSelected = true;

            // 设置当前项目ID
            CurrentProjectId = selectedItem.ProjectId;

            // 触发项目详情加载
            GetProjectsDatas(selectedItem.ProjectId, selectedItem.StatusText);
        }

        public void GetProjectsDatas(string data , string projectstage)
        {
            InformationCardArea.Clear();
            CurrentProjectId = data;
            Projectstage = projectstage;
            //MessageBox.Show("overviewvm" + data);
            Task.Run(() =>
            {
                using (var context = new ProjectContext())
                {
                    var projectinfor = context.Projects
                    .Where(p => p.ProjectsId == Convert.ToInt32(data))
                    .Include(p => p.ProjectStage)
                    .Include(p => p.type)
                    .Include(p => p.ProjectPhaseStatus)
                    .Include(p => p.ProjectLeader)
                    .FirstOrDefault();
                    Stageprojectname = projectinfor.ProjectName;
                    if (projectinfor.ProjectPhaseStatusId == 104)
                    {
                        Stageprogress = 100.0;
                    }
                    else
                    {
                        switch (projectinfor.ProjectStageId)
                        {
                            case 101:
                                Stageprogress = 10.0;
                                break;

                            case 102:
                                Stageprogress = 30.0;
                                break;

                            case 103:
                                Stageprogress = 50.0;
                                break;

                            case 104:
                                Stageprogress = 70.0;
                                break;

                            case 105:
                                Stageprogress = 90.0;
                                break;

                            default:
                                Stageprogress = 0.0;
                                break;
                        }
                    }
                    // 初始化阶段图标颜色
                    UpdateStageColors(0);

                    // 查询该项目阶段所需的文档
                    var requiredDocuments = context.EquipTypeStageDocTable
                        .Where(etsd => etsd.equipmenttypeId == projectinfor.equipmenttypeId
                                    && etsd.ProjectStageId == projectinfor.ProjectStageId)
                        .Include(etsd => etsd.documenttype)
                        .Select(etsd => new
                        {
                            //projectid = etsd.
                            DocumentTypeId = etsd.documenttype.DocumentTypeId,
                            DocumentTypeName = etsd.documenttype.DocumentTypeName,
                            Permission = etsd.documenttype.Permission,
                            FileTypesDataname = etsd.documenttype.FileTypesData.FileTypesName
                        })
                        .ToList();

                    // 查询该项目阶段所需的信息
                    var requiredInformation = context.EquipTypeStageInfoTable
                        .Where(etsi => etsi.equipmenttypeId == projectinfor.equipmenttypeId
                                     && etsi.ProjectStageId == projectinfor.ProjectStageId)
                        .Include(etsi => etsi.Information)
                        .ThenInclude(info => info.InforTypesData)
                        .Select(etsi => new
                        {
                            InformationId = etsi.Information.Id,
                            InformationName = etsi.Information.Reamrks,
                            Permission = etsi.Information.Permission,
                            //Remarks = etsi.Information.Reamrks,
                            InformationType = etsi.Information.InforTypesData.FileTypesName
                        })
                        .ToList();

                    if (requiredDocuments.Count != 0)
                    {
                        foreach (var item in requiredDocuments)
                        {

                            switch (item.FileTypesDataname)
                            {
                                case "文档":
                                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        InformationCardArea.Add(new InformationCardVM()
                                        {
                                            Inforprojectid = data,
                                            Filename = item.DocumentTypeName,
                                            Loginpersonnamegrade = Loginpersonnamegrade,
                                            Taginfor = item.Permission,

                                            Infortype = item.FileTypesDataname,
                                        });
                                    }));
                                    break;

                                case "文档-OA":
                                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        InformationCardArea.Add(new InformationCardVM()
                                        {
                                            Inforprojectid = data,
                                            File_oa_indata = item.DocumentTypeName,
                                            Loginpersonnamegrade = Loginpersonnamegrade,
                                            Taginfor = item.Permission,
                                            Infortype = item.FileTypesDataname,
                                        });
                                    }));
                                    break;

                                default:
                                    break;
                            }



                        }
                    }

                    if (requiredInformation.Count != 0)
                    {
                        foreach (var item in requiredInformation)
                        {

                            switch (item.InformationType)
                            {

                                case "信息-下拉框":
                                    switch (item.InformationName)
                                    {
                                        case string value when(value == "项目负责人" || value == "跟进人"):
                                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                            {
                                                InformationCardArea.Add(new InformationCardVM()
                                                {
                                                    Inforprojectid = data,
                                                    Infor_people = item.InformationName,
                                                    Loginpersonnamegrade = Loginpersonnamegrade,
                                                    Taginfor = item.Permission,
                                                    Infortype = item.InformationType,

                                                });
                                            }));
                                            break;

                                        case "设备类型":
                                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                            {
                                                InformationCardArea.Add(new InformationCardVM()
                                                {
                                                    Inforprojectid = data,
                                                    Infor_equipmenttype = item.InformationName,
                                                    Loginpersonnamegrade = Loginpersonnamegrade,
                                                    Taginfor = item.Permission,
                                                    Infortype = item.InformationType,

                                                });
                                            }));
                                            break;

                                        case "类型":
                                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                            {
                                                InformationCardArea.Add(new InformationCardVM()
                                                {
                                                    Inforprojectid = data,
                                                    Infor_type = item.InformationName,
                                                    Loginpersonnamegrade = Loginpersonnamegrade,
                                                    Taginfor = item.Permission,
                                                    Infortype = item.InformationType,

                                                });
                                            }));
                                            break;

                                        case "项目阶段":
                                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                            {
                                                InformationCardArea.Add(new InformationCardVM()
                                                {
                                                    Inforprojectid = data,
                                                    Infor_projectstage = item.InformationName,
                                                    Loginpersonnamegrade = Loginpersonnamegrade,
                                                    Taginfor = item.Permission,
                                                    Infortype = item.InformationType,

                                                });
                                            }));
                                            break;

                                        case " 当前进度（例：进行中）":
                                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                            {
                                                InformationCardArea.Add(new InformationCardVM()
                                                {
                                                    Inforprojectid = data,
                                                    Infor_projectphasestatus = item.InformationName,
                                                    Loginpersonnamegrade = Loginpersonnamegrade,
                                                    Taginfor = item.Permission,
                                                    Infortype = item.InformationType,

                                                });
                                            }));
                                            break;

                                    }

                                    break;

                                case "信息-填写":
                                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        InformationCardArea.Add(new InformationCardVM()
                                        {
                                            Inforprojectid = data,
                                            Infor_text_in = item.InformationName,
                                            Loginpersonnamegrade = Loginpersonnamegrade,
                                            Taginfor = item.Permission,
                                            Infortype = item.InformationType,
                                        });
                                    }));
                                    break;

                                case "信息-日期":
                                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        InformationCardArea.Add(new InformationCardVM()
                                        {
                                            Inforprojectid = data,
                                            Infor_date_in = item.InformationName,
                                            Loginpersonnamegrade = Loginpersonnamegrade,
                                            Taginfor = item.Permission,
                                            Infortype = item.InformationType,
                                        });
                                    }));
                                    break;

                                default:
                                    break;
                            }



                        }
                    }
                }
            });
            


        }

        private async Task<(bool hasApprovalPermission, bool isInApprovalFlow)> CheckApprovalPermissionAndFlowStatusAsync(ProjectContext context)
        {
            var projectId = Convert.ToInt32(CurrentProjectId);
            
            // 获取当前项目信息
            var project = await context.Projects
                .Include(p => p.ProjectStage)
                .FirstOrDefaultAsync(p => p.ProjectsId == projectId);
            
            if (project == null)
            {
                return (false, false);
            }
            
            // 检查当前项目流程是否为指定流程（101,103,105,107,109,111）
            var specifiedFlows = new[] { 101, 103, 105, 107, 109, 111 };
            var isInApprovalFlow = specifiedFlows.Contains(project.ProjInforId.GetValueOrDefault());
            
            // 获取当前登录人ID
            var currentUserId = await GetCurrentUserIdAsync(context);
            
            // 查询typeappflowpersseqtable表，确认当前登录人是否具有审批资格
            var hasApprovalPermission = await context.TypeApprFlowPersSeqTable
                .AnyAsync(t => t.equipmenttypeId == project.equipmenttypeId 
                             && t.ReviewerPeopleId == currentUserId 
                             && t.Mark != "Dele");
            
            return (hasApprovalPermission, isInApprovalFlow);
        }

        private async Task<int> GetCurrentUserIdAsync(ProjectContext context)
        {
            // 根据登录名获取用户ID
            var currentUser = await context.PeopleTable
                .FirstOrDefaultAsync(p => p.PeopleName == Loginpersonname);
            
            return currentUser?.PeopleId ?? 0;
        }

        private async Task SubmitApprovalAsync(ProjectContext context)
        {
            var projectId = Convert.ToInt32(CurrentProjectId);
            
            // 获取当前项目信息
            var project = await context.Projects
                .Include(p => p.ProjectStage)
                .FirstOrDefaultAsync(p => p.ProjectsId == projectId);
            
            if (project == null)
            {
                return;
            }
            
            // 更新最后一次提交审核时间
            project.LastSubmitTime = DateTime.Now;
            
            // 根据当前流程状态，设置下一步的流程状态
            // 这里需要根据实际业务逻辑来确定下一步的流程状态
            // 目前暂时保持原状态，实际应用中可能需要更新到下一个审批阶段
            
            // 示例：如果当前是101，则可能需要更新到下一个状态
            // 这里需要根据具体的业务规则来实现
            
            // 保存项目状态变更
            await context.SaveChangesAsync();
        }

        private async Task<InspectionRecord> CheckInspectionRecordAsync(ProjectContext context)
        {
            var projectId = Convert.ToInt32(CurrentProjectId);
            
            // 获取当前项目信息
            var project = await context.Projects
                .Include(p => p.ProjectStage)
                .FirstOrDefaultAsync(p => p.ProjectsId == projectId);
            
            if (project == null)
            {
                return null;
            }
            
            // 获取当前登录人ID
            var currentUserId = await GetCurrentUserIdAsync(context);
            
            // 使用当前项目ID、流程ID和当前用户ID查询InspectionRecord表
            var inspectionRecord = await context.InspectionRecord
                .OrderByDescending(ir => ir.CheckTime)
                .FirstOrDefaultAsync(ir => ir.ProjectsId == projectId 
                                         && ir.projId == project.ProjInforId
                                         && ir.CheckPeopleId == currentUserId);
            
            return inspectionRecord;
        }

        private async Task<bool> CheckIfFirstApproverAsync(ProjectContext context)
        {
            var projectId = Convert.ToInt32(CurrentProjectId);
            
            // 获取当前项目信息
            var project = await context.Projects
                .FirstOrDefaultAsync(p => p.ProjectsId == projectId);
            
            if (project == null)
            {
                return false;
            }
            
            // 获取当前登录人ID
            var currentUserId = await GetCurrentUserIdAsync(context);
            
            // 查询第一顺位审批人
            var firstApprover = await context.TypeApprFlowPersSeqTable
                .Where(t => t.equipmenttypeId == project.equipmenttypeId 
                         && t.Sequence == 1 
                         && t.Mark != "Dele")
                .FirstOrDefaultAsync();
            
            return firstApprover?.ReviewerPeopleId == currentUserId;
        }

        private async Task<string> CheckPreviousApprovalResultAsync(ProjectContext context)
        {
            var projectId = Convert.ToInt32(CurrentProjectId);
            
            // 获取当前项目信息
            var project = await context.Projects
                .Include(p => p.ProjectStage)
                .FirstOrDefaultAsync(p => p.ProjectsId == projectId);
            
            if (project == null)
            {
                return "FAIL";
            }
            
            // 获取当前登录人ID
            var currentUserId = await GetCurrentUserIdAsync(context);
            
            // 查询当前登录人的审批顺序
            var currentApprover = await context.TypeApprFlowPersSeqTable
                .Where(t => t.equipmenttypeId == project.equipmenttypeId 
                         && t.ReviewerPeopleId == currentUserId 
                         && t.Mark != "Dele")
                .FirstOrDefaultAsync();
            
            if (currentApprover == null || currentApprover.Sequence <= 1)
            {
                return "PASS"; // 第一顺位没有前序审批
            }
            
            // 查询前一顺位审批人
            var previousSequence = currentApprover.Sequence - 1;
            var previousApprover = await context.TypeApprFlowPersSeqTable
                .Where(t => t.equipmenttypeId == project.equipmenttypeId 
                         && t.Sequence == previousSequence 
                         && t.Mark != "Dele")
                .FirstOrDefaultAsync();
            
            if (previousApprover == null)
            {
                return "FAIL";
            }
            
            // 查询前一顺位审批人的审批结果（按时间降序排序，获取最新记录）
            var previousApproval = await context.InspectionRecord
                .Where(ir => ir.ProjectsId == projectId 
                          && ir.projId == project.ProjInforId
                          && ir.CheckPeopleId == previousApprover.ReviewerPeopleId)
                .OrderByDescending(ir => ir.CheckTime) // 按审批时间降序排序
                .FirstOrDefaultAsync();
            
            return previousApproval?.CheckResult ?? "FAIL";
        }

        private async Task WriteApprovalResultAsync(ProjectContext context)
        {
            var projectId = Convert.ToInt32(CurrentProjectId);
            
            // 获取当前项目信息
            var project = await context.Projects
                .Include(p => p.ProjectStage)
                .FirstOrDefaultAsync(p => p.ProjectsId == projectId);
            
            if (project == null)
            {
                return;
            }
            
            // 获取当前登录人ID
            var currentUserId = await GetCurrentUserIdAsync(context);
            
            // 获取当前用户审批顺序
            var sequence = await GetCurrentUserSequenceAsync(context, project.equipmenttypeId, currentUserId);
            
            // 获取数据库总条数+1作为InspectionRecordId
            var totalRecords = await context.InspectionRecord.CountAsync();
            var newInspectionRecordId = totalRecords + 1;
            
            // 创建审批记录
            var inspectionRecord = new InspectionRecord
            {
                InspectionRecordId = newInspectionRecordId,
                ProjectsId = projectId,
                CheckPeopleId = currentUserId,
                CheckTime = DateTime.Now,
                CheckResult = "PASS", // 默认通过，实际应根据审批结果设置
                CheckOpinion = "审批通过", // 可为空
                projId = project.ProjInforId.GetValueOrDefault(),
                Sequence = sequence
            };
            
            context.InspectionRecord.Add(inspectionRecord);
            
            // 检查当前登录人员是否为最后一位审批人
            var isLastApprover = await IsLastApproverAsync(context, project.equipmenttypeId, currentUserId);
            
            // 如果是最后一位审批人且ProjInforId不等于111，则将ProjInforId加1
            if (isLastApprover && project.ProjInforId.HasValue && project.ProjInforId.Value != 111)
            {
                project.ProjInforId = project.ProjInforId.Value + 1;
                project.ProjectPhaseStatusId = 102;
                //ProjectStageId更新
                if (project.ProjectStageId == 106)
                {
                    project.ProjectStageId = 101;
                }
                else
                {
                    project.ProjectStageId = project.ProjectStageId +1;
                }

                MessageBox.Show("审批完成！作为最后一位审批人，已将项目流程状态更新到下一阶段。");
            }
            else if (isLastApprover && project.ProjInforId.HasValue && project.ProjInforId.Value == 111)
            {
                project.ProjectPhaseStatusId = 104;
                MessageBox.Show("审批完成！当前项目流程状态已为最终阶段（111），无需更新。");
            }
            
            await context.SaveChangesAsync();
        }

        private async Task WriteRejectionResultAsync(ProjectContext context)
        {
            var projectId = Convert.ToInt32(CurrentProjectId);
            
            // 获取当前项目信息
            var project = await context.Projects
                .Include(p => p.ProjectStage)
                .FirstOrDefaultAsync(p => p.ProjectsId == projectId);
            
            if (project == null)
            {
                return;
            }
            
            // 获取当前登录人ID
            var currentUserId = await GetCurrentUserIdAsync(context);
            
            // 获取当前用户审批顺序
            var sequence = await GetCurrentUserSequenceAsync(context, project.equipmenttypeId, currentUserId);
            
            // 获取数据库总条数+1作为InspectionRecordId
            var totalRecords = await context.InspectionRecord.CountAsync();
            var newInspectionRecordId = totalRecords + 1;
            
            // 创建驳回记录
            var inspectionRecord = new InspectionRecord
            {
                InspectionRecordId = newInspectionRecordId,
                ProjectsId = projectId,
                CheckPeopleId = currentUserId,
                CheckTime = DateTime.Now,
                CheckResult = "Rejection", // 驳回结果
                CheckOpinion = "项目驳回", // 可为空
                projId = project.ProjInforId.GetValueOrDefault(),
                Sequence = sequence
            };
            
            context.InspectionRecord.Add(inspectionRecord);
            
            // 将projects表的ProjInforId字段当前值减去1
            if (project.ProjInforId.HasValue && project.ProjInforId.Value > 1)
            {
                project.ProjectPhaseStatusId = 102;
                project.ProjInforId = project.ProjInforId.Value - 1;
                MessageBox.Show("项目已驳回！项目流程状态已回退到上一阶段。");
            }
            else if (project.ProjInforId.HasValue && project.ProjInforId.Value == 1)
            {
                MessageBox.Show("项目已驳回！当前项目流程状态已为初始阶段（1），无法继续回退。");
            }
            else
            {
                MessageBox.Show("项目已驳回！");
            }
            
            await context.SaveChangesAsync();
        }

        private async Task<int> GetCurrentUserSequenceAsync(ProjectContext context, int? equipmentTypeId, int userId)
        {
            var approver = await context.TypeApprFlowPersSeqTable
                .Where(t => t.equipmenttypeId == equipmentTypeId 
                         && t.ReviewerPeopleId == userId 
                         && t.Mark != "Dele")
                .FirstOrDefaultAsync();
            
            return approver?.Sequence ?? 0;
        }

        /// <summary>
        /// 检查当前登录人员是否为最后一位审批人
        /// </summary>
        private async Task<bool> IsLastApproverAsync(ProjectContext context, int? equipmentTypeId, int userId)
        {
            // 获取当前用户的审批顺序
            var currentUserSequence = await GetCurrentUserSequenceAsync(context, equipmentTypeId, userId);
            
            if (currentUserSequence == 0)
            {
                return false; // 当前用户不是审批人
            }
            
            // 获取该设备类型的最大审批顺序
            var maxSequence = await context.TypeApprFlowPersSeqTable
                .Where(t => t.equipmenttypeId == equipmentTypeId && t.Mark != "Dele")
                .MaxAsync(t => (int?)t.Sequence);
            
            return maxSequence.HasValue && currentUserSequence == maxSequence.Value;
        }

        private async Task UpdateProjectProjInforIdAsync(ProjectContext context)
        {
            var projectId = Convert.ToInt32(CurrentProjectId);
            
            // 获取当前项目信息
            var project = await context.Projects
                .FirstOrDefaultAsync(p => p.ProjectsId == projectId);
            
            if (project == null)
            {
                return;
            }
            
            // 更新最后一次提交审核时间
            project.LastSubmitTime = DateTime.Now;
            
            // 更新ProjInforId字段（当前值+1）
            if (project.ProjInforId.HasValue)
            {
                project.ProjInforId = project.ProjInforId.Value + 1;
            }
            else
            {
                project.ProjInforId = null; // 如果当前值为null，则设置为1
            }
            project.ProjectPhaseStatusId = 105;
            await context.SaveChangesAsync();
        }

    }
}
