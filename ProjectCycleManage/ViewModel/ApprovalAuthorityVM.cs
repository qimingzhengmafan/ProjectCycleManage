using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
            ProjectTypes.Clear();
            
            var projectTypeA = new ProjectTypeVM("项目类型A");
            projectTypeA.ApprovalStages.Add(new ApprovalStageVM("立项阶段", 3)
            {
                Approvers = new ObservableCollection<ApproverVM>
                {
                    new ApproverVM(1, "王", "王总监", "战略规划部"),
                    new ApproverVM(2, "李", "李经理", "项目管理部"),
                    new ApproverVM(3, "赵", "赵主管", "财务部")
                }
            });
            
            projectTypeA.ApprovalStages.Add(new ApprovalStageVM("规划阶段", 2)
            {
                Approvers = new ObservableCollection<ApproverVM>
                {
                    new ApproverVM(1, "周", "周总监", "技术研发部"),
                    new ApproverVM(2, "吴", "吴经理", "项目管理部")
                }
            });
            
            projectTypeA.ApprovalStages.Add(new ApprovalStageVM("执行阶段", 1)
            {
                Approvers = new ObservableCollection<ApproverVM>
                {
                    new ApproverVM(1, "郑", "郑主管", "质量管理部")
                }
            });
            
            projectTypeA.ApprovalStages.Add(new ApprovalStageVM("监控阶段", 0));
            projectTypeA.ApprovalStages.Add(new ApprovalStageVM("收尾阶段", 0));
            
            ProjectTypes.Add(projectTypeA);
            
            var projectTypeB = new ProjectTypeVM("项目类型B");
            projectTypeB.ApprovalStages.Add(new ApprovalStageVM("立项阶段", 0));
            projectTypeB.ApprovalStages.Add(new ApprovalStageVM("规划阶段", 0));
            projectTypeB.ApprovalStages.Add(new ApprovalStageVM("执行阶段", 0));
            projectTypeB.ApprovalStages.Add(new ApprovalStageVM("监控阶段", 0));
            projectTypeB.ApprovalStages.Add(new ApprovalStageVM("收尾阶段", 0));
            
            ProjectTypes.Add(projectTypeB);
            
            var projectTypeC = new ProjectTypeVM("项目类型C");
            projectTypeC.ApprovalStages.Add(new ApprovalStageVM("立项阶段", 0));
            projectTypeC.ApprovalStages.Add(new ApprovalStageVM("规划阶段", 0));
            projectTypeC.ApprovalStages.Add(new ApprovalStageVM("执行阶段", 0));
            projectTypeC.ApprovalStages.Add(new ApprovalStageVM("监控阶段", 0));
            projectTypeC.ApprovalStages.Add(new ApprovalStageVM("收尾阶段", 0));
            
            ProjectTypes.Add(projectTypeC);
            
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
            var newApprover = new ApproverVM(newOrder, NewApproverName.Substring(0, 1), NewApproverName, NewApproverDepartment);
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

        [ObservableProperty]
        private bool _isEditing;

        [ObservableProperty]
        private ApprovalStageVM _selectedStage;

        [ObservableProperty]
        private string _newApproverName = string.Empty;

        [ObservableProperty]
        private string _newApproverDepartment = string.Empty;
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

        public ApprovalStageVM(string stageName, int approverCount = 0)
        {
            StageName = stageName;
            ApproverCount = approverCount;
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

        [ObservableProperty]
        private string _department;

        public ApproverVM(int order, string initial, string name, string department)
        {
            Order = order;
            Initial = initial;
            Name = name;
            Department = department;
        }
    }
}