using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectCycleManage.ViewModel
{
    public partial class NewProjectVM :ObservableObject
    {
        [ObservableProperty]
        private string _projectname;

        [ObservableProperty]
        private string _projectidentifyingnumber;

        [ObservableProperty]
        private int _projectid;

        [ObservableProperty]
        private string _applicant;  //申请人

        [ObservableProperty]
        private string _usingdepartment;  //使用部门

        [ObservableProperty]
        private string _year;  //年份

        [ObservableProperty] 
        private DateTime _applicationtime;  //申请部门

        [ObservableProperty]
        private bool _isenable_people;

        /// <summary>
        /// 登陆人
        /// </summary>
        private string _loginpersonname;

        /// <summary>
        /// 登陆者级别
        /// </summary>
        private int _loginpersonnamegrade;

        public NewProjectVM(int loginpeoplegrade, string loginpeoplename)
        {
            _loginpersonname = loginpeoplename;
            _loginpersonnamegrade = loginpeoplegrade;

            LoadEmployees();
            LoadEquipmentType();
            LoadType();

            Applicationtime = DateTime.Now;

            bool ReviewerRequre = false;
            using (var context = new ProjectContext())
            {
                ///Projectid = context.Projects.Count() + 1;

                var currentUser = context.PeopleTable
                    .FirstOrDefault(p => p.PeopleName == loginpeoplename);
                ReviewerRequre = context.TypeApprFlowPersSeqTable
                .Any(t => t.ReviewerPeopleId == currentUser.PeopleId && t.Mark != "Dele");
            }
            if (ReviewerRequre)
            {
                Isenable_people = true;
            }
            else
            {
                Isenable_people = false;
            }

            Projectidentifyingnumber = IdentifyingnumberFun();
            Year = DateTime.Now.Year.ToString();
            
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
                        SelectedEmployee = null;
                    }

                    SelectedEmployee = Employees?.FirstOrDefault(e => e.PeopleName == _loginpersonname);
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
                //ProjectsLeaderID = SelectedEmployee.PeopleId;
            }
        }


        #endregion
        #region 设备类型下拉框-例：非标外购

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
                ///EquipmenttypeId = SelectedEquipmentType.EquipmentTypeId;
            }
                
        }


        #endregion
        #region 项目类型下拉框，例：复制/新增/改善

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
                //TypeId = SelectedType.TypeId;
            }
            
        }


        #endregion

        /// <summary>
        /// 获取项目编号，例-HDGC2024038
        /// </summary>
        /// <returns></returns>
        private string IdentifyingnumberFun()
        {
            int DateTimeCounts = 0;
            try
            {
                using (var context = new ProjectContext())
                {
                    //Projectid = context.Projects.Count() + 1;

                    int count = context.Projects
                        .Where(p => p.Year == DateTime.Now.Year)
                        .Count();
                    DateTimeCounts = count + 1;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("获取当年项目数失败！");
                //throw;
            }

            string resultcount = null;

            if (DateTimeCounts < 10)
                resultcount = "00" + DateTimeCounts.ToString();
            else if (DateTimeCounts >= 10 && DateTimeCounts < 100)
                resultcount = "0" + DateTimeCounts.ToString();
            else if (DateTimeCounts >= 100)
                resultcount = DateTimeCounts.ToString();


            return "HDGC" + DateTime.Now.Year.ToString() + resultcount;


        }

        [RelayCommand]
        private void ResetData()
        {

        }

        [RelayCommand]
        private void SaveData()
        {
            try
            {
                using (var context = new ProjectContext())
                {
                    Projectid = context.Projects.Count() + 1;
                }

                if (Projectid != null && Projectid != 0 &&
                Projectidentifyingnumber != null &&
                SelectedEmployee != null && Year != null)
                {
                    if (Projectname != null && SelectedEquipmentType != null &&
                        SelectedType != null && Applicationtime != null)
                    {
                        MessageBoxResult result = MessageBox.Show(
                            "是否提交审批？",
                            "确认",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question
                        );

                        Projects NewProject = NewProjectFun();

                        //提交审批
                        if (result == MessageBoxResult.Yes)
                        {
                            NewProject.ProjectStageId = 106;
                            NewProject.ProjInforId = 101;
                            NewProject.ProjectPhaseStatusId = 105;
                        }
                        //不提交审批
                        else if (result == MessageBoxResult.No)
                        {
                            NewProject.ProjectStageId = 106;
                            NewProject.ProjInforId = 100;
                            NewProject.ProjectPhaseStatusId = 102;
                        }
                        int Saveresult = 0;
                        try
                        {
                            using (var context = new ProjectContext())
                            {
                                context.Projects.Add(NewProject);
                                Saveresult = context.SaveChanges();
                            }
                        }
                        catch (Exception)
                        {

                            //throw;
                            MessageBox.Show("数据库故障，请稍后刷新重新保存");
                        }
                        finally
                        {
                            if (result > 0)
                            {
                                MessageBox.Show("保存成功");
                            }
                            else
                            {
                                MessageBox.Show("保存失败");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("必填项为空，请检查红色星号必填项");
                    }
                }
                else
                {
                    MessageBox.Show("系统自动生成数据有误，请重新打开页面！");
                }

            }
            catch (Exception)
            {

                //throw;
            }
        }

        private Projects NewProjectFun()
        {
            var project = new Projects()
            {
                //_projectsid
                ProjectsId = Projectid,

                //_year
                Year = Convert.ToInt32(Year),

                //_projectName
                ProjectName = Projectname,

                //_equipmenttypeId
                equipmenttypeId = SelectedEquipmentType.EquipmentTypeId,

                //_projectIdentifyingNumber
                ProjectIdentifyingNumber = Projectidentifyingnumber,

                //_typeId
                typeId = SelectedType.TypeId,


                //_startTime
                ApplicationTime = Applicationtime,

                //_projectsLeaderID
                ProjectLeaderId = SelectedEmployee.PeopleId,

            };
            return project;
        }
    }
}