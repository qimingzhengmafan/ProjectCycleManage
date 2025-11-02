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
                        return;
                        
                    }

                    if (Infor_equipmenttype == "设备类型")
                    {
                        Task.Run(() =>
                        {
                            LoadEquipmentType();
                        });
                        return;
                    }
                    if (Infor_type == "项目类型")
                    {
                        Task.Run(() =>
                        {
                            LoadType();
                        });
                        return;
                    }
                    if (Infor_projectstage == "阶段")
                    {
                        Task.Run(() =>
                        {
                            Loadprojectstage();
                        });
                        return;
                    }
                    if (Infor_projectphasestatus == "项目阶段状态")
                    {
                        Task.Run(() =>
                        {
                            Loadprojectphasestatus();
                        });
                        return;
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
                    switch (Filename)
                    {
                        //102
                        case "设备申请表":
                            Fileisexist = GetFileValueByRemarkDynamic(Convert.ToInt32(Inforprojectid),102).GetAwaiter().GetResult();
                            break;
                        case "技术协议":
                            Fileisexist = GetFileValueByRemarkDynamic(Convert.ToInt32(Inforprojectid), 103).GetAwaiter().GetResult();
                            break;
                        case "设备方案 OR Boom清单":
                            Fileisexist = GetFileValueByRemarkDynamic(Convert.ToInt32(Inforprojectid), 104).GetAwaiter().GetResult();
                            break;
                        case "设备项目问题改善":
                            Fileisexist = GetFileValueByRemarkDynamic(Convert.ToInt32(Inforprojectid), 105).GetAwaiter().GetResult();
                            break;
                        case "设备验证记录":
                            Fileisexist = GetFileValueByRemarkDynamic(Convert.ToInt32(Inforprojectid), 106).GetAwaiter().GetResult();
                            break;
                        case "培训记录":
                            Fileisexist = GetFileValueByRemarkDynamic(Convert.ToInt32(Inforprojectid), 107).GetAwaiter().GetResult();
                            break;
                        case "说明书":
                            Fileisexist = GetFileValueByRemarkDynamic(Convert.ToInt32(Inforprojectid), 108).GetAwaiter().GetResult();
                            break;
                        case "维保文件":
                            Fileisexist = GetFileValueByRemarkDynamic(Convert.ToInt32(Inforprojectid), 109).GetAwaiter().GetResult();
                            break;
                        case "WI":
                            Fileisexist = GetFileValueByRemarkDynamic(Convert.ToInt32(Inforprojectid), 110).GetAwaiter().GetResult();
                            break;
                        case "设备验收单":
                            Fileisexist = GetFileValueByRemarkDynamic(Convert.ToInt32(Inforprojectid), 111).GetAwaiter().GetResult();
                            break;

                        //112
                        case "文件发放记录":
                            Fileisexist = GetFileValueByRemarkDynamic(Convert.ToInt32(Inforprojectid), 112).GetAwaiter().GetResult();
                            break;
                        default:
                            break;
                    }
                }
                else if (_infortype == "文档-OA")
                {
                    if (File_oa_indata == "OA申请单号")
                    {
                        File_oa_writedata = GetOAApplicationFileValueByRemarkDynamic(Convert.ToInt32(Inforprojectid)).GetAwaiter().GetResult();
                    }
                    else if (File_oa_indata == "OA领用单号")
                    {

                        File_oa_writedata = GetOAUseFileValueByRemarkDynamic(Convert.ToInt32(Inforprojectid)).GetAwaiter().GetResult();
                    }
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


        #region 设备类型下拉框
        [ObservableProperty]
        private string _infor_equipmenttype;

        private ObservableCollection<EquipmentType> _infor_equipmentTypes;
        
        // 列表 - 用于下拉框
        public ObservableCollection<EquipmentType> Infor_EquipmentTypes
        {
            get => _infor_equipmentTypes;
            set
            {
                _infor_equipmentTypes = value;
                OnPropertyChanged();
            }
        }

        private EquipmentType _selectedEquipmentType;
        // 选中
        public EquipmentType SelectedEquipmentType
        {
            get => _selectedEquipmentType;
            set
            {
                _selectedEquipmentType = value;
                OnPropertyChanged();
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

                    Infor_EquipmentTypes = new ObservableCollection<EquipmentType>(equipmenttypes);

                    if (Infor_EquipmentTypes.Count > 0)
                        SelectedEquipmentType = null;



                }

                using (var context = new ProjectContext())
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
                        if (project.equipmenttypeId.HasValue)
                        {
                            SelectedEquipmentType = Infor_EquipmentTypes?.FirstOrDefault(e => e.EquipmentTypeId == project.equipmenttypeId.Value);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                //StatusMessage = $"加载失败: {ex.Message}";
            }
        }
        [RelayCommand]
        private void Infor_Equipment_BtnFun()
        {
            Task.Run(() =>
            {
                try
                {
                    using (var context = new ProjectContext())
                    {
                        // 先查询出要更新的实体
                        var entity = context.Projects
                            .FirstOrDefault(x => x.ProjectsId == Convert.ToInt32(Inforprojectid));

                        if (entity != null)
                        {
                            // 更新指定字段
                            entity.equipmenttypeId = SelectedEquipmentType.EquipmentTypeId;

                            // 保存更改
                            context.SaveChangesAsync();
                        }
                        MessageBox.Show("保存成功");

                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("保存失败");

                    //throw;
                }

            });
        }
        #endregion

        #region 项目类型下拉框

        [ObservableProperty]
        private string _infor_type;

        private ObservableCollection<TypeTable> _types;
        public ObservableCollection<TypeTable> Types
        {
            get => _types;
            set
            {
                _types = value;
                OnPropertyChanged();
            }
        }

        private TypeTable _selectedType;
        // 选中
        public TypeTable SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                OnPropertyChanged();
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
                        SelectedType = null;

                }

                using (var context = new ProjectContext())
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
                        if (project.typeId.HasValue)
                        {
                            SelectedType = Types?.FirstOrDefault(e => e.TypeId == project.typeId.Value);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        [RelayCommand]
        private void Infor_Type_BtnFun()
        {
            Task.Run(() =>
            {
                try
                {
                    using (var context = new ProjectContext())
                    {
                        // 先查询出要更新的实体
                        var entity = context.Projects
                            .FirstOrDefault(x => x.ProjectsId == Convert.ToInt32(Inforprojectid));

                        if (entity != null)
                        {
                            // 更新指定字段
                            entity.typeId = SelectedType.TypeId;

                            // 保存更改
                            context.SaveChangesAsync();
                        }
                        MessageBox.Show("保存成功");

                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("保存失败");

                    //throw;
                }

            });
        }
        #endregion

        #region 阶段下拉框
        [ObservableProperty]
        private string _infor_projectstage;

        private ObservableCollection<ProjectStage> _projectstage;
        public ObservableCollection<ProjectStage> Projectstage
        {
            get => _projectstage;
            set
            {
                _projectstage = value;
                OnPropertyChanged();
            }
        }


        private ProjectStage _selectedprojectstage;
        // 选中
        public ProjectStage Selectedprojectstage
        {
            get => _selectedprojectstage;
            set
            {
                _selectedprojectstage = value;
                OnPropertyChanged();
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
                        Selectedprojectstage = null;

                }

                using (var context = new ProjectContext())
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
                        Selectedprojectstage = Projectstage?.FirstOrDefault(e => e.ProjectStageId == project.ProjectStageId);
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        [RelayCommand]
        private void Infor_ProjectStage_BtnFun()
        {
            Task.Run(() =>
            {
                try
                {
                    using (var context = new ProjectContext())
                    {
                        // 先查询出要更新的实体
                        var entity = context.Projects
                            .FirstOrDefault(x => x.ProjectsId == Convert.ToInt32(Inforprojectid));

                        if (entity != null)
                        {
                            // 更新指定字段
                            entity.ProjectStageId = Selectedprojectstage.ProjectStageId;

                            // 保存更改
                            context.SaveChangesAsync();
                        }
                        MessageBox.Show("保存成功");

                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("保存失败");

                    //throw;
                }

            });
        }

        #endregion

        #region 项目阶段状态下拉框
        [ObservableProperty]
        private string _infor_projectphasestatus;

        private ObservableCollection<ProjectPhaseStatus> _projectphasestatus;
        public ObservableCollection<ProjectPhaseStatus> Projectphasestatus
        {
            get => _projectphasestatus;
            set
            {
                _projectphasestatus = value;
                OnPropertyChanged();
            }
        }

        private ProjectPhaseStatus _selectedprojectphasestatus;
        // 选中
        public ProjectPhaseStatus Selectedprojectphasestatus
        {
            get => _selectedprojectphasestatus;
            set
            {
                _selectedprojectphasestatus = value;
                OnPropertyChanged();
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

                using (var context = new ProjectContext())
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
                        Selectedprojectphasestatus = Projectphasestatus?.FirstOrDefault(e => e.ProjectPhaseStatusId == project.ProjectPhaseStatusId.Value);
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        [RelayCommand]
        private void Infor_projectphase_BtnFun()
        {
            Task.Run(() =>
            {
                try
                {
                    using (var context = new ProjectContext())
                    {
                        // 先查询出要更新的实体
                        var entity = context.Projects
                            .FirstOrDefault(x => x.ProjectsId == Convert.ToInt32(Inforprojectid));

                        if (entity != null)
                        {
                            // 更新指定字段
                            entity.ProjectPhaseStatusId = Selectedprojectphasestatus.ProjectPhaseStatusId;

                            // 保存更改
                            context.SaveChangesAsync();
                        }
                        MessageBox.Show("保存成功");

                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("保存失败");

                    //throw;
                }

            });
        }

        #endregion

        public InformationCardVM()
        {

        }


        /// <summary>
        /// 通过备注更新主表记录（信息部分专用）
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

        //OA申请单号
        public async Task<string?> GetOAApplicationFileValueByRemarkDynamic(int recordId)
        {
            using (var context = new ProjectContext())
            {
                var query = context.ProjectDocumentStatus
                    .Where(p => p.ProjectsId == recordId)
                    .FirstOrDefault(p => p.DocumentTypeId == 101);

                var result = query.Remarks;

                //if (result == null)
                //{
                //    throw new KeyNotFoundException($"未找到ID为 {recordId} 的记录");
                //}

                return result;
            }
        }

        //OA领用单号
        public async Task<string?> GetOAUseFileValueByRemarkDynamic(int recordId)
        {
            using (var context = new ProjectContext())
            {
                var query = context.ProjectDocumentStatus
                    .Where(p => p.ProjectsId == recordId)
                    .FirstOrDefault(p => p.DocumentTypeId == 113);

                var result = query.Remarks;

                //if (result == null)
                //{
                //    throw new KeyNotFoundException($"未找到ID为 {recordId} 的记录");
                //}

                return result;
            }
        }

        //文件是否存在
        public async Task<bool?> GetFileValueByRemarkDynamic(int recordId , int FileID)
        {
            using (var context = new ProjectContext())
            {
                var query = context.ProjectDocumentStatus
                    .Where(p => p.ProjectsId == recordId)
                    .FirstOrDefault(p => p.DocumentTypeId == FileID);

                var result = query.IsHasDocument;

                //if (result == null)
                //{
                //    throw new KeyNotFoundException($"未找到ID为 {recordId} 的记录");
                //}

                return (bool)result;
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
