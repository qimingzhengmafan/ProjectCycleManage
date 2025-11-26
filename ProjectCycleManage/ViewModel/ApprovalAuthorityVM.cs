using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using ProjectCycleManage.Utilities;
using ProjectManagement.Data;
using ProjectManagement.Models;
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

                        data.Add(new ApproverVM(item.Sequence.GetValueOrDefault(), StringHelper.GetFirstCharOrN(item.Reviewer.PeopleName), item.Reviewer.PeopleName));
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
        private void AddApprover(object parameter)
        {
            if (SelectedStage == null)
                return;

            string approverName = string.Empty;
            
            // 处理不同的参数类型
            if (parameter is PeopleTable person)
            {
                // 从默认建议列表添加
                approverName = person.PeopleName;
            }
            else if (parameter is string name && !string.IsNullOrWhiteSpace(name))
            {
                // 从手动输入添加
                approverName = name;
            }
            else if (!string.IsNullOrWhiteSpace(NewApproverName))
            {
                // 从NewApproverName属性添加（向后兼容）
                approverName = NewApproverName;
            }
            else
            {
                return;
            }

            var newOrder = SelectedStage.Approvers.Count + 1;
            var newApprover = new ApproverVM(newOrder, approverName.Substring(0, 1), approverName);
            SelectedStage.Approvers.Add(newApprover);
            SelectedStage.ApproverCount = SelectedStage.Approvers.Count;
            
            // 清空输入框
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
            // 搜索用户逻辑
            if (string.IsNullOrWhiteSpace(SearchKeyword))
            {
                // 搜索关键词为空时，显示默认用户建议列表
                ShowDefaultSuggestions = true;
                HasSearchResults = false;
                SearchResults.Clear();
            }
            else
            {
                // 有搜索关键词时，从数据库搜索在职人员
                ShowDefaultSuggestions = false;
                SearchResults.Clear();
                
                try
                {
                    using var context = new ProjectContext();
                    var searchResults = context.PeopleTable
                        .Where(p => p.IsEmployed == "True" && 
                                   (p.PeopleName.Contains(SearchKeyword) || 
                                    p.PeopleId.ToString().Contains(SearchKeyword)))
                        .ToList();
                    
                    foreach (var person in searchResults)
                    {
                        SearchResults.Add(person);
                    }
                    
                    HasSearchResults = SearchResults.Count > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"搜索人员失败: {ex.Message}");
                    HasSearchResults = false;
                }
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

        [ObservableProperty]
        private ObservableCollection<PeopleTable> _defaultSuggestions = new ObservableCollection<PeopleTable>();

        [ObservableProperty]
        private ObservableCollection<PeopleTable> _searchResults = new ObservableCollection<PeopleTable>();

        partial void OnSelectedStageChanged(ApprovalStageVM? value)
        {
            HasApprovers = value?.Approvers?.Count > 0;
        }

        partial void OnIsEditingChanged(bool value)
        {
            if (value)
            {
                // 当开始编辑时，从数据库加载在职人员
                LoadDefaultSuggestions();
            }
        }

        private void LoadDefaultSuggestions()
        {
            try
            {
                using var context = new ProjectContext();
                var employedPeople = context.PeopleTable
                    .Where(p => p.IsEmployed == "True")
                    .ToList();

                DefaultSuggestions.Clear();
                foreach (var person in employedPeople)
                {
                    DefaultSuggestions.Add(person);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载人员数据失败: {ex.Message}");
            }
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