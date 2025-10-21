using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectManagement.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ProjectDetailsVM : ObservableObject
    {
        
        #region Binding字段
        /// <summary>
        /// 项目编号
        /// </summary>
        [ObservableProperty]
        private int _projectsid;
        
        /// <summary>
        /// 年份
        /// </summary>
        [ObservableProperty]
        private int? _year;
        
        /// <summary>
        /// 采购月份
        /// </summary>
        [ObservableProperty]
        private DateTime? _procurementMonth;
        
        /// <summary>
        /// 项目名称
        /// </summary>
        [ObservableProperty]
        private string _projectName;
        
        /// <summary>
        /// 设备名称
        /// </summary>
        [ObservableProperty]
        private string _equipmentname;
        
        /// <summary>
        /// 项目编号
        /// </summary>
        [ObservableProperty]
        private string _projectIdentifyingNumber;


        /// <summary>
        /// 设备类型ID
        /// </summary>
        [ObservableProperty]
        private int? _equipmenttypeId;
        
        /// <summary>
        /// 项目类型ID
        /// </summary>
        [ObservableProperty]
        private int _typeId;
        
        /// <summary>
        /// 项目状态ID
        /// </summary>
        [ObservableProperty]
        private int _projectStageId;
        
        /// <summary>
        /// 完成时间
        /// </summary>
        [ObservableProperty]
        private DateTime? _finishTime;

        /// <summary>
        /// 开始时间
        /// </summary>
        [ObservableProperty]
        private DateTime? _startTime;

        [ObservableProperty]
        private int? _projectCycle;

        /// <summary>
        /// 项目预算
        /// </summary>
        [ObservableProperty]
        private string? _budget;
        
        /// <summary>
        /// 项目实际花费
        /// </summary>
        [ObservableProperty]
        private string? _actualExpenditure;

        /// <summary>
        /// 项目阶段状态
        /// </summary>
        [ObservableProperty] 
        private int _projectPhaseStatusId;
        
        /// <summary>
        /// 项目Leader
        /// </summary>
        [ObservableProperty] 
        private int? _projectsLeaderID;
        
        /// <summary>
        /// 项目跟进人
        /// </summary>
        [ObservableProperty] 
        private int? _projectsfollowuppersonId;
        
        /// <summary>
        /// 资产编号
        /// </summary>
        [ObservableProperty] 
        private string? _assetnumber;
        
        /// <summary>
        /// 备注
        /// </summary>
        [ObservableProperty] 
        private string? _remarkks;

        /// <summary>
        /// 已完成
        /// </summary>
        [ObservableProperty]
        private string? _confirm;

        #endregion



        #region 文件记录

        //OA采购申请单号OA Purchase Application Number 
        [ObservableProperty]
        private string? oapurchaseapplicationnumber;

        //"OA领用申请单号"，OA Requisition application Number
        [ObservableProperty]
        private string? oarequisitionapplicationnumber;

        //"设备申请表"Equipment application Form
        [ObservableProperty]
        private bool _equipmentapplicationform;

        //技术协议 technical agreement
        [ObservableProperty]
        private bool _technicalagreement;

        //设备方案/BOM清单，equipment solution OR BOM list
        [ObservableProperty]
        private bool _equipmentsolutionorbomlist;

        //"设备项目问题改善"，Equipment Project Problem Improvement
        /// <summary>
        /// 
        /// </summary>
        [ObservableProperty]
        private bool _equipmentprojectproblemimprovement;

        //"设备验证记录"	，Equipment Verification Record
        [ObservableProperty]
        private bool _equipmentverificationrecord;

        //培训记录，training record
        [ObservableProperty]
        private bool _trainingrecord;

        //说明书，manual
        [ObservableProperty]
        private bool _manual;

        //维保文件，maintenance document
        [ObservableProperty]
        private bool _maintenancedocument;

        //WI，WI
        [ObservableProperty]
        private bool _wi;

        //"设备验收单"，Equipment Acceptance Form
        [ObservableProperty]
        private bool _equipmentacceptanceform;

        //“文件发放记录表Document Distribution Record Form
        [ObservableProperty]
        private bool _documentdistributionrecordform;




        #endregion


        public ProjectDetailsVM()
        {
            LoadEmployees();
            LoadType();
            LoadEquipmentType();
            Loadprojectstage();
            Loadprojectphasestatus();
        }

        #region 责任人下拉框

        private ObservableCollection<PeopleTable> _employees;
        private PeopleTable _selectedEmployee;
        private string _statusMessage;

        // 员工列表 - 用于下拉框
        public ObservableCollection<PeopleTable> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged();
            }
        }

        // 选中的员工
        public PeopleTable SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged();
                UpdateStatusMessage();
            }
        }

        // 状态信息
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        // 加载员工数据
        private void LoadEmployees()
        {
            try
            {
                using (var context = new ProjectContext())
                {
                    var employees = context.PeopleTable
                        .OrderBy(e => e.PeopleName)
                        .ToList();

                    Employees = new ObservableCollection<PeopleTable>(employees);

                    if (Employees.Count > 0)
                    {
                        //SelectedFollowEmployee = Employees[0];
                        SelectedEmployee = Employees[0];
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        // 更新状态信息
        private void UpdateStatusMessage()
        {
            if (SelectedEmployee != null)
            {
                //StatusMessage = $"选中: {SelectedEmployee.PeopleName} ({SelectedEmployee.PeopleId})";
                ProjectsLeaderID = SelectedEmployee.PeopleId;
            }
        }


        #endregion
        #region 跟进人下拉框

        private PeopleTable _selectedFollowEmployee;


        // 选中
        public PeopleTable SelectedFollowEmployee
        {
            get => _selectedFollowEmployee;
            set
            {
                _selectedFollowEmployee = value;
                OnPropertyChanged();
                UpdateFollowStatusMessage();
            }
        }

        // 更新状态信息
        private void UpdateFollowStatusMessage()
        {
            if (SelectedFollowEmployee != null)
            {
                ProjectsfollowuppersonId = SelectedFollowEmployee.PeopleId;
            }
            
        }


        #endregion
        #region 设备类型下拉框

        private ObservableCollection<EquipmentType> _equipmentTypes;
        private EquipmentType _selectedEquipmentType;

        // 列表 - 用于下拉框
        public ObservableCollection<EquipmentType> EquipmentTypes
        {
            get => _equipmentTypes;
            set
            {
                _equipmentTypes = value;
                OnPropertyChanged();
            }
        }

        // 选中
        public EquipmentType SelectedEquipmentType
        {
            get => _selectedEquipmentType;
            set
            {
                _selectedEquipmentType = value;
                OnPropertyChanged();
                UpdateEquipmentTypeStatus();
            }
        }

        // 加载数据
        private void LoadEquipmentType()
        {
            try
            {
                using (var context = new ProjectContext())
                {
                    var equipmenttypes = context.EquipmentType
                        .OrderBy(e => e.EquipmentName)
                        .ToList();

                    EquipmentTypes = new ObservableCollection<EquipmentType>(equipmenttypes);

                    if (EquipmentTypes.Count > 0)
                        SelectedEquipmentType = EquipmentTypes[0];

                }
            }
            catch (Exception ex)
            {
                //StatusMessage = $"加载失败: {ex.Message}";
            }
        }

        // 更新状态信息
        private void UpdateEquipmentTypeStatus()
        {

            if (SelectedEquipmentType != null)
            {
                EquipmenttypeId = SelectedEquipmentType.EquipmentTypeId;
            }
                
        }


        #endregion
        #region 项目类型下拉框

        private ObservableCollection<TypeTable> _types;
        private TypeTable _selectedType;

        // 员工列表 - 用于下拉框
        public ObservableCollection<TypeTable> Types
        {
            get => _types;
            set
            {
                _types = value;
                OnPropertyChanged();
            }
        }

        // 选中
        public TypeTable SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                OnPropertyChanged();
                UpdateTypeStatus();
            }
        }

        // 加载数据
        private void LoadType()
        {
            try
            {
                using (var context = new ProjectContext())
                {
                    var types = context.TypeTable
                        .OrderBy(e => e.TypeName)
                        .ToList();

                    Types = new ObservableCollection<TypeTable>(types);

                    if (Types.Count > 0)
                        SelectedType = Types[0];

                }
            }
            catch (Exception ex)
            {

            }
        }

        // 更新状态信息
        private void UpdateTypeStatus()
        {
            if (SelectedType != null)
            {
                TypeId = SelectedType.TypeId;
            }
            
        }


        #endregion
        #region 阶段下拉框

        private ObservableCollection<ProjectStage> _projectstage;
        private ProjectStage _selectedprojectstage;

        // 列表 - 用于下拉框
        public ObservableCollection<ProjectStage> Projectstage
        {
            get => _projectstage;
            set
            {
                _projectstage = value;
                OnPropertyChanged();
            }
        }

        // 选中
        public ProjectStage Selectedprojectstage
        {
            get => _selectedprojectstage;
            set
            {
                _selectedprojectstage = value;
                OnPropertyChanged();
                UpdateProjectStage();
            }
        }

        // 加载数据
        private void Loadprojectstage()
        {
            try
            {
                using (var context = new ProjectContext())
                {
                    var projectstage = context.ProjectStage
                        .OrderBy(e => e.ProjectStageName)
                        .ToList();

                    Projectstage = new ObservableCollection<ProjectStage>(projectstage);

                    if (Projectstage.Count > 0)
                        Selectedprojectstage = Projectstage[0];

                }
            }
            catch (Exception ex)
            {

            }
        }

        // 更新状态信息
        private void UpdateProjectStage()
        {
            if (Selectedprojectstage != null)
            {
                ProjectStageId = Selectedprojectstage.ProjectStageId;
            }
            
        }


        #endregion
        #region 项目阶段状态下拉框

        private ObservableCollection<ProjectPhaseStatus> _projectphasestatus;
        private ProjectPhaseStatus _selectedprojectphasestatus;

        // 列表 - 用于下拉框
        public ObservableCollection<ProjectPhaseStatus> Projectphasestatus
        {
            get => _projectphasestatus;
            set
            {
                _projectphasestatus = value;
                OnPropertyChanged();
            }
        }

        // 选中
        public ProjectPhaseStatus Selectedprojectphasestatus
        {
            get => _selectedprojectphasestatus;
            set
            {
                _selectedprojectphasestatus = value;
                OnPropertyChanged();
                UpdateProjectPhaseStatus();
            }
        }

        // 加载数据
        private void Loadprojectphasestatus()
        {
            try
            {
                using (var context = new ProjectContext())
                {
                    var projectphasestatus = context.ProjectPhaseStatus
                        .OrderBy(e => e.ProjectPhaseStatusName)
                        .ToList();

                    Projectphasestatus = new ObservableCollection<ProjectPhaseStatus>(projectphasestatus);

                    if (Projectphasestatus.Count > 0)
                        Selectedprojectphasestatus = Projectphasestatus[0];

                }
            }
            catch (Exception ex)
            {

            }
        }

        // 更新状态信息
        private void UpdateProjectPhaseStatus()
        {
            if (Selectedprojectstage != null)
            {
                ProjectPhaseStatusId = Selectedprojectphasestatus.ProjectPhaseStatusId;
            }
            
        }


        #endregion


        #region OtherFun

        [RelayCommand]
        private void SaveChanges()
        {
            UpdateUserEmailAsync();
            SaveDocumentStatusAsync(); // 新增：保存文档状态
        }

        [RelayCommand]
        private void SaveFileChanges()
        {
            
        }

        [RelayCommand]
        private void DeleteProject()
        {
            
        }

        [RelayCommand]
        private void ConformFun()
        {
            Confirm = "PASS";
        }
        
        
        public async Task UpdateUserEmailAsync()
        {
            using var context = new ProjectContext();
    
            // 第一步：查询数据库获取完整实体
            var project = await context.Projects.FindAsync(Projectsid);
            if (project != null)
            {
                project.ProjectsId = Projectsid;
                project.ProjectName = ProjectName;
                project.EquipmentName = Equipmentname;
                project.ProjectIdentifyingNumber = ProjectIdentifyingNumber;
                project.equipmenttypeId = EquipmenttypeId;
                project.typeId = TypeId;
                project.ProjectStageId = ProjectStageId;
                project.FinishTime = FinishTime;
                project.StartTime = StartTime;
                project.Budget = Budget;
                project.ActualExpenditure = ActualExpenditure;
                project.ProjectPhaseStatusId = ProjectPhaseStatusId;
                project.ProjectLeaderId = ProjectsLeaderID;
                project.projectfollowuppersonId = ProjectsfollowuppersonId;
                project.AssetNumber = Assetnumber;
                project.remarks = Remarkks;
                project.CheckData = Confirm;
                //await UpdateUserEmailAsync();
                
                await context.SaveChangesAsync(); // 更新所有修改的字段
            }
        }

        #endregion
        // 在 ProjectDetailsVM.cs 中添加

        #region 文档状态快速实现

        // 加载文档状态（在项目加载时调用）
        public async Task LoadDocumentStatusAsync()
        {
            try
            {
                using (var context = new ProjectContext())
                {
                    // 1. 获取当前项目的设备类型
                    var project = await context.Projects
                        .Include(p => p.equipmenttype)
                        .FirstOrDefaultAsync(p => p.ProjectsId == Projectsid);

                    if (project?.equipmenttypeId == null) return;

                    // 2. 获取该设备类型关联的文档类型
                    var associatedDocs = await context.ProjectTypeDocumentAssociationTables
                        .Where(x => x.EquipmentTypeId == project.equipmenttypeId)
                        .Select(x => x.DocumentTypeId)
                        .ToListAsync();

                    // 3. 获取项目文档状态
                    var documentStatuses = await context.ProjectDocumentStatus
                        .Where(x => x.ProjectsId == Projectsid && associatedDocs.Contains(x.DocumentTypeId))
                        .ToListAsync();

                    // 4. 更新复选框状态
                    foreach (var status in documentStatuses)
                    {
                        switch (status.DocumentTypeId)
                        {
                            case 101: // OA申请单号
                                Oapurchaseapplicationnumber = status.Remarks;
                                break;
                            case 102: // 设备申请表
                                Equipmentapplicationform = status.IsHasDocument ?? false;
                                break;
                            case 103: // 技术协议
                                Technicalagreement = status.IsHasDocument ?? false;
                                break;
                            case 104: // 设备方案/BOM清单
                                Equipmentsolutionorbomlist = status.IsHasDocument ?? false;
                                break;
                            case 105: // 设备项目问题改善
                                Equipmentprojectproblemimprovement = status.IsHasDocument ?? false;
                                break;
                            case 106: // 设备验证记录
                                Equipmentverificationrecord = status.IsHasDocument ?? false;
                                break;
                            case 107: // 培训记录
                                Trainingrecord = status.IsHasDocument ?? false;
                                break;
                            case 108: // 说明书
                                Manual = status.IsHasDocument ?? false;
                                break;
                            case 109: // 维保文件
                                Maintenancedocument = status.IsHasDocument ?? false;
                                break;
                            case 110: // WI
                                Wi = status.IsHasDocument ?? false;
                                break;
                            case 111: // 设备验收单
                                Equipmentacceptanceform = status.IsHasDocument ?? false;
                                break;
                            case 112: // 文件发放记录表
                                Documentdistributionrecordform = status.IsHasDocument ?? false;
                                break;
                            case 113: // OA领用申请单号
                                Oarequisitionapplicationnumber = status.Remarks;
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 简单处理异常
                Console.WriteLine($"加载文档状态失败: {ex.Message}");
            }
        }

        // 保存文档状态（在保存项目时调用）
        public async Task SaveDocumentStatusAsync()
        {
            try
            {
                using (var context = new ProjectContext())
                {
                    // 获取当前项目的设备类型
                    var project = await context.Projects
                        .FirstOrDefaultAsync(p => p.ProjectsId == Projectsid);

                    if (project?.equipmenttypeId == null) return;

                    // 获取该设备类型关联的文档类型
                    var associatedDocs = await context.ProjectTypeDocumentAssociationTables
                        .Where(x => x.EquipmentTypeId == project.equipmenttypeId)
                        .Select(x => x.DocumentTypeId)
                        .ToListAsync();

                    // 获取或创建文档状态记录
                    foreach (var docTypeId in associatedDocs)
                    {
                        var status = await context.ProjectDocumentStatus
                            .FirstOrDefaultAsync(x => x.ProjectsId == Projectsid && x.DocumentTypeId == docTypeId);

                        if (status == null)
                        {
                            status = new ProjectDocumentStatus
                            {
                                ProjectsId = Projectsid,
                                DocumentTypeId = docTypeId
                            };
                            context.ProjectDocumentStatus.Add(status);
                        }

                        // 更新状态
                        bool hasDocument = false;
                        string remarks = null;

                        switch (docTypeId)
                        {
                            case 101: // OA申请单号
                                remarks = Oapurchaseapplicationnumber;
                                hasDocument = !string.IsNullOrEmpty(remarks);
                                break;
                            case 102: // 设备申请表
                                hasDocument = Equipmentapplicationform;
                                break;
                            case 103: // 技术协议
                                hasDocument = Technicalagreement;
                                break;
                            case 104: // 设备方案/BOM清单
                                hasDocument = Equipmentsolutionorbomlist;
                                break;
                            case 105: // 设备项目问题改善
                                hasDocument = Equipmentprojectproblemimprovement;
                                break;
                            case 106: // 设备验证记录
                                hasDocument = Equipmentverificationrecord;
                                break;
                            case 107: // 培训记录
                                hasDocument = Trainingrecord;
                                break;
                            case 108: // 说明书
                                hasDocument = Manual;
                                break;
                            case 109: // 维保文件
                                hasDocument = Maintenancedocument;
                                break;
                            case 110: // WI
                                hasDocument = Wi;
                                break;
                            case 111: // 设备验收单
                                hasDocument = Equipmentacceptanceform;
                                break;
                            case 112: // 文件发放记录表
                                hasDocument = Documentdistributionrecordform;
                                break;
                            case 113: // OA领用申请单号
                                remarks = Oarequisitionapplicationnumber;
                                hasDocument = !string.IsNullOrEmpty(remarks);
                                break;
                        }

                        status.IsHasDocument = hasDocument;
                        status.TheLastUpDateTime = hasDocument ? DateTime.Now : null;
                        status.UpdatePeopleId = ProjectsLeaderID;
                        status.Remarks = remarks;
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存文档状态失败: {ex.Message}");
            }
        }

        #endregion



        // 修改初始化方法
        public async Task InitializeAsync(int projectId)
        {
            Projectsid = projectId;
            await LoadProjectDataAsync();
            await LoadDocumentStatusAsync(); // 新增：加载文档状态
        }

        // 修改 LoadProjectDataAsync 方法
        private async Task LoadProjectDataAsync()
        {
            try
            {
                using (var context = new ProjectContext())
                {
                    var project = await context.Projects
                        .Include(p => p.equipmenttype)
                        .Include(p => p.type)
                        .Include(p => p.ProjectStage)
                        .Include(p => p.ProjectPhaseStatus)
                        .Include(p => p.ProjectLeader)
                        .Include(p => p.projectfollowupperson)
                        .FirstOrDefaultAsync(p => p.ProjectsId == Projectsid);

                    if (project != null)
                    {
                        // 设置基本属性
                        Year = project.Year;
                        ProjectName = project.ProjectName;
                        Equipmentname = project.EquipmentName;
                        ProjectIdentifyingNumber = project.ProjectIdentifyingNumber;
                        EquipmenttypeId = project.equipmenttypeId;
                        TypeId = project.typeId ?? 0;
                        ProjectStageId = project.ProjectStageId;
                        FinishTime = project.FinishTime;
                        StartTime = project.StartTime;
                        Budget = project.Budget;
                        ActualExpenditure = project.ActualExpenditure;
                        ProjectPhaseStatusId = project.ProjectPhaseStatusId ?? 0;
                        ProjectsLeaderID = project.ProjectLeaderId;
                        ProjectsfollowuppersonId = project.projectfollowuppersonId;
                        Assetnumber = project.AssetNumber;
                        Remarkks = project.remarks;

                        // 设置下拉框选中项
                        if (project.ProjectLeaderId.HasValue)
                        {
                            SelectedEmployee = Employees?.FirstOrDefault(e => e.PeopleId == project.ProjectLeaderId.Value);
                        }
                        if (project.projectfollowuppersonId.HasValue)
                        {
                            SelectedFollowEmployee = Employees?.FirstOrDefault(e => e.PeopleId == project.projectfollowuppersonId.Value);
                        }
                        if (project.equipmenttypeId.HasValue)
                        {
                            SelectedEquipmentType = EquipmentTypes?.FirstOrDefault(e => e.EquipmentTypeId == project.equipmenttypeId.Value);
                        }
                        if (project.typeId.HasValue)
                        {
                            SelectedType = Types?.FirstOrDefault(t => t.TypeId == project.typeId.Value);
                        }
                        if (project.ProjectStageId > 0)
                        {
                            Selectedprojectstage = Projectstage?.FirstOrDefault(s => s.ProjectStageId == project.ProjectStageId);
                        }
                        if (project.ProjectPhaseStatusId.HasValue)
                        {
                            Selectedprojectphasestatus = Projectphasestatus?.FirstOrDefault(s => s.ProjectPhaseStatusId == project.ProjectPhaseStatusId.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载项目数据失败: {ex.Message}");
            }
        }

    }
}
