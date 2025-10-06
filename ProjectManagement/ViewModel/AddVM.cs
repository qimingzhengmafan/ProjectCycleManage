using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Model;
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
    public partial class AddVM: ObservableObject
    {
        public ProjectContext Context { get; set; }
        private ContextModel _contextModel { get; set; }
        DateTime currenttime = DateTime.Now;
        //int year = currenttime.Year;
        public AddVM()
        {
            _contextModel = new ContextModel(Context);
            Year = currenttime.Year;
            ProcurementMonth = currenttime;


            LoadEmployees();
            LoadType();
            LoadEquipmentType();
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
            ProjectsfollowuppersonId = SelectedFollowEmployee.PeopleId;
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
                    var employees = context.EquipmentType
                        .OrderBy(e => e.EquipmentName)
                        .ToList();

                    EquipmentTypes = new ObservableCollection<EquipmentType>(employees);

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
            //if (SelectedEmployee != null)
            //{
            //    StatusMessage = $"选中: {SelectedEmployee.PeopleName} ({SelectedEmployee.PeopleId})";
            //    MessageBox.Show(StatusMessage);
            //}
            EquipmenttypeId = SelectedEquipmentType.EquipmentTypeId;
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
            TypeId = SelectedType.TypeId;
        }


        #endregion



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

        #endregion

    }

}
