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

namespace ProjectCycleManage.ViewModel
{
    public partial class ProjectStageVM : ObservableObject
    {
        private readonly ProjectContext _context;

        #region 项目类型相关
        [ObservableProperty]
        private ObservableCollection<EquipmentType> _equipmentTypes;

        [ObservableProperty]
        private EquipmentType _selectedEquipmentType;

        [ObservableProperty]
        private bool _isEquipmentTypeSelected;
        #endregion

        #region 项目阶段相关
        [ObservableProperty]
        private ObservableCollection<ProjectStage> _projectStages;

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
                EquipmentTypes = new ObservableCollection<EquipmentType>(
                    await _context.EquipmentType.ToListAsync());

                // 加载项目阶段
                ProjectStages = new ObservableCollection<ProjectStage>(
                    await _context.ProjectStage.ToListAsync());

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

                IsEquipmentTypeSelected = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化数据失败: {ex.Message}");
            }
        }

        #region 命令
        [RelayCommand]
        private void OpenEditModal()
        {
            if (SelectedProjectStage == null) return;
            
            ModalTitle = $"编辑 {SelectedProjectStage.ProjectStageName} 阶段文档";
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
        private void AddDocumentToStage(DocumentType document)
        {
            if (document == null || StageDocumentTypes.Contains(document)) return;
            
            StageDocumentTypes.Add(document);
            AvailableDocumentTypes.Remove(document);
        }

        [RelayCommand]
        private void RemoveDocumentFromStage(DocumentType document)
        {
            if (document == null || !StageDocumentTypes.Contains(document)) return;
            
            StageDocumentTypes.Remove(document);
            AvailableDocumentTypes.Add(document);
        }

        [RelayCommand]
        private void AddInformationToStage(InformationTable information)
        {
            if (information == null || StageInformationTypes.Contains(information)) return;
            
            StageInformationTypes.Add(information);
            AvailableInformationTypes.Remove(information);
        }

        [RelayCommand]
        private void RemoveInformationFromStage(InformationTable information)
        {
            if (information == null || !StageInformationTypes.Contains(information)) return;
            
            StageInformationTypes.Remove(information);
            AvailableInformationTypes.Add(information);
        }

        [RelayCommand]
        private void SaveConfiguration()
        {
            if (SelectedEquipmentType == null || SelectedProjectStage == null) return;
            
            try
            {
                // 保存文档配置到数据库
                SaveStageDocumentConfiguration();
                
                // 保存信息配置到数据库
                SaveStageInformationConfiguration();
                
                MessageBox.Show("配置保存成功！");
                CloseEditModal();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存配置失败: {ex.Message}");
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

                // 加载已配置的文档类型
                var configuredDocs = _context.EquipTypeStageDocTable
                    .Where(x => x.equipmenttypeId == SelectedEquipmentType.EquipmentTypeId && 
                               x.ProjectStageId == SelectedProjectStage.ProjectStageId)
                    .Include(x => x.documenttype)
                    .Select(x => x.documenttype)
                    .ToList();

                foreach (var doc in configuredDocs)
                {
                    StageDocumentTypes.Add(doc);
                    AvailableDocumentTypes.Remove(doc);
                }

                //// 加载已配置的信息类型
                //var configuredInfos = _context.EquipTypeStageInfoTable
                //    .Where(x => x.equipmenttypeId == SelectedEquipmentType.EquipmentTypeId && 
                //               x.ProjectStageId == SelectedProjectStage.ProjectStageId)
                //    .Include(x => x.information)
                //    .Select(x => x.information)
                //    .ToList();

                //foreach (var info in configuredInfos)
                //{
                //    StageInformationTypes.Add(info);
                //    AvailableInformationTypes.Remove(info);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载阶段配置失败: {ex.Message}");
            }
        }

        private void SaveStageDocumentConfiguration()
        {
            // 删除旧的配置
            var existingDocs = _context.EquipTypeStageDocTable
                .Where(x => x.equipmenttypeId == SelectedEquipmentType.EquipmentTypeId && 
                           x.ProjectStageId == SelectedProjectStage.ProjectStageId)
                .ToList();
            
            _context.EquipTypeStageDocTable.RemoveRange(existingDocs);

            // 添加新的配置
            foreach (var doc in StageDocumentTypes)
            {
                var newConfig = new EquipTypeStageDocTable
                {
                    equipmenttypeId = SelectedEquipmentType.EquipmentTypeId,
                    ProjectStageId = SelectedProjectStage.ProjectStageId,
                    documenttypeId = doc.DocumentTypeId
                };
                _context.EquipTypeStageDocTable.Add(newConfig);
            }
            
            _context.SaveChanges();
        }

        private void SaveStageInformationConfiguration()
        {
            // 删除旧的配置
            var existingInfos = _context.EquipTypeStageInfoTable
                .Where(x => x.equipmenttypeId == SelectedEquipmentType.EquipmentTypeId && 
                           x.ProjectStageId == SelectedProjectStage.ProjectStageId)
                .ToList();
            
            _context.EquipTypeStageInfoTable.RemoveRange(existingInfos);

            // 添加新的配置
            foreach (var info in StageInformationTypes)
            {
                var newConfig = new EquipTypeStageInfoTable
                {
                    equipmenttypeId = SelectedEquipmentType.EquipmentTypeId,
                    ProjectStageId = SelectedProjectStage.ProjectStageId,
                    InformationId = info.Id
                };
                _context.EquipTypeStageInfoTable.Add(newConfig);
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
    }
}
