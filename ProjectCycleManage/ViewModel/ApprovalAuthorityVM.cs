using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using ProjectCycleManage.Model;
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
            
            // 设置默认选中的项目类型
            if (ProjectTypes.Count > 0)
            {
                SelectedProjectType = ProjectTypes[0];
                SelectedProjectType.IsSelected = true;
            }
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
            if (SelectedProjectType == null || SelectedStage == null)
            {
                MessageBox.Show("请选择项目类型和阶段");
                return;
            }

            try
            {
                // 获取当前编辑的审批人列表
                var currentApprovers = SelectedStage.Approvers.ToList();
                
                // 验证审批人列表数据
                if (!ValidateApproversList(currentApprovers))
                {
                    return;
                }
                
                // 同步数据到数据库
                SyncApproversToDatabase(currentApprovers);
                
                // 重新加载数据以刷新页面显示
                InitializeData();
                
                MessageBox.Show($"已保存 {SelectedStage?.StageName} 的审批人设置");
                IsEditing = false;
                SelectedStage = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证审批人列表数据
        /// </summary>
        /// <param name="approvers">审批人列表</param>
        /// <returns>验证是否通过</returns>
        private bool ValidateApproversList(List<ApproverVM> approvers)
        {
            // 检查列表是否为空
            if (approvers.Count == 0)
            {
                MessageBox.Show("审批人列表不能为空，请至少添加一个审批人");
                return false;
            }
            
            // 检查顺序是否连续（没有跳过）
            var expectedOrder = 1;
            foreach (var approver in approvers.OrderBy(a => a.Order))
            {
                if (approver.Order != expectedOrder)
                {
                    MessageBox.Show($"审批人顺序不连续，请检查第{expectedOrder}个位置");
                    return false;
                }
                expectedOrder++;
            }
            
            // 检查是否有重复的审批人
            var duplicateApprovers = approvers
                .GroupBy(a => a.Name)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
                
            if (duplicateApprovers.Any())
            {
                MessageBox.Show($"存在重复的审批人: {string.Join(", ", duplicateApprovers)}");
                return false;
            }
            
            // 检查是否有空值
            var emptyApprovers = approvers.Where(a => string.IsNullOrWhiteSpace(a.Name)).ToList();
            if (emptyApprovers.Any())
            {
                MessageBox.Show("存在姓名为空的审批人，请检查并修正");
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// 获取现有审批流程人员记录（包括已标记删除的记录）
        /// </summary>
        /// <param name="equipmentTypeName">设备类型名称</param>
        /// <param name="stageName">阶段名称</param>
        /// <param name="includeDeleted">是否包含已标记删除的记录</param>
        /// <returns>现有的审批人记录列表</returns>
        private List<TypeApprFlowPersSeqTable> GetExistingApprovers(string equipmentTypeName, string stageName, bool includeDeleted = false)
        {
            using var context = new ProjectContext();
            
            var query = context.TypeApprFlowPersSeqTable
                .Include(t => t.equipmenttype)
                .Include(t => t.projectflow)
                .Include(t => t.Reviewer)
                .Where(t => t.equipmenttype.EquipmentName == equipmentTypeName &&
                           t.projectflow.ProjFlowInfor == stageName);
            
            // 如果不包含已删除记录，则过滤掉标记为删除的记录
            if (!includeDeleted)
            {
                query = query.Where(t => t.Mark == null || t.Mark != "Dele");
            }
            
            return query.OrderBy(t => t.Sequence).ToList();
        }

        /// <summary>
        /// 根据人员姓名查找人员ID
        /// </summary>
        /// <param name="personName">人员姓名</param>
        /// <returns>人员ID，如果找不到返回null</returns>
        private int? FindPersonIdByName(string personName)
        {
            using var context = new ProjectContext();
            var person = context.PeopleTable
                .FirstOrDefault(p => p.PeopleName == personName && p.IsEmployed == "True");
            
            return person?.PeopleId;
        }

        /// <summary>
        /// 同步审批人数据到数据库
        /// </summary>
        /// <param name="currentApprovers">当前编辑的审批人列表</param>
        private void SyncApproversToDatabase(List<ApproverVM> currentApprovers)
        {
            using var context = new ProjectContext();
            
            // 获取现有数据库记录（包含已标记删除的记录，这样才能正确进行软删除操作）
            var existingRecords = GetExistingApprovers(SelectedProjectType.TypeName, SelectedStage.StageName, true);
            
            // 处理新增和修改的记录
            for (int i = 0; i < currentApprovers.Count; i++)
            {
                var currentApprover = currentApprovers[i];
                var personId = FindPersonIdByName(currentApprover.Name);
                
                if (personId == null)
                {
                    MessageBox.Show($"找不到人员: {currentApprover.Name}，请确保该人员存在且在职");
                    continue;
                }
                
                // 查找对应的现有记录（包含已标记删除的记录）
                var existingRecord = existingRecords.FirstOrDefault(r => 
                    r.ReviewerPeopleId == personId);
                
                if (existingRecord != null)
                {
                    // 更新现有记录的序号和标记
                    existingRecord.Sequence = i + 1;
                    existingRecord.Mark = null; // 确保标记为空
                    
                    // 标记为已修改，确保Entity Framework跟踪更改
                    context.Entry(existingRecord).State = EntityState.Modified;
                }
                else
                {
                    // 创建新记录
                    var newRecord = new TypeApprFlowPersSeqTable
                    {
                        equipmenttypeId = GetEquipmentTypeIdByName(SelectedProjectType.TypeName),
                        projectflowId = GetProjectFlowIdByName(SelectedStage.StageName),
                        ReviewerPeopleId = personId.Value,
                        Sequence = i + 1,
                        Mark = null
                    };
                    
                    context.TypeApprFlowPersSeqTable.Add(newRecord);
                }
            }
            
            // 处理需要标记为删除的记录（在数据库中存在但不在当前列表中）
            foreach (var existingRecord in existingRecords)
            {
                var isInCurrentList = currentApprovers.Any(a => 
                    FindPersonIdByName(a.Name) == existingRecord.ReviewerPeopleId);
                
                if (!isInCurrentList)
                {
                    existingRecord.Mark = "Dele";
                    // 显式标记为已修改，确保Entity Framework跟踪更改
                    context.Entry(existingRecord).State = EntityState.Modified;
                }
            }
            
            context.SaveChanges();
        }

        /// <summary>
        /// 根据设备类型名称获取设备类型ID
        /// </summary>
        /// <param name="equipmentTypeName">设备类型名称</param>
        /// <returns>设备类型ID</returns>
        private int? GetEquipmentTypeIdByName(string equipmentTypeName)
        {
            using var context = new ProjectContext();
            var equipmentType = context.EquipmentType
                .FirstOrDefault(e => e.EquipmentName == equipmentTypeName);
            
            return equipmentType?.EquipmentTypeId;
        }

        /// <summary>
        /// 根据流程名称获取流程ID
        /// </summary>
        /// <param name="flowName">流程名称</param>
        /// <returns>流程ID</returns>
        private int? GetProjectFlowIdByName(string flowName)
        {
            using var context = new ProjectContext();
            var projectFlow = context.ProjFlowTable
                .FirstOrDefault(p => p.ProjFlowInfor == flowName);
            
            return projectFlow?.Id;
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