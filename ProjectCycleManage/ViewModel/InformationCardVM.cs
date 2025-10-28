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
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProjectCycleManage.ViewModel
{
    public partial class InformationCardVM : ObservableObject
    {
        #region 公共属性

        /// <summary>
        /// 登录者级别
        /// </summary>
        [ObservableProperty]
        private int _loginpersonnamegrade;


        /// <summary>
        /// 控件等级/权限信息
        /// </summary>
        [ObservableProperty]
        private string _taginfor;

        /// <summary>
        /// 信息类型-例-文档-OA
        /// </summary>
        //[ObservableProperty]
        private string _infortype;
        public string Infortype
        {
            get => _infortype;
            set
            {
                _infortype = value;
                OnPropertyChanged();
                if (_infortype == "信息-下拉框")
                {
                    if (Infor_people == "项目负责人" || Infor_people == "跟进人")
                    {
                        Task.Run(() =>
                        {
                            LoadEmployees();
                        });
                        
                    }
                }
                else if (_infortype == "信息-填写")
                {
                    Infor_text_write = GetFieldValueByRemarkDynamic( Convert.ToInt32(Inforprojectid) , Infor_text_in).GetAwaiter().GetResult();
                }
                else if (_infortype == "信息-日期")
                {
                    Infor_date_selecttime = GetFieldValueByRemarkDynamicDateTime(Convert.ToInt32(Inforprojectid), Infor_date_in).GetAwaiter().GetResult();
                }
                else if (_infortype == "文档")
                {

                }
                else if (_infortype == "文档-OA")
                {

                }
            }
        }

        /// <summary>
        /// 项目ID
        /// </summary>
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
            MessageBox.Show("click" + Fileisexist.Value.ToString());
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

        #region 信息-人员下拉框
        [ObservableProperty]
        private string _infor_people;

        //[ObservableProperty]
        private ObservableCollection<PeopleTable> _infor_people_ob;
        public ObservableCollection<PeopleTable> Infor_people_ob
        {
            get => _infor_people_ob;
            set
            {
                _infor_people_ob = value;
                OnPropertyChanged();
            }
        }

        //[ObservableProperty]
        private PeopleTable _infor_sele_people;
        public PeopleTable Infor_sele_people
        {
            get => _infor_sele_people;
            set
            {
                _infor_sele_people = value;
                OnPropertyChanged();
                //UpdateStatusMessage();
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

                    Infor_people_ob = new ObservableCollection<PeopleTable>(employees);

                    if (Infor_people_ob.Count > 0)
                    {
                        //SelectedFollowEmployee = Employees[0];
                        //Infor_sele_people = Infor_people_ob[0];
                        Infor_sele_people = null;
                    }
                }

                using (var context = new ProjectContext())
                {
                    if (Infor_people == "跟进人")
                    {
                        var project = context.Projects
                            .Include(p => p.equipmenttype)
                            .Include(p => p.type)
                            .Include(p => p.ProjectStage)
                            .Include(p => p.ProjectPhaseStatus)
                            .Include(p => p.ProjectLeader)
                            .Include(p => p.projectfollowupperson)
                            .FirstOrDefault(p => p.ProjectsId == Convert.ToInt32(Inforprojectid));

                        if (project != null)
                        {
                            if (project.projectfollowuppersonId.HasValue)
                            {
                                Infor_sele_people = Infor_people_ob?.FirstOrDefault(e => e.PeopleId == project.projectfollowuppersonId.Value);
                            }
                        }
                    }
                    else if (Infor_people == "项目负责人")
                    {
                        var project = context.Projects
                            .Include(p => p.equipmenttype)
                            .Include(p => p.type)
                            .Include(p => p.ProjectStage)
                            .Include(p => p.ProjectPhaseStatus)
                            .Include(p => p.ProjectLeader)
                            .Include(p => p.projectfollowupperson)
                            .FirstOrDefault(p => p.ProjectsId == Convert.ToInt32(Inforprojectid));

                        if (project != null)
                        {
                            if (project.projectfollowuppersonId.HasValue)
                            {
                                Infor_sele_people = Infor_people_ob?.FirstOrDefault(e => e.PeopleId == project.ProjectLeaderId.Value);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        [RelayCommand]
        private void Infor_People_BtnFun()
        {
            Task.Run(() =>
            {
                try
                {
                    using (var context = new ProjectContext())
                    {
                        if (Infor_people == "跟进人")
                        {
                            // 先查询出要更新的实体
                            var entity = context.Projects
                                .FirstOrDefault(x => x.ProjectsId == Convert.ToInt32(Inforprojectid));

                            if (entity != null)
                            {
                                // 更新指定字段
                                entity.projectfollowuppersonId = Infor_sele_people.PeopleId;

                                // 保存更改
                                context.SaveChangesAsync();
                            }
                            MessageBox.Show("保存成功");
                        }
                        else if (Infor_people == "项目负责人")
                        {
                            // 先查询出要更新的实体
                            var entity = context.Projects
                                .FirstOrDefault(x => x.ProjectsId == Convert.ToInt32(Inforprojectid));

                            if (entity != null)
                            {
                                // 更新指定字段
                                entity.ProjectLeaderId = Infor_sele_people.PeopleId;

                                // 保存更改
                                context.SaveChangesAsync();
                            }
                            MessageBox.Show("保存成功");
                        }

                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("保存失败");

                    //throw;
                }
                //MessageBox.Show("Change!" + Infor_sele_people.PeopleName);
                
            });
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
            //MessageBox.Show("1" + Infor_text_write);
            UpdateByRemark(Convert.ToInt32(Inforprojectid) , Infor_text_in , Infor_text_write);

        }
        #endregion

        #region 信息-日期
        [ObservableProperty]
        private string _infor_date_in;

        [ObservableProperty]
        private DateTime? _infor_date_selecttime;

        [RelayCommand]
        private void Infor_Data_BtnFun()
        {
            //MessageBox.Show(Infor_date_selecttime.ToString());
        }
        #endregion

        public InformationCardVM()
        {

        }


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


        /// <summary>
        /// 通过备注更新主表记录
        /// </summary>
        /// <param name="mainTableId">主表记录的唯一ID</param>
        /// <param name="remark">已知的字段备注</param>
        /// <param name="newValue">要更新的新值</param>
        /// <returns>是否更新成功</returns>
        public async Task<UpdateResult> UpdateByRemark(int mainTableId, string remark, object newValue)
        {
            try
            {
                using var context = new ProjectContext();

                // 查找字段映射
                var fieldRemark = await context.InformationTable
                    .FirstOrDefaultAsync(r => r.Reamrks == remark);

                if (fieldRemark == null)
                {
                    return UpdateResult.Fail($"未找到备注 '{remark}' 对应的字段映射");
                }

                string targetFieldName = fieldRemark.Infor;

                // 验证字段存在性
                var targetProperty = typeof(Projects).GetProperty(targetFieldName);
                if (targetProperty == null)
                {
                    return UpdateResult.Fail($"主表不存在字段：{targetFieldName}");
                }

                // 查找实体
                var entity = await context.Projects
                    .FirstOrDefaultAsync(m => m.ProjectsId == mainTableId);

                if (entity == null)
                {
                    return UpdateResult.Fail($"未找到ID为 {mainTableId} 的记录");
                }

                // 类型转换并赋值
                try
                {
                    var convertedValue = Convert.ChangeType(newValue, targetProperty.PropertyType);
                    targetProperty.SetValue(entity, convertedValue);
                }
                catch (InvalidCastException)
                {
                    return UpdateResult.Fail($"值 '{newValue}' 无法转换为字段 {targetFieldName} 的类型 {targetProperty.PropertyType.Name}");
                }

                // 保存更改
                await context.SaveChangesAsync();
                return UpdateResult.Success();
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "更新字段时发生错误");
                return UpdateResult.Fail($"更新失败：{ex.Message}");
            }
        }
        public async Task<string> GetFieldValueByRemarkDynamic(int recordId, string remark)
        {
            using (var context = new ProjectContext())
            {
                // 步骤1：根据备注查询对应的字段名
                var fieldMapping = context.InformationTable
                .FirstOrDefault(r => r.Reamrks == remark);

                if (fieldMapping == null)
                {
                    throw new KeyNotFoundException($"未找到备注 '{remark}' 对应的字段名");
                }
                string targetFieldName = fieldMapping.Infor;

                // 步骤2：使用动态LINQ只查询特定字段
                // 构建只选择目标字段的查询
                var query = context.Projects
                    .Where(p => p.ProjectsId == recordId)
                    .Select($"new ({targetFieldName} as Value)");

                var result =query.Cast<dynamic>().FirstOrDefault();

                if (result == null)
                {
                    throw new KeyNotFoundException($"未找到ID为 {recordId} 的记录");
                }

                return ConvertToString(result.Value);
            }


        }

        public async Task<DateTime?> GetFieldValueByRemarkDynamicDateTime(int recordId, string remark)
        {
            using (var context = new ProjectContext())
            {
                // 步骤1：根据备注查询对应的字段名
                var fieldMapping = context.InformationTable
                .FirstOrDefault(r => r.Reamrks == remark);

                if (fieldMapping == null)
                {
                    throw new KeyNotFoundException($"未找到备注 '{remark}' 对应的字段名");
                }
                string targetFieldName = fieldMapping.Infor;

                // 步骤2：使用动态LINQ只查询特定字段
                // 构建只选择目标字段的查询
                var query = context.Projects
                    .Where(p => p.ProjectsId == recordId)
                    .Select($"new ({targetFieldName} as Value)");

                var result = query.Cast<dynamic>().FirstOrDefault();

                if (result == null)
                {
                    throw new KeyNotFoundException($"未找到ID为 {recordId} 的记录");
                }

                return result.Value;
            }


        }

        // 返回结果类
        public class UpdateResult
        {
            public bool IsSuccess { get; set; }
            public string Message { get; set; }

            public static UpdateResult Success() => new UpdateResult { IsSuccess = true, Message = "操作成功" };
            public static UpdateResult Fail(string message) => new UpdateResult { IsSuccess = false, Message = message };
        }
        private string ConvertToString(object value)
        {
            if (value == null)
                return string.Empty;

            if (value is string str)
                return str;

            // 处理特殊类型
            if (value is DateTime dateTime)
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss");

            if (value is bool boolValue)
                return boolValue ? "是" : "否";

            if (value is decimal decimalValue)
                return decimalValue.ToString("F2");

            // 默认使用ToString()
            return value.ToString();
        }

    }
}
