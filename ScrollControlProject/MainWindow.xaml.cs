using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScrollControlProject
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<FolderItem> RootFolders { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            InitializeDemoData();

            RootFolders = new ObservableCollection<FolderItem>();
            DataContext = this;
            LoadFolderStructure();
        }

        private void LoadFolderStructure()
        {
            // 明确指定要加载的目录
            string rootPath = @"C:\LINQPadv8411"; // 或者使用环境变量等

            try
            {
                var rootFolder = new FolderItem(rootPath) { IsExpanded = true };
                LoadSubFolders(rootFolder, rootPath);
                RootFolders.Add(rootFolder);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载目录失败: {ex.Message}");
            }
        }

        private void LoadSubFolders(FolderItem parent, string parentPath)
        {
            try
            {
                // 明确使用 System.IO.Directory
                var directories = System.IO.Directory.GetDirectories(parentPath);

                foreach (var directoryPath in directories)
                {
                    // 这里的 directoryPath 是完整的目录路径
                    var folderItem = new FolderItem(directoryPath);
                    parent.Children.Add(folderItem);

                    // 可以在这里递归加载，或者使用懒加载
                    // LoadSubFolders(folderItem, directoryPath); // 取消注释可立即加载所有子目录
                }
            }
            catch (UnauthorizedAccessException)
            {
                // 无权限访问的目录
            }
            catch (Exception ex)
            {
                // 其他异常处理
            }
        }

        private void InitializeDemoData()
        {
            var demoItems = new ObservableCollection<SeamlessLoopingScroll.ProjectItem>
            {
                new SeamlessLoopingScroll.ProjectItem
                {
                    ProjectName = "紧急：产线升级",
                    Progress = 30,
                    Materials = "图纸001",
                    Owner = "负责人：A"
                },
                new SeamlessLoopingScroll.ProjectItem
                {
                    ProjectName = "常规：设备维护",
                    Progress = 65,
                    Materials = "手册002",
                    Owner = "负责人：B"
                },
                new SeamlessLoopingScroll.ProjectItem()
                {
                    IsTimeout = true,
                    ProjectName = "紧急：产线升级",
                    Progress = 30,
                    Materials = "图纸1 规范2",
                    Owner = "负责人：C"
                },
                new SeamlessLoopingScroll.ProjectItem
                {
                    IsTimeout = true,
                    ProjectName = "紧急：产线升级",
                    Progress = 30,
                    Materials = "图纸1 规范2",
                    Owner = "负责人：D"
                },
                new SeamlessLoopingScroll.ProjectItem
                {
                    IsTimeout = true,
                    ProjectName = "紧急：产线升级",
                    Progress = 30,
                    Materials = "图纸1 规范2",
                    Owner = "负责人：E"
                },
                new SeamlessLoopingScroll.ProjectItem
                {
                    IsTimeout = true,
                    ProjectName = "紧急：产线升级",
                    Progress = 30,
                    Materials = "图纸1 规范2",
                    Owner = "负责人：F"
                },
                new SeamlessLoopingScroll.ProjectItem
                {
                    IsTimeout = true,
                    ProjectName = "紧急：产线升级",
                    Progress = 30,
                    Materials = "图纸1 规范2",
                    Owner = "负责人：G"
                },
                new SeamlessLoopingScroll.ProjectItem
                {
                    IsTimeout = true,
                    ProjectName = "紧急：产线升级",
                    Progress = 30,
                    Materials = "图纸1 规范2",
                    Owner = "负责人：H"
                },
                new SeamlessLoopingScroll.ProjectItem
                {
                    IsTimeout = true,
                    ProjectName = "紧急：产线升级",
                    Progress = 30,
                    Materials = "图纸1 规范2",
                    Owner = "负责人：I"
                },
                new SeamlessLoopingScroll.ProjectItem
                {
                    IsTimeout = true,
                    ProjectName = "紧急：产线升级",
                    Progress = 30,
                    Materials = "图纸1 规范2",
                    Owner = "负责人：J"
                },
                new SeamlessLoopingScroll.ProjectItem
                {
                    IsTimeout = true,
                    ProjectName = "紧急：产线升级",
                    Progress = 30,
                    Materials = "图纸1 规范2",
                    Owner = "负责人：K"
                }
            };

            scrollControl.ItemsSource = demoItems;
            scrollControl.UpdateLayout();
            scrollControl.ScrollSpeed = 0.8;



        }

    }


    // 数据模型
    public class FolderItem : INotifyPropertyChanged
    {
        private string _name;
        private string _fullPath;
        private bool _isExpanded;
        private ObservableCollection<FolderItem> _children;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public string FullPath
        {
            get => _fullPath;
            set { _fullPath = value; OnPropertyChanged(nameof(FullPath)); }
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set { _isExpanded = value; OnPropertyChanged(nameof(IsExpanded)); }
        }

        public ObservableCollection<FolderItem> Children
        {
            get => _children;
            set { _children = value; OnPropertyChanged(nameof(Children)); }
        }

        public FolderItem(string path)
        {
            FullPath = path;
            // 明确使用 System.IO.Path
            Name = System.IO.Path.GetFileName(path);
            // 处理根目录情况（如 C:\ 会返回空名称）
            if (string.IsNullOrEmpty(Name))
                Name = path;

            Children = new ObservableCollection<FolderItem>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class FolderItem1 : INotifyPropertyChanged
    {
        private string _name;
        private string _fullPath;
        private bool _isExpanded;
        private ObservableCollection<FolderItem> _children;
        private bool _hasChildren;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public string FullPath
        {
            get => _fullPath;
            set { _fullPath = value; OnPropertyChanged(nameof(FullPath)); }
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set { _isExpanded = value; OnPropertyChanged(nameof(IsExpanded)); }
        }

        public ObservableCollection<FolderItem> Children
        {
            get => _children;
            set
            {
                _children = value;
                OnPropertyChanged(nameof(Children));
                // 当Children变化时，更新HasChildren
                UpdateHasChildren();
            }
        }

        // HasChildren 属性 - 用于控制文件夹图标的显示
        public bool HasChildren
        {
            get => _hasChildren;
            set
            {
                _hasChildren = value;
                OnPropertyChanged(nameof(HasChildren));
            }
        }

        public FolderItem1(string path)
        {
            FullPath = path;
            Name = System.IO.Path.GetFileName(path);
            if (string.IsNullOrEmpty(Name))
                Name = path;

            Children = new ObservableCollection<FolderItem>();

            // 初始化时检查是否有子文件夹
            CheckIfHasChildren();
        }

        // 检查是否有子文件夹的方法
        private void CheckIfHasChildren()
        {
            try
            {
                // 只检查是否存在子目录，不实际加载所有子项（提高性能）
                var hasSubDirs = Directory.EnumerateDirectories(FullPath).Any();
                HasChildren = hasSubDirs;
            }
            catch (UnauthorizedAccessException)
            {
                HasChildren = false; // 无权限访问，假设没有子文件夹
            }
            catch (Exception)
            {
                HasChildren = false; // 其他异常，假设没有子文件夹
            }
        }

        // 当Children集合变化时更新HasChildren
        private void UpdateHasChildren()
        {
            HasChildren = _children?.Count > 0;
        }

        // 加载子文件夹的方法
        public void LoadChildren()
        {
            try
            {
                var directories = Directory.GetDirectories(FullPath);
                foreach (var dirPath in directories)
                {
                    var child = new FolderItem(dirPath);
                    Children.Add(child);
                }
                // 加载后更新HasChildren状态
                UpdateHasChildren();
            }
            catch (Exception ex)
            {
                // 处理异常
                System.Diagnostics.Debug.WriteLine($"加载子文件夹失败: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
