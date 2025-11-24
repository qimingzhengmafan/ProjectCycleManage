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
            SelectedProjectType = projectType;
        }

        [RelayCommand]
        private void EditApprovers(ApprovalStageVM stage)
        {
            // 编辑审批人逻辑
            MessageBox.Show($"编辑 {stage.StageName} 的审批人设置");
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