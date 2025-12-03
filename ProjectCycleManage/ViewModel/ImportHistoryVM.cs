﻿﻿﻿﻿﻿﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Models;
using ProjectCycleManage.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectCycleManage.ViewModel
{
    public partial class ImportHistoryVM : ObservableObject
    {
        /// <summary>
        /// 登陆人
        /// </summary>
        private string _loginpersonname;

        /// <summary>
        /// 登陆者级别
        /// </summary>
        private int _loginpersonnamegrade;

        #region 基本信息属性

        [ObservableProperty]
        private string _projectName;  // 项目名称

        [ObservableProperty]
        private string _originalCode;  // 原始项目编号

        [ObservableProperty]
        private string _selectedDeviceType;  // 选中的设备类型

        [ObservableProperty]
        private int _projectid;

        #endregion

        #region 人员信息属性

        [ObservableProperty]
        private string _applicant;  // 申请人

        #endregion

        #region 部门与时间信息属性

        [ObservableProperty]
        private string _usingdepartment;  // 使用部门

        [ObservableProperty]
        private DateTime _applicationtime;  // 申请日期

        //[ObservableProperty]
        //private string _year;  // 年份

        [ObservableProperty]
        private DateTime _importDate;  // 导入日期

        #endregion


        #region 动态项目阶段信息属性

        /// <summary>
        /// 动态项目阶段列表
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<DynamicStageGroup> _dynamicStageGroups;

        #endregion

        #region 备注信息

        [ObservableProperty]
        private string _notes;  // 历史项目备注

        #endregion

        #region 控制属性

        [ObservableProperty]
        private bool _isServerSelected;  // 是否选择了服务器类型

        [ObservableProperty]
        private bool _hasDynamicContent;  // 是否有动态内容

        #endregion



        public ImportHistoryVM(int loginpeoplegrade, string loginpeoplename)
        {
            _loginpersonname = loginpeoplename;
            _loginpersonnamegrade = loginpeoplegrade;
            // 加载数据
            LoadEmployees();
            LoadEquipmentType();
            LoadType();
            LoadYears();

            // 初始化默认值
            Applicationtime = DateTime.Now;
            ImportDate = DateTime.Now;
            //Year = DateTime.Now.Year.ToString();
        }

        #region 项目负责人下拉框

        private ObservableCollection<PeopleTable> _employees;
        private PeopleTable _selectedEmployee;

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
                        SelectedEmployee = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载人员数据失败: {ex.Message}");
            }
        }

        #endregion

        #region 设备类型下拉框

        private ObservableCollection<EquipmentType> _equipmentTypes;
        private EquipmentType _selectedEquipmentType;

        // 设备类型列表 - 用于下拉框
        public ObservableCollection<EquipmentType> EquipmentTypes
        {
            get => _equipmentTypes;
            set
            {
                _equipmentTypes = value;
                OnPropertyChanged();
            }
        }

        // 选中的设备类型
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

        // 加载设备类型数据
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
                        SelectedEquipmentType = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载设备类型失败: {ex.Message}");
            }
        }

        // 更新设备类型状态
        private void UpdateEquipmentTypeStatus()
        {
            if (SelectedEquipmentType != null)
            {
                // 根据设备类型名称判断是否显示服务器信息
                // 可以根据实际需求调整判断逻辑
                //IsServerSelected = SelectedEquipmentType.EquipmentName.Contains("服务器");
                
                // 加载动态项目阶段信息
                LoadDynamicStageInfo();
            }
            else
            {
                IsServerSelected = false;
                HasDynamicContent = false;
                DynamicStageGroups = null;
            }
        }

        #endregion

        #region 项目类型下拉框

        private ObservableCollection<TypeTable> _types;
        private TypeTable _selectedType;

        // 项目类型列表 - 用于下拉框
        public ObservableCollection<TypeTable> Types
        {
            get => _types;
            set
            {
                _types = value;
                OnPropertyChanged();
            }
        }

        // 选中的项目类型
        public TypeTable SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                OnPropertyChanged();
            }
        }

        // 加载项目类型数据
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载项目类型失败: {ex.Message}");
            }
        }

        #endregion

        #region 年份下拉框

        private ObservableCollection<string> _years;
        private string _selectedYear;

        // 年份列表 - 用于下拉框
        public ObservableCollection<string> Years
        {
            get => _years;
            set
            {
                _years = value;
                OnPropertyChanged();
            }
        }

        // 选中的年份
        public string SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged();
            }
        }

        // 加载年份数据(最近10年)
        private void LoadYears()
        {
            try
            {
                Years = new ObservableCollection<string>();
                int currentYear = DateTime.Now.Year;
                
                // 生成从当前年份往前推10年的年份列表
                for (int i = 0; i < 10; i++)
                {
                    Years.Add((currentYear - i).ToString());
                }

                if (Years.Count > 0)
                    SelectedYear = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载年份数据失败: {ex.Message}");
            }
        }

        #endregion

        #region 命令

        /// <summary>
        /// 重置表单命令
        /// </summary>
        [RelayCommand]
        private void Reset()
        {
            try
            {
                // 清空基本信息
                ProjectName = string.Empty;
                OriginalCode = string.Empty;
                SelectedEquipmentType = null;
                SelectedType = null;

                // 清空人员信息
                SelectedEmployee = null;
                Applicant = string.Empty;

                // 清空部门与时间信息
                Usingdepartment = string.Empty;
                Applicationtime = DateTime.Now;
                SelectedYear = null;
                ImportDate = DateTime.Now;
                //Year = DateTime.Now.Year.ToString();

                // 清空备注
                Notes = string.Empty;

                MessageBox.Show("表单已重置", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"重置失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 导入历史项目命令
        /// </summary>
        [RelayCommand]
        private void Import()
        {
            try
            {
                // 验证必填字段
                if (string.IsNullOrWhiteSpace(ProjectName))
                {
                    MessageBox.Show("请输入项目名称", "验证失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(OriginalCode))
                {
                    MessageBox.Show("请输入原始项目编号", "验证失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (SelectedEquipmentType == null)
                {
                    MessageBox.Show("请选择设备类型", "验证失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (SelectedType == null)
                {
                    MessageBox.Show("请选择项目类型", "验证失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(SelectedYear))
                {
                    MessageBox.Show("请选择项目年份", "验证失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (SelectedEmployee == null)
                {
                    MessageBox.Show("请选择项目年份", "验证失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Applicationtime.Year.ToString() != SelectedYear)
                {
                    MessageBox.Show("申请日期与年份不一致", "验证失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 验证动态区域必填字段
                string dynamicValidationError = ValidateDynamicFields();
                if (!string.IsNullOrEmpty(dynamicValidationError))
                {
                    MessageBox.Show(dynamicValidationError, "验证失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 确认导入
                MessageBoxResult result = MessageBox.Show(
                    $"确认导入历史项目:{ProjectName}?",
                    "确认导入",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    // 创建历史项目对象
                    var historyProject = CreateHistoryProject();

                    // 保存到数据库
                    int saveResult = 0;
                    try
                    {
                        using (var context = new ProjectContext())
                        {
                            // 检查是否已存在相同的原始编号
                            var existingProject = context.Projects
                                .FirstOrDefault(p => p.ProjectIdentifyingNumber == OriginalCode);

                            if (existingProject != null)
                            {
                                MessageBox.Show("该原始项目编号已存在,请检查后重新输入", "导入失败", 
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }

                            context.Projects.Add(historyProject);
                            saveResult = context.SaveChanges();

                            // 保存成功后,保存动态阶段数据
                            if (saveResult > 0)
                            {
                                SaveDynamicStageData(context, historyProject.ProjectsId);
                                context.SaveChanges();
                            }
                        }

                        if (saveResult > 0)
                        {
                            MessageBox.Show("历史项目导入成功!", "成功", 
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            
                            // 导入成功后重置表单
                            Reset();
                        }
                        else
                        {
                            MessageBox.Show("导入失败,请重试", "失败", 
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"数据库操作失败: {ex.Message}", "错误", 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入失败: {ex.Message}", "错误", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 创建历史项目对象
        /// </summary>
        /// <returns></returns>
        private Projects CreateHistoryProject()
        {
            using (var context = new ProjectContext())
            {
                Projectid = context.Projects.Count() + 1;
            }

            var project = new Projects()
            {
                ProjectsId = Projectid,
                Year = Convert.ToInt32(SelectedYear),
                ProjectName = ProjectName,
                equipmenttypeId = SelectedEquipmentType.EquipmentTypeId,
                ProjectIdentifyingNumber = OriginalCode,  // 使用原始编号
                typeId = SelectedType.TypeId,
                ApplicationTime = Applicationtime,
                ProjectLeaderId = SelectedEmployee?.PeopleId ?? 0,
                
                // 历史项目特殊标记
                ProjectStageId = 105,  // 根据实际情况设置
                ProjInforId = 111,     // 标记为历史导入项目
                ProjectPhaseStatusId = 104,  // 设置为已完成或其他合适状态

                // 可以添加备注字段记录历史项目信息
                remarks = Notes
            };

            return project;
        }

        #endregion

        #region 动态字段验证

        /// <summary>
        /// 验证动态区域必填字段
        /// </summary>
        /// <returns>错误信息,没有错误返回null</returns>
        private string ValidateDynamicFields()
        {
            if (DynamicStageGroups == null || DynamicStageGroups.Count == 0)
            {
                // 如果没有动态区域,不需要验证
                return null;
            }

            var errorMessages = new List<string>();

            foreach (var stageGroup in DynamicStageGroups)
            {
                foreach (var item in stageGroup.Items)
                {
                    // 跳过废弃的字段
                    if (item.IsAbolished)
                        continue;

                    // 验证文档类型
                    if (item.IsDocument)
                    {
                        if (item.Type == "文档")
                        {
                            // 文档类型必须勾选
                            if (!item.CheckboxValue)
                            {
                                errorMessages.Add($"阶段【{stageGroup.StageName}】的【{item.Name}】必须勾选");
                            }
                        }
                        else if (item.Type == "文档-OA")
                        {
                            // 文档-OA必须填写
                            if (string.IsNullOrWhiteSpace(item.TextValue))
                            {
                                errorMessages.Add($"阶段【{stageGroup.StageName}】的【{item.Name}】必须填写");
                            }
                        }
                    }
                    // 验证信息类型
                    else
                    {
                        if (item.Type == "信息-填写")
                        {
                            // 信息-填写必须填写
                            if (string.IsNullOrWhiteSpace(item.TextValue))
                            {
                                errorMessages.Add($"阶段【{stageGroup.StageName}】的【{item.Name}】必须填写");
                            }
                        }
                        else if (item.Type == "信息-下拉框")
                        {
                            // 信息-下拉框必须选择
                            if (item.SelectedComboValue == null)
                            {
                                errorMessages.Add($"阶段【{stageGroup.StageName}】的【{item.Name}】必须选择");
                            }
                        }
                        else if (item.Type == "信息-日期")
                        {
                            // 信息-日期必须选择
                            if (!item.DateValue.HasValue)
                            {
                                errorMessages.Add($"阶段【{stageGroup.StageName}】的【{item.Name}】必须选择日期");
                            }
                        }
                    }
                }
            }

            // 如果有错误,返回第一个错误信息
            if (errorMessages.Count > 0)
            {
                return errorMessages[0];
            }

            return null;
        }

        #endregion

        #region 保存动态阶段数据

        /// <summary>
        /// 保存动态阶段数据到数据库
        /// </summary>
        /// <param name="context">数据库上下文</param>
        /// <param name="projectId">项目ID</param>
        private void SaveDynamicStageData(ProjectContext context, int projectId)
        {
            if (DynamicStageGroups == null || DynamicStageGroups.Count == 0)
                return;

            // 获取当前项目对象
            var project = context.Projects.FirstOrDefault(p => p.ProjectsId == projectId);
            if (project == null)
                return;

            // 遍历所有阶段和字段
            foreach (var stageGroup in DynamicStageGroups)
            {
                foreach (var item in stageGroup.Items)
                {
                    // 跳过废弃的字段
                    if (item.IsAbolished)
                        continue;

                    if (item.IsDocument)
                    {
                        // 处理文档类型
                        SaveDocumentStatus(context, projectId, item);
                    }
                    else
                    {
                        // 处理信息类型 - 更新Projects表字段
                        UpdateProjectInfoFields(project, item);
                    }
                }
            }
        }

        /// <summary>
        /// 保存文档状态到ProjectDocumentStatus表
        /// </summary>
        private void SaveDocumentStatus(ProjectContext context, int projectId, DynamicStageItem item)
        {
            if (!item.DocumentTypeId.HasValue)
                return;

            bool isHasDocument = false;
            string remarks = null;

            if (item.Type == "文档")
            {
                // 文档类型: 根据CheckBox判断
                isHasDocument = item.CheckboxValue;
            }
            else if (item.Type == "文档-OA")
            {
                // 文档-OA类型: 根据TextBox是否为空判断
                isHasDocument = !string.IsNullOrWhiteSpace(item.TextValue);
                remarks = item.TextValue;
            }

            // 只有当isHasDocument=true时才插入记录(根据需求导入时一般为1)
            if (!isHasDocument)
                return;

            // 检查是否已存在该文档记录
            var existingDoc = context.ProjectDocumentStatus
                .FirstOrDefault(pds => pds.ProjectsId == projectId && pds.DocumentTypeId == item.DocumentTypeId.Value);

            if (existingDoc == null)
            {
                // 获取当前登录人ID
                var loginPerson = context.PeopleTable.FirstOrDefault(p => p.PeopleName == _loginpersonname);
                int? updatePeopleId = loginPerson?.PeopleId;

                // 不存在则新增
                var docStatus = new ProjectDocumentStatus
                {
                    ProjectsId = projectId,
                    DocumentTypeId = item.DocumentTypeId.Value,
                    IsHasDocument = true,  // 导入时一般为1
                    Remarks = remarks,
                    TheLastUpDateTime = DateTime.Now,
                    UpdatePeopleId = updatePeopleId
                };
                context.ProjectDocumentStatus.Add(docStatus);
            }
        }

        /// <summary>
        /// 更新Projects表的信息字段
        /// </summary>
        private void UpdateProjectInfoFields(Projects project, DynamicStageItem item)
        {
            if (string.IsNullOrWhiteSpace(item.InformationFieldName))
                return;

            // 获取Projects类型
            var projectType = typeof(Projects);
            var propertyInfo = projectType.GetProperty(item.InformationFieldName);

            if (propertyInfo == null)
                return;

            // 根据不同的控件类型设置值
            object value = null;

            if (item.Type == "信息-填写")
            {
                value = item.TextValue;
            }
            else if (item.Type == "信息-下拉框")
            {
                // 下拉框为跟进人,保存PeopleId
                if (item.SelectedComboValue != null)
                {
                    value = item.SelectedComboValue.PeopleId;
                }
            }
            else if (item.Type == "信息-日期")
            {
                value = item.DateValue;
            }

            // 设置属性值
            if (value != null)
            {
                try
                {
                    // 类型转换
                    if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(int?))
                    {
                        propertyInfo.SetValue(project, Convert.ToInt32(value));
                    }
                    else if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
                    {
                        propertyInfo.SetValue(project, value);
                    }
                    else if (propertyInfo.PropertyType == typeof(string))
                    {
                        propertyInfo.SetValue(project, value.ToString());
                    }
                }
                catch (Exception ex)
                {
                    // 记录错误但不中断整个导入过程
                    System.Diagnostics.Debug.WriteLine($"设置字段 {item.InformationFieldName} 失败: {ex.Message}");
                }
            }
        }

        #endregion

        #region 加载动态阶段信息

        /// <summary>
        /// 加载动态项目阶段信息
        /// </summary>
        private void LoadDynamicStageInfo()
        {
            if (SelectedEquipmentType == null)
            {
                DynamicStageGroups = null;
                HasDynamicContent = false;
                return;
            }

            try
            {
                using (var context = new ProjectContext())
                {
                    var equipmentTypeId = SelectedEquipmentType.EquipmentTypeId;

                    // 查询该设备类型对应的所有文档和信息
                    var docs = context.EquipTypeStageDocTable
                        .Include(d => d.ProjectStage)
                        .Include(d => d.documenttype)
                        .ThenInclude(dt => dt.FileTypesData)
                        .Where(d => d.equipmenttypeId == equipmentTypeId)
                        .OrderBy(d => d.ProjectStageId)
                        .ToList();

                    var infos = context.EquipTypeStageInfoTable
                        .Include(i => i.ProjectStage)
                        .Include(i => i.Information)
                        .ThenInclude(inf => inf.InforTypesData)
                        .Where(i => i.equipmenttypeId == equipmentTypeId)
                        .OrderBy(i => i.ProjectStageId)
                        .ToList();

                    // 按项目阶段分组
                    var stageGroups = new List<DynamicStageGroup>();

                    // 获取所有相关的项目阶段
                    var stageIds = docs.Select(d => d.ProjectStageId)
                        .Union(infos.Select(i => i.ProjectStageId))
                        .Distinct()
                        .ToList();

                    foreach (var stageId in stageIds)
                    {
                        var stage = context.ProjectStage.FirstOrDefault(s => s.ProjectStageId == stageId);
                        if (stage == null) continue;

                        var stageGroup = new DynamicStageGroup
                        {
                            StageName = stage.ProjectStageName,
                            StageId = stageId,
                            Items = new ObservableCollection<DynamicStageItem>()
                        };

                        // 添加文档项
                        var stageDocs = docs.Where(d => d.ProjectStageId == stageId).ToList();
                        foreach (var doc in stageDocs)
                        {
                            var item = new DynamicStageItem
                            {
                                Id = doc.Id,
                                Name = doc.documenttype.DocumentTypeName,
                                Type = doc.documenttype.FileTypesData?.FileTypesName ?? "文档",
                                IsAbolished = doc.Status == "Abolish",
                                IsDocument = true,
                                DocumentTypeId = doc.documenttypeId  // 保存文档类型ID
                            };
                            stageGroup.Items.Add(item);
                        }

                        // 添加信息项
                        var stageInfos = infos.Where(i => i.ProjectStageId == stageId).ToList();
                        foreach (var info in stageInfos)
                        {
                            var item = new DynamicStageItem
                            {
                                Id = info.Id,
                                Name = info.Information.Reamrks ?? info.Information.Infor,
                                Type = info.Information.InforTypesData?.FileTypesName ?? "信息-填写",
                                IsAbolished = info.Status == "Abolish",
                                IsDocument = false,
                                InformationFieldName = info.Information.Infor  // 保存字段名(对应Projects表字段)
                            };
                            stageGroup.Items.Add(item);
                        }

                        // 过滤掉"新建"阶段(与基本信息重复)
                        if (stageGroup.Items.Count > 0 && !stage.ProjectStageName.Contains("新建"))
                        {
                            stageGroups.Add(stageGroup);
                        }
                    }

                    // 按StageId排序
                    var sortedGroups = stageGroups
                        .OrderBy(g => g.StageId)
                        .ToList();

                    DynamicStageGroups = new ObservableCollection<DynamicStageGroup>(sortedGroups);
                    HasDynamicContent = sortedGroups.Count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载动态信息失败: {ex.Message}", "错误", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                HasDynamicContent = false;
            }
        }

        #endregion

        #region 动态项目阶段数据模型

        /// <summary>
        /// 动态项目阶段分组
        /// </summary>
        public class DynamicStageGroup
        {
            public string StageName { get; set; }
            public int StageId { get; set; }
            public ObservableCollection<DynamicStageItem> Items { get; set; }
        }

        /// <summary>
        /// 动态项目阶段项
        /// </summary>
        public class DynamicStageItem : ObservableObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }  // 文档, 信息-下拉框, 信息-填写, 文档-OA, 信息-日期
            public bool IsAbolished { get; set; }
            public bool IsDocument { get; set; }  // true=文档, false=信息
            public int? DocumentTypeId { get; set; }  // 文档类型ID(用于保存到ProjectDocumentStatus)
            public string InformationFieldName { get; set; }  // 信息字段名(对应Projects表字段名)

            // 数据存储
            private bool _checkboxValue;
            public bool CheckboxValue
            {
                get => _checkboxValue;
                set => SetProperty(ref _checkboxValue, value);
            }

            private string _textValue;
            public string TextValue
            {
                get => _textValue;
                set => SetProperty(ref _textValue, value);
            }

            private PeopleTable _selectedComboValue;
            public PeopleTable SelectedComboValue
            {
                get => _selectedComboValue;
                set => SetProperty(ref _selectedComboValue, value);
            }

            private DateTime? _dateValue;
            public DateTime? DateValue
            {
                get => _dateValue;
                set => SetProperty(ref _dateValue, value);
            }

            // UI控制属性
            public bool IsCheckbox => IsDocument && Type == "文档";
            public bool IsTextBox => Type == "信息-填写" || Type == "文档-OA";
            public bool IsComboBox => Type == "信息-下拉框";
            public bool IsDatePicker => Type == "信息-日期";
        }

        #endregion
    }
}
