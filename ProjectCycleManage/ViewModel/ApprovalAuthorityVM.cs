using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace ProjectCycleManage.ViewModel
{
    public partial class ApprovalAuthorityVM : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ProjectTypeVM> _projectTypes = new ObservableCollection<ProjectTypeVM>();

        [ObservableProperty]
        private ProjectTypeVM _selectedProjectType;

        public ApprovalAuthorityVM()
        {
            InitializeData();
        }

        private void InitializeData()
        {
            // 初始化项目类型数据

            using var context = new ProjectContext();
            var projects = context.EquipmentType.Select(p => p.EquipmentName).ToList();
            var projectstage = context.ProjectStage.ToList();

            ProjectTypes.Clear();
            if (projects == null)
            {
                return;
            }
            
            //var projectTypeA = new ProjectTypeVM("项目类型A");
            for ( var i = 0; i < projects.Count; i++)
            {

                var projectType = new ProjectTypeVM(projects[i]);
                var typeapprflowpersseq = context.TypeApprFlowPersSeqTable
                    .Include(p => p.equipmenttype)
                    .Include(p => p.projectflow)
                    .Include(p => p.Reviewer)
                    .Where(p => p.equipmenttype.EquipmentName == projects[i] && p.Mark != "Dele")
                    .ToList();

                var typeapprflowpersseqGroup = typeapprflowpersseq
                    .GroupBy(p => p.projectflow.ProjFlowInfor)
                    .Select( p => new
                    {
                        GroupName = p.Key,
                        SortedItems = p.OrderBy(p => p.Sequence),
                        itemcount = p.Count()
                    })
                    .ToList();

                foreach (var flowgroup in typeapprflowpersseqGroup)
                {
                    ObservableCollection<ApproverVM> data = new ();

                    foreach (var item in flowgroup.SortedItems)
                    {

                        data.Add(new ApproverVM(item.Sequence.GetValueOrDefault(), item.Reviewer.PeopleName, item.Reviewer.PeopleName));
                    }

                    projectType.ApprovalStages.Add(new ApprovalStageVM(flowgroup.GroupName, flowgroup.itemcount)
                    {
                        Approvers = data
                    });
                }
                ProjectTypes.Add(projectType);
                
            }
            
            SelectedProjectType = ProjectTypes[0];
        }

        [RelayCommand]
        private void SelectProjectType(ProjectTypeVM projectType)
        {
            // 清除所有项目类型的选中状态
            foreach (var type in ProjectTypes)
            {
                type.IsSelected = false;
            }
            
            // 设置选中的项目类型
            projectType.IsSelected = true;
            SelectedProjectType = projectType;
        }

        [RelayCommand]
        private void EditApprovers(ApprovalStageVM stage)
        {
            SelectedStage = stage;
            IsEditing = true;
        }

        [RelayCommand]
        private void CancelEdit()
        {
            IsEditing = false;
            SelectedStage = null;
            NewApproverName = string.Empty;
            NewApproverDepartment = string.Empty;
        }

        [RelayCommand]
        private void AddApprover()
        {
            if (string.IsNullOrWhiteSpace(NewApproverName) || SelectedStage == null)
                return;

            var newOrder = SelectedStage.Approvers.Count + 1;
            var newApprover = new ApproverVM(newOrder, NewApproverName.Substring(0, 1), NewApproverName);
            SelectedStage.Approvers.Add(newApprover);
            SelectedStage.ApproverCount = SelectedStage.Approvers.Count;
            
            NewApproverName = string.Empty;
            NewApproverDepartment = string.Empty;
        }

        [RelayCommand]
        private void RemoveApprover(ApproverVM approver)
        {
            if (SelectedStage == null || !SelectedStage.Approvers.Contains(approver))
                return;

            SelectedStage.Approvers.Remove(approver);
            
            // 重新排序
            for (int i = 0; i < SelectedStage.Approvers.Count; i++)
            {
                SelectedStage.Approvers[i].Order = i + 1;
            }
            
            SelectedStage.ApproverCount = SelectedStage.Approvers.Count;
        }

        [RelayCommand]
        private void SaveApprovers()
        {
            // 保存审批人设置逻辑
            MessageBox.Show($"已保存 {SelectedStage?.StageName} 的审批人设置");
            IsEditing = false;
            SelectedStage = null;
        }

        [RelayCommand]
        private void IncreaseOrder(ApproverVM approver)
        {
            if (SelectedStage == null || approver == null)
                return;

            var currentIndex = SelectedStage.Approvers.IndexOf(approver);
            if (currentIndex > 0) // 不是第一个
            {
                // 交换位置
                SelectedStage.Approvers.Move(currentIndex, currentIndex - 1);
                
                // 更新序号
                UpdateApproverOrders();
            }
        }

        [RelayCommand]
        private void DecreaseOrder(ApproverVM approver)
        {
            if (SelectedStage == null || approver == null)
                return;

            var currentIndex = SelectedStage.Approvers.IndexOf(approver);
            if (currentIndex < SelectedStage.Approvers.Count - 1) // 不是最后一个
            {
                // 交换位置
                SelectedStage.Approvers.Move(currentIndex, currentIndex + 1);
                
                // 更新序号
                UpdateApproverOrders();
            }
        }

        [RelayCommand]
        private void SearchUser()
        {
            // 模拟搜索用户逻辑
            // 搜索功能与用户建议列表共存，搜索时显示搜索结果，否则显示默认建议列表
            if (string.IsNullOrWhiteSpace(SearchKeyword))
            {
                // 搜索关键词为空时，显示默认用户建议列表
                ShowDefaultSuggestions = true;
            }
            else
            {
                // 有搜索关键词时，显示搜索结果
                ShowDefaultSuggestions = false;
            }
        }

        private void UpdateApproverOrders()
        {
            for (int i = 0; i < SelectedStage.Approvers.Count; i++)
            {
                SelectedStage.Approvers[i].Order = i + 1;
            }
        }

        [ObservableProperty]
        private bool _isEditing;

        [ObservableProperty]
        private ApprovalStageVM _selectedStage;

        [ObservableProperty]
        private string _newApproverName = string.Empty;

        [ObservableProperty]
        private string _newApproverDepartment = string.Empty;

        [ObservableProperty]
        private string _searchKeyword = string.Empty;

        [ObservableProperty]
        private bool _hasSearchResults = false;

        [ObservableProperty]
        private bool _showDefaultSuggestions = true;

        [ObservableProperty]
        private bool _hasApprovers = false;

        partial void OnSelectedStageChanged(ApprovalStageVM? value)
        {
            HasApprovers = value?.Approvers?.Count > 0;
        }
    }

    public partial class ProjectTypeVM : ObservableObject
    {
        [ObservableProperty]
        private string _typeName;

        [ObservableProperty]
        private ObservableCollection<ApprovalStageVM> _approvalStages = new ObservableCollection<ApprovalStageVM>();

        [ObservableProperty]
        private bool _isSelected;

        public ProjectTypeVM(string typeName)
        {
            TypeName = typeName;
        }
    }

    public partial class ApprovalStageVM : ObservableObject
    {
        [ObservableProperty]
        private string _stageName;

        [ObservableProperty]
        private int _approverCount;

        [ObservableProperty]
        private ObservableCollection<ApproverVM> _approvers = new ObservableCollection<ApproverVM>();

        [ObservableProperty]
        private bool _hasApprovers = false;

        public ApprovalStageVM(string stageName, int approverCount = 0)
        {
            StageName = stageName;
            ApproverCount = approverCount;
            HasApprovers = approverCount > 0;
        }

        partial void OnApproversChanged(ObservableCollection<ApproverVM>? value)
        {
            HasApprovers = value?.Count > 0;
        }

        partial void OnApproverCountChanged(int value)
        {
            HasApprovers = value > 0;
        }
    }

    public partial class ApproverVM : ObservableObject
    {
        [ObservableProperty]
        private int _order;

        [ObservableProperty]
        private string _initial;

        [ObservableProperty]
        private string _name;


        public ApproverVM(int order, string initial, string name)
        {
            Order = order;
            Initial = initial;
            Name = name;
        }
    }
}