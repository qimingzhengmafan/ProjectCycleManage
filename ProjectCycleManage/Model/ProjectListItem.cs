using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace ProjectCycleManage.Model
{
    public partial class ProjectListItem : ObservableObject
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        [ObservableProperty]
        private string _projectId;

        /// <summary>
        /// 项目名称
        /// </summary>
        [ObservableProperty]
        private string _projectName;

        /// <summary>
        /// 项目负责人
        /// </summary>
        [ObservableProperty]
        private string _projectLeader;

        /// <summary>
        /// 开始时间
        /// </summary>
        [ObservableProperty]
        private DateTime _startTime;

        /// <summary>
        /// 状态文本
        /// </summary>
        [ObservableProperty]
        private string _statusText;

        /// <summary>
        /// 状态背景色
        /// </summary>
        [ObservableProperty]
        private string _statusBackground;

        /// <summary>
        /// 状态前景色
        /// </summary>
        [ObservableProperty]
        private string _statusForeground;

        /// <summary>
        /// 是否选中
        /// </summary>
        [ObservableProperty]
        private bool _isSelected;

        /// <summary>
        /// 是否需要审批标记
        /// </summary>
        [ObservableProperty]
        private bool _needsApproval;

        /// <summary>
        /// 选择命令
        /// </summary>
        public RelayCommand SelectCommand { get; }

        /// <summary>
        /// 选择动作委托
        /// </summary>
        public Action<ProjectListItem> OnSelected { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProjectListItem(string projectId, string projectName, string projectLeader, DateTime startTime, string statusText, string statusBackground, string statusForeground, bool needsApproval = false)
        {
            ProjectId = projectId;
            ProjectName = projectName;
            ProjectLeader = projectLeader;
            StartTime = startTime;
            StatusText = statusText;
            StatusBackground = statusBackground;
            StatusForeground = statusForeground;
            IsSelected = false;
            NeedsApproval = needsApproval;
            
            SelectCommand = new RelayCommand(Select);
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ProjectListItem()
        {
            SelectCommand = new RelayCommand(Select);
        }

        /// <summary>
        /// 选择项目
        /// </summary>
        private void Select()
        {
            OnSelected?.Invoke(this);
        }
    }
}