using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectManagement.Data;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectCycleManage.ViewModel
{
    public partial class InformationCardVM : ObservableObject
    {
        #region 公共属性

        [ObservableProperty]
        private string _taginfor;

        [ObservableProperty]
        private string _infortype;

        [ObservableProperty]
        private string _inforprojectid;

        #endregion

        #region 文档
        [ObservableProperty]
        private bool? _fileisexist;

        [ObservableProperty]
        private string _filename;

        [RelayCommand]
        private void checkcommand()
        {

        }
        #endregion

        #region 文档-OA
        [ObservableProperty]
        private string _file_oa_indata;

        [ObservableProperty]
        private string _file_oa_writedata;

        [RelayCommand]
        private void File_OA_BtnFun()
        {

        }
        #endregion

        #region 信息-下拉框
        [ObservableProperty]
        private string _infor_people;

        [ObservableProperty]
        private ObservableCollection<PeopleTable> _infor_people_ob;

        [ObservableProperty]
        private PeopleTable infor_sele_people;

        [RelayCommand]
        private void Infor_People_BtnFun()
        {

        }
        #endregion

        #region 信息-填写内容
        [ObservableProperty]
        private string _infor_text_in;

        [ObservableProperty]
        private string _infor_text_write;

        [RelayCommand]
        private void Infor_text_btnFun()
        {

        }
        #endregion

        #region 信息-日期
        [ObservableProperty]
        private string _infor_date_in;

        [ObservableProperty]
        private DateTime _infor_date_selecttime;

        [RelayCommand]
        private void Infor_Data_BtnFun()
        {

        }
        #endregion




        //#region 责任人下拉框

        //private ObservableCollection<PeopleTable> _employees;
        //private PeopleTable _selectedEmployee;
        //private string _statusMessage;

        //// 员工列表 - 用于下拉框
        //public ObservableCollection<PeopleTable> Employees
        //{
        //    get => _employees;
        //    set
        //    {
        //        _employees = value;
        //        OnPropertyChanged();
        //    }
        //}

        //// 选中的员工
        //public PeopleTable SelectedEmployee
        //{
        //    get => _selectedEmployee;
        //    set
        //    {
        //        _selectedEmployee = value;
        //        OnPropertyChanged();
        //        UpdateStatusMessage();
        //    }
        //}

        //// 状态信息
        //public string StatusMessage
        //{
        //    get => _statusMessage;
        //    set
        //    {
        //        _statusMessage = value;
        //        OnPropertyChanged();
        //    }
        //}

        //// 加载员工数据
        //private void LoadEmployees()
        //{
        //    try
        //    {
        //        using (var context = new ProjectContext())
        //        {
        //            var employees = context.PeopleTable
        //                .OrderBy(e => e.PeopleName)
        //                .ToList();

        //            Employees = new ObservableCollection<PeopleTable>(employees);

        //            if (Employees.Count > 0)
        //            {
        //                //SelectedFollowEmployee = Employees[0];
        //                SelectedEmployee = Employees[0];
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //// 更新状态信息
        //private void UpdateStatusMessage()
        //{
        //    if (SelectedEmployee != null)
        //    {
        //        //StatusMessage = $"选中: {SelectedEmployee.PeopleName} ({SelectedEmployee.PeopleId})";
        //        ProjectsLeaderID = SelectedEmployee.PeopleId;
        //    }
        //}


        //#endregion
        //#region 跟进人下拉框

        //private PeopleTable _selectedFollowEmployee;


        //// 选中
        //public PeopleTable SelectedFollowEmployee
        //{
        //    get => _selectedFollowEmployee;
        //    set
        //    {
        //        _selectedFollowEmployee = value;
        //        OnPropertyChanged();
        //        UpdateFollowStatusMessage();
        //    }
        //}

        //// 更新状态信息
        //private void UpdateFollowStatusMessage()
        //{
        //    if (SelectedFollowEmployee != null)
        //    {
        //        ProjectsfollowuppersonId = SelectedFollowEmployee.PeopleId;
        //    }

        //}


        //#endregion


        //#region 设备类型下拉框

        //private ObservableCollection<EquipmentType> _equipmentTypes;
        //private EquipmentType _selectedEquipmentType;

        //// 列表 - 用于下拉框
        //public ObservableCollection<EquipmentType> EquipmentTypes
        //{
        //    get => _equipmentTypes;
        //    set
        //    {
        //        _equipmentTypes = value;
        //        OnPropertyChanged();
        //    }
        //}

        //// 选中
        //public EquipmentType SelectedEquipmentType
        //{
        //    get => _selectedEquipmentType;
        //    set
        //    {
        //        _selectedEquipmentType = value;
        //        OnPropertyChanged();
        //        UpdateEquipmentTypeStatus();
        //    }
        //}

        //// 加载数据
        //private void LoadEquipmentType()
        //{
        //    try
        //    {
        //        using (var context = new ProjectContext())
        //        {
        //            var equipmenttypes = context.EquipmentType
        //                .OrderBy(e => e.EquipmentName)
        //                .ToList();

        //            EquipmentTypes = new ObservableCollection<EquipmentType>(equipmenttypes);

        //            if (EquipmentTypes.Count > 0)
        //                SelectedEquipmentType = EquipmentTypes[0];

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //StatusMessage = $"加载失败: {ex.Message}";
        //    }
        //}

        //// 更新状态信息
        //private void UpdateEquipmentTypeStatus()
        //{

        //    if (SelectedEquipmentType != null)
        //    {
        //        EquipmenttypeId = SelectedEquipmentType.EquipmentTypeId;
        //    }

        //}


        //#endregion


        //#region 项目类型下拉框

        //private ObservableCollection<TypeTable> _types;
        //private TypeTable _selectedType;

        //// 员工列表 - 用于下拉框
        //public ObservableCollection<TypeTable> Types
        //{
        //    get => _types;
        //    set
        //    {
        //        _types = value;
        //        OnPropertyChanged();
        //    }
        //}

        //// 选中
        //public TypeTable SelectedType
        //{
        //    get => _selectedType;
        //    set
        //    {
        //        _selectedType = value;
        //        OnPropertyChanged();
        //        UpdateTypeStatus();
        //    }
        //}

        //// 加载数据
        //private void LoadType()
        //{
        //    try
        //    {
        //        using (var context = new ProjectContext())
        //        {
        //            var types = context.TypeTable
        //                .OrderBy(e => e.TypeName)
        //                .ToList();

        //            Types = new ObservableCollection<TypeTable>(types);

        //            if (Types.Count > 0)
        //                SelectedType = Types[0];

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //// 更新状态信息
        //private void UpdateTypeStatus()
        //{
        //    if (SelectedType != null)
        //    {
        //        TypeId = SelectedType.TypeId;
        //    }

        //}


        //#endregion


        //#region 阶段下拉框

        //private ObservableCollection<ProjectStage> _projectstage;
        //private ProjectStage _selectedprojectstage;

        //// 列表 - 用于下拉框
        //public ObservableCollection<ProjectStage> Projectstage
        //{
        //    get => _projectstage;
        //    set
        //    {
        //        _projectstage = value;
        //        OnPropertyChanged();
        //    }
        //}

        //// 选中
        //public ProjectStage Selectedprojectstage
        //{
        //    get => _selectedprojectstage;
        //    set
        //    {
        //        _selectedprojectstage = value;
        //        OnPropertyChanged();
        //        UpdateProjectStage();
        //    }
        //}

        //// 加载数据
        //private void Loadprojectstage()
        //{
        //    try
        //    {
        //        using (var context = new ProjectContext())
        //        {
        //            var projectstage = context.ProjectStage
        //                .OrderBy(e => e.ProjectStageName)
        //                .ToList();

        //            Projectstage = new ObservableCollection<ProjectStage>(projectstage);

        //            if (Projectstage.Count > 0)
        //                Selectedprojectstage = Projectstage[0];

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //// 更新状态信息
        //private void UpdateProjectStage()
        //{
        //    if (Selectedprojectstage != null)
        //    {
        //        ProjectStageId = Selectedprojectstage.ProjectStageId;
        //    }

        //}


        //#endregion


        //#region 项目阶段状态下拉框

        //private ObservableCollection<ProjectPhaseStatus> _projectphasestatus;
        //private ProjectPhaseStatus _selectedprojectphasestatus;

        //// 列表 - 用于下拉框
        //public ObservableCollection<ProjectPhaseStatus> Projectphasestatus
        //{
        //    get => _projectphasestatus;
        //    set
        //    {
        //        _projectphasestatus = value;
        //        OnPropertyChanged();
        //    }
        //}

        //// 选中
        //public ProjectPhaseStatus Selectedprojectphasestatus
        //{
        //    get => _selectedprojectphasestatus;
        //    set
        //    {
        //        _selectedprojectphasestatus = value;
        //        OnPropertyChanged();
        //        UpdateProjectPhaseStatus();
        //    }
        //}

        //// 加载数据
        //private void Loadprojectphasestatus()
        //{
        //    try
        //    {
        //        using (var context = new ProjectContext())
        //        {
        //            var projectphasestatus = context.ProjectPhaseStatus
        //                .OrderBy(e => e.ProjectPhaseStatusName)
        //                .ToList();

        //            Projectphasestatus = new ObservableCollection<ProjectPhaseStatus>(projectphasestatus);

        //            if (Projectphasestatus.Count > 0)
        //                Selectedprojectphasestatus = Projectphasestatus[0];

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //// 更新状态信息
        //private void UpdateProjectPhaseStatus()
        //{
        //    if (Selectedprojectstage != null)
        //    {
        //        ProjectPhaseStatusId = Selectedprojectphasestatus.ProjectPhaseStatusId;
        //    }

        //}


        //#endregion

    }
}
