using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectManagement.Data;
using ProjectManagement.Models;
using ProjectCycleManage.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;

namespace ProjectCycleManage.ViewModel
{
    /// <summary>
    /// 设备类型显示模型
    /// </summary>
    public class EquipmentTypeDisplayModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private EquipmentType _equipmentType;
        private bool _isSelected;

        public EquipmentType EquipmentType
        {
            get => _equipmentType;
            set
            {
                _equipmentType = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EquipmentType)));
            }
        }

        public int EquipmentTypeId => _equipmentType?.EquipmentTypeId ?? 0;
        public string EquipmentTypeName => _equipmentType?.EquipmentName ?? string.Empty;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }

        public EquipmentTypeDisplayModel(EquipmentType equipmentType)
        {
            _equipmentType = equipmentType;
            _isSelected = false;
        }
    }

    /// <summary>
    /// 项目阶段显示模型，用于展示阶段和文档信息
    /// </summary>
    public class ProjectStageDisplayModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ProjectStage _stage;
        private int _documentCount;
        private int _informationCount;
        private ObservableCollection<DocumentDisplayModel> _documents;

        public ProjectStage Stage
        {
            get => _stage;
            set
            {
                _stage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Stage)));
            }
        }

        public int ProjectStageId => _stage?.ProjectStageId ?? 0;
        public string ProjectStageName => _stage?.ProjectStageName ?? string.Empty;
        public int ProjectProgress => _stage?.ProjectProgress ?? 0;

        public int DocumentCount
        {
            get => _documentCount;
            set
            {
                _documentCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DocumentCount)));
            }
        }

        public int InformationCount
        {
            get => _informationCount;
            set
            {
                _informationCount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InformationCount)));
            }
        }

        public ObservableCollection<DocumentDisplayModel> Documents
        {
            get => _documents;
            set
            {
                _documents = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Documents)));
            }
        }

        public ProjectStageDisplayModel(ProjectStage stage)
        {
            _stage = stage;
            _documents = new ObservableCollection<DocumentDisplayModel>();
        }
    }

    /// <summary>
    /// 文档显示模型
    /// </summary>
    public class DocumentDisplayModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
    public partial class ProjectStageVM : ObservableObject
    {
        private readonly ProjectContext _context;

        #region 项目类型相关
        [ObservableProperty]
        private ObservableCollection<EquipmentTypeDisplayModel> _equipmentTypes;

        [ObservableProperty]
        private EquipmentType _selectedEquipmentType;

        [ObservableProperty]
        private bool _isEquipmentTypeSelected;
        #endregion

        #region 项目阶段相关
        [ObservableProperty]
        private ObservableCollection<ProjectStageDisplayModel> _projectStages;

        [ObservableProperty]
        private ProjectStage _selectedProjectStage;
        #endregion

        #region 文档类型相关
        [ObservableProperty]
        private ObservableCollection<DocumentType> _allDocumentTypes;

        [ObservableProperty]
        private ObservableCollection<DocumentType> _availableDocumentTypes;

        [ObservableProperty]
        private ObservableCollection<DocumentType> _stageDocumentTypes;

        [ObservableProperty]
        private string _documentSearchText;
        #endregion

        #region 信息类型相关
        [ObservableProperty]
        private ObservableCollection<InformationTable> _allInformationTypes;

        [ObservableProperty]
        private ObservableCollection<InformationTable> _availableInformationTypes;

        [ObservableProperty]
        private ObservableCollection<InformationTable> _stageInformationTypes;

        [ObservableProperty]
        private string _informationSearchText;
        #endregion

        #region 模态框控制
        [ObservableProperty]
        private bool _isEditModalOpen;

        [ObservableProperty]
        private string _modalTitle;
        #endregion

        public ProjectStageVM()
        {
            _context = new ProjectContext();
            InitializeData();
        }

        private async void InitializeData()
        {
            try
            {
                // 加载设备类型
                var equipmentTypes = await _context.EquipmentType.ToListAsync();
                EquipmentTypes = new ObservableCollection<EquipmentTypeDisplayModel>();
                
                foreach (var type in equipmentTypes)
                {
                    var displayModel = new EquipmentTypeDisplayModel(type);
                    EquipmentTypes.Add(displayModel);
                }

                // 加载项目阶段（按ID降序排列，最新的排在最前面）
                var stages = await _context.ProjectStage
                    .OrderByDescending(s => s.ProjectStageId)
                    .ToListAsync();
                ProjectStages = new ObservableCollection<ProjectStageDisplayModel>();
                
                foreach (var stage in stages)
                {
                    var displayModel = new ProjectStageDisplayModel(stage);
                    ProjectStages.Add(displayModel);
                }

                // 加载所有文档类型
                AllDocumentTypes = new ObservableCollection<DocumentType>(
                    await _context.DocumentType.ToListAsync());

                // 加载所有信息类型
                AllInformationTypes = new ObservableCollection<InformationTable>(
                    await _context.InformationTable.ToListAsync());

                // 初始化可用列表和阶段列表
                AvailableDocumentTypes = new ObservableCollection<DocumentType>(AllDocumentTypes);
                StageDocumentTypes = new ObservableCollection<DocumentType>();
                
                AvailableInformationTypes = new ObservableCollection<InformationTable>(AllInformationTypes);
                StageInformationTypes = new ObservableCollection<InformationTable>();

                // 默认选中第一个设备类型
                if (EquipmentTypes.Count > 0)
                {
                    var firstType = EquipmentTypes[0];
                    firstType.IsSelected = true;
                    SelectedEquipmentType = firstType.EquipmentType;
                    // IsEquipmentTypeSelected会在OnSelectedEquipmentTypeChanged中自动设置
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化数据失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region 命令
        [RelayCommand]
        private void SelectEquipmentType(object parameter)
        {
            if (parameter is EquipmentTypeDisplayModel selectedType)
            {
                // 取消所有其他选择
                foreach (var type in EquipmentTypes)
                {
                    type.IsSelected = false;
                }
                
                // 选中当前项
                selectedType.IsSelected = true;
                SelectedEquipmentType = selectedType.EquipmentType;
            }
        }

        [RelayCommand]
        private void OpenEditModal(ProjectStage stage)
        {
            if (stage == null || SelectedEquipmentType == null) return;
            
            SelectedProjectStage = stage;
            ModalTitle = $"{stage.ProjectStageName} - 文档与信息配置";
            IsEditModalOpen = true;
            
            // 加载当前阶段的文档配置
            LoadStageConfiguration();
        }

        [RelayCommand]
        private void CloseEditModal()
        {
            IsEditModalOpen = false;
            ClearSearch();
        }

        [RelayCommand]
        private void AddDocumentToStage(object parameter)
        {
            if (parameter == null) return;
            
            // 支持从ListBox的SelectedItem传入
            if (parameter is DocumentType document)
            {
                if (!StageDocumentTypes.Contains(document))
                {
                    StageDocumentTypes.Add(document);
                    AvailableDocumentTypes.Remove(document);
                }
            }
        }

        [RelayCommand]
        private void RemoveDocumentFromStage(object parameter)
        {
            if (parameter == null) return;
            
            // 支持从ListBox的SelectedItem传入
            if (parameter is DocumentType document)
            {
                if (StageDocumentTypes.Contains(document))
                {
                    StageDocumentTypes.Remove(document);
                    AvailableDocumentTypes.Add(document);
                }
            }
        }

        [RelayCommand]
        private void AddInformationToStage(object parameter)
        {
            if (parameter == null) return;
            
            // 支持从ListBox的SelectedItem传入
            if (parameter is InformationTable information)
            {
                if (!StageInformationTypes.Contains(information))
                {
                    StageInformationTypes.Add(information);
                    AvailableInformationTypes.Remove(information);
                }
            }
        }

        [RelayCommand]
        private void RemoveInformationFromStage(object parameter)
        {
            if (parameter == null) return;
            
            // 支持从ListBox的SelectedItem传入
            if (parameter is InformationTable information)
            {
                if (StageInformationTypes.Contains(information))
                {
                    StageInformationTypes.Remove(information);
                    AvailableInformationTypes.Add(information);
                }
            }
        }

        [RelayCommand]
        private async Task SaveConfiguration()
        {
            if (SelectedEquipmentType == null || SelectedProjectStage == null) return;
            
            try
            {
                // 保存文档配置到数据库
                await Task.Run(() => SaveStageDocumentConfiguration());
                
                // 保存信息配置到数据库
                await Task.Run(() => SaveStageInformationConfiguration());
                
                MessageBox.Show("阶段配置已保存！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // 刷新主页面的阶段文档卡片显示
                LoadStageDocumentCounts();
                
                CloseEditModal();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存配置失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region 私有方法
        private void LoadStageConfiguration()
        {
            if (SelectedEquipmentType == null || SelectedProjectStage == null) return;
            
            try
            {
                // 重置列表
                AvailableDocumentTypes = new ObservableCollection<DocumentType>(AllDocumentTypes);
                StageDocumentTypes = new ObservableCollection<DocumentType>();
                AvailableInformationTypes = new ObservableCollection<InformationTable>(AllInformationTypes);
                StageInformationTypes = new ObservableCollection<InformationTable>();

                // 加载已配置的文档类型（只加载状态为Nece的）
                var configuredDocs = _context.EquipTypeStageDocTable
                    .Where(x => x.equipmenttypeId == SelectedEquipmentType.EquipmentTypeId && 
                               x.ProjectStageId == SelectedProjectStage.ProjectStageId &&
                               x.Status == "Nece")
                    .Include(x => x.documenttype)
                    .Select(x => x.documenttype)
                    .ToList();

                foreach (var doc in configuredDocs)
                {
                    StageDocumentTypes.Add(doc);
                    AvailableDocumentTypes.Remove(doc);
                }

                // 加载已配置的信息类型（只加载状态为Nece的）
                var configuredInfos = _context.EquipTypeStageInfoTable
                    .Where(x => x.equipmenttypeId == SelectedEquipmentType.EquipmentTypeId && 
                               x.ProjectStageId == SelectedProjectStage.ProjectStageId &&
                               x.Status == "Nece")
                    .Include(x => x.Information)
                    .Select(x => x.Information)
                    .ToList();

                foreach (var info in configuredInfos)
                {
                    StageInformationTypes.Add(info);
                    AvailableInformationTypes.Remove(info);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载阶段配置失败: {ex.Message}");
            }
        }

        private void SaveStageDocumentConfiguration()
        {
            // 获取当前所有文档配置（包括已废除的）
            var allExistingDocs = _context.EquipTypeStageDocTable
                .Where(x => x.equipmenttypeId == SelectedEquipmentType.EquipmentTypeId && 
                           x.ProjectStageId == SelectedProjectStage.ProjectStageId)
                .ToList();

            // 处理当前选中的文档
            foreach (var doc in StageDocumentTypes)
            {
                // 检查数据库是否已存在该记录
                var existing = allExistingDocs.FirstOrDefault(x => x.documenttypeId == doc.DocumentTypeId);
                
                if (existing != null)
                {
                    // 如果存在，更新状态为Nece
                    existing.Status = "Nece";
                }
                else
                {
                    // 如果不存在，创建新记录并设置状态为Nece
                    var newConfig = new EquipTypeStageDocTable
                    {
                        equipmenttypeId = SelectedEquipmentType.EquipmentTypeId,
                        ProjectStageId = SelectedProjectStage.ProjectStageId,
                        documenttypeId = doc.DocumentTypeId,
                        Status = "Nece"
                    };
                    _context.EquipTypeStageDocTable.Add(newConfig);
                }
            }

            // 处理被移除的文档（标记为Abolish）
            var selectedDocIds = StageDocumentTypes.Select(d => d.DocumentTypeId).ToList();
            var removedDocs = allExistingDocs.Where(x => !selectedDocIds.Contains(x.documenttypeId));
            
            foreach (var removed in removedDocs)
            {
                removed.Status = "Abolish";
            }
            
            _context.SaveChanges();
        }

        private void SaveStageInformationConfiguration()
        {
            // 获取当前所有信息配置（包括已废除的）
            var allExistingInfos = _context.EquipTypeStageInfoTable
                .Where(x => x.equipmenttypeId == SelectedEquipmentType.EquipmentTypeId && 
                           x.ProjectStageId == SelectedProjectStage.ProjectStageId)
                .ToList();

            // 处理当前选中的信息
            foreach (var info in StageInformationTypes)
            {
                // 检查数据库是否已存在该记录
                var existing = allExistingInfos.FirstOrDefault(x => x.InformationId == info.Id);
                
                if (existing != null)
                {
                    // 如果存在，更新状态为Nece
                    existing.Status = "Nece";
                }
                else
                {
                    // 如果不存在，创建新记录并设置状态为Nece
                    var newConfig = new EquipTypeStageInfoTable
                    {
                        equipmenttypeId = SelectedEquipmentType.EquipmentTypeId,
                        ProjectStageId = SelectedProjectStage.ProjectStageId,
                        InformationId = info.Id,
                        Status = "Nece"
                    };
                    _context.EquipTypeStageInfoTable.Add(newConfig);
                }
            }

            // 处理被移除的信息（标记为Abolish）
            var selectedInfoIds = StageInformationTypes.Select(i => i.Id).ToList();
            var removedInfos = allExistingInfos.Where(x => !selectedInfoIds.Contains(x.InformationId));
            
            foreach (var removed in removedInfos)
            {
                removed.Status = "Abolish";
            }
            
            _context.SaveChanges();
        }

        private void ClearSearch()
        {
            DocumentSearchText = string.Empty;
            InformationSearchText = string.Empty;
        }
        #endregion

        partial void OnSelectedEquipmentTypeChanged(EquipmentType value)
        {
            IsEquipmentTypeSelected = value != null;
            
            if (value != null)
            {
                // 当选择设备类型时，加载每个阶段的文档数量
                LoadStageDocumentCounts();
            }
        }

        partial void OnDocumentSearchTextChanged(string value)
        {
            FilterDocumentTypes();
        }

        partial void OnInformationSearchTextChanged(string value)
        {
            FilterInformationTypes();
        }

        private void FilterDocumentTypes()
        {
            if (string.IsNullOrWhiteSpace(DocumentSearchText))
            {
                AvailableDocumentTypes = new ObservableCollection<DocumentType>(AllDocumentTypes.Except(StageDocumentTypes));
            }
            else
            {
                var filtered = AllDocumentTypes
                    .Where(d => d.DocumentTypeName.Contains(DocumentSearchText, StringComparison.OrdinalIgnoreCase))
                    .Except(StageDocumentTypes);
                AvailableDocumentTypes = new ObservableCollection<DocumentType>(filtered);
            }
        }

        private void FilterInformationTypes()
        {
            if (string.IsNullOrWhiteSpace(InformationSearchText))
            {
                AvailableInformationTypes = new ObservableCollection<InformationTable>(AllInformationTypes.Except(StageInformationTypes));
            }
            else
            {
                var filtered = AllInformationTypes
                    .Where(i => i.Infor.Contains(InformationSearchText, StringComparison.OrdinalIgnoreCase))
                    .Except(StageInformationTypes);
                AvailableInformationTypes = new ObservableCollection<InformationTable>(filtered);
            }
        }

        /// <summary>
        /// 加载每个阶段的文档数量
        /// </summary>
        private void LoadStageDocumentCounts()
        {
            if (SelectedEquipmentType == null || ProjectStages == null) return;

            try
            {
                foreach (var stageDisplay in ProjectStages)
                {
                    // 查询该阶段已配置的文档数量（只统计状态为Nece的）
                    var docCount = _context.EquipTypeStageDocTable
                        .Count(x => x.equipmenttypeId == SelectedEquipmentType.EquipmentTypeId &&
                                   x.ProjectStageId == stageDisplay.ProjectStageId &&
                                   x.Status == "Nece");

                    var infoCount = _context.EquipTypeStageInfoTable
                        .Count(x => x.equipmenttypeId == SelectedEquipmentType.EquipmentTypeId &&
                                   x.ProjectStageId == stageDisplay.ProjectStageId &&
                                   x.Status == "Nece");

                    stageDisplay.DocumentCount = docCount + infoCount;
                    stageDisplay.InformationCount = infoCount;

                    // 加载预览文档列表（最多3个，只加载状态为Nece的）
                    var docs = _context.EquipTypeStageDocTable
                        .Where(x => x.equipmenttypeId == SelectedEquipmentType.EquipmentTypeId &&
                                   x.ProjectStageId == stageDisplay.ProjectStageId &&
                                   x.Status == "Nece")
                        .Include(x => x.documenttype)
                        .Take(3)
                        .Select(x => new DocumentDisplayModel
                        {
                            Name = x.documenttype.DocumentTypeName,
                            Description = "文档模板",
                            Icon = "📄"
                        })
                        .ToList();

                    // 如果文档不足3个，用信息填充
                    var remainingCount = 3 - docs.Count;
                    if (remainingCount > 0)
                    {
                        var infos = _context.EquipTypeStageInfoTable
                            .Where(x => x.equipmenttypeId == SelectedEquipmentType.EquipmentTypeId &&
                                       x.ProjectStageId == stageDisplay.ProjectStageId &&
                                       x.Status == "Nece")
                            .Include(x => x.Information)
                            .Take(remainingCount)
                            .Select(x => new DocumentDisplayModel
                            {
                                Name = x.Information.Infor,
                                Description = x.Information.Reamrks ?? "信息模板",
                                Icon = "ℹ️"
                            })
                            .ToList();
                        
                        docs.AddRange(infos);
                    }

                    stageDisplay.Documents = new ObservableCollection<DocumentDisplayModel>(docs);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载阶段文档数量失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
