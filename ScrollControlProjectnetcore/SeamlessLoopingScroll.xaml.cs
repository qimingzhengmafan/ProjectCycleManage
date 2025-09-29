using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Threading;

namespace ScrollControlProjectnetcore
{
    /// <summary>
    /// SeamlessLoopingScroll.xaml 的交互逻辑
    /// </summary>
    public partial class SeamlessLoopingScroll : UserControl
    {
        private readonly DispatcherTimer _timer;
        private double _scrollOffset;
        private double _itemHeight = 40; // 根据新模板调整项目高度
        private int _itemCount = 15;

        public SeamlessLoopingScroll()
        {
            InitializeComponent();

            // 初始化数据
            InitializeData();

            // 设置定时器实现自动滚动
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(20);
            _timer.Tick += Timer_Tick;
            Loaded += (s, e) =>
            {
                InitializeItemsControl();
                _timer.Start();
            };
        }

        public class ProjectItem : INotifyPropertyChanged
        {
            private Brush _statusColor = Brushes.LimeGreen;
            private bool _isTimeout;
            public bool IsTimeout
            {
                get => _isTimeout;
                set
                {
                    _isTimeout = value;
                    OnPropertyChanged(nameof(IsTimeout));
                    StatusColor = value ? Brushes.Red : Brushes.LimeGreen;
                }
            }
            private string _projectName;
            private int _progress;
            private string _materials;
            private string _owner;

            public event PropertyChangedEventHandler PropertyChanged;

            public Brush StatusColor
            {
                get => _statusColor;
                set { _statusColor = value; OnPropertyChanged(nameof(StatusColor)); }
            }

            public string ProjectName
            {
                get => _projectName;
                set { _projectName = value; OnPropertyChanged(); }
            }

            public int Progress
            {
                get => _progress;
                set { _progress = value; OnPropertyChanged(); }
            }

            public string Materials
            {
                get => _materials;
                set { _materials = value; OnPropertyChanged(); }
            }

            public string Owner
            {
                get => _owner;
                set { _owner = value; OnPropertyChanged(); }
            }

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource",
            typeof(IEnumerable<ProjectItem>), // 使用具体泛型类型 
            typeof(SeamlessLoopingScroll),
            new PropertyMetadata(null, OnItemsSourceChanged));

        public IEnumerable<ProjectItem> ItemsSource
        {
            get => (IEnumerable<ProjectItem>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty ScrollSpeedProperty = DependencyProperty.Register(
            "ScrollSpeed",
            typeof(double),
            typeof(SeamlessLoopingScroll),
            new PropertyMetadata(0.5));

        public double ScrollSpeed
        {
            get => (double)GetValue(ScrollSpeedProperty);
            set => SetValue(ScrollSpeedProperty, value);
        }

        public static readonly DependencyProperty IsAutoScrollEnabledProperty = DependencyProperty.Register(
            "IsAutoScrollEnabled",
            typeof(bool),
            typeof(SeamlessLoopingScroll),
            new PropertyMetadata(true, OnAutoScrollChanged));

        private static void OnAutoScrollChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (SeamlessLoopingScroll)d;
            control._timer.IsEnabled = (bool)e.NewValue;
        }

        public bool IsAutoScrollEnabled
        {
            get => (bool)GetValue(IsAutoScrollEnabledProperty);
            set => SetValue(IsAutoScrollEnabledProperty, value);
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (SeamlessLoopingScroll)d;
            control.InitializeData();
        }

        private ObservableCollection<ProjectItem> _projects = new ObservableCollection<ProjectItem>();
        // 初始化数据控件
        private void InitializeItemsControl()
        {
            itemsControl.ItemsSource = _projects;
            itemsControl.Items.Refresh();
        }

        private void InitializeData()
        {
            // 清空现有数据
            _projects.Clear();

            // 初始化ItemsControl
            if (Resources.Contains("ProjectItemTemplate"))
            {
                itemsControl.ItemTemplate = (DataTemplate)FindResource("ProjectItemTemplate");
            }

            // 加载外部数据源
            if (ItemsSource != null)
            {
                foreach (var item in ItemsSource)
                {
                    _projects.Add(item);
                }
            }
            else
            {
                // 生成示例数据
                for (int i = 1; i <= 15; i++)
                {
                    _projects.Add(new ProjectItem
                    {
                        ProjectName = $"项目{i}：生产线优化",
                        Progress = new Random().Next(20, 100),
                        Materials = $"图纸{(i % 3 + 1)} 规范{(i % 3 + 1)}",
                        Owner = $"{(char)(65 + i % 5)}"
                    });
                }
            }

            // 实现循环需要的重复项
            for (int i = 0; i < 5; i++)
            {
                _projects.Add(_projects[i]);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // 计算滚动位置
            _scrollOffset += ScrollSpeed; // 使用依赖属性控制滚动速度

            // 获取实际内容高度
            double contentHeight = itemsControl.ActualHeight;

            // 获取可视区域高度
            double viewportHeight = scroller.ViewportHeight;

            // 如果滚动到末尾附近，重置到开头
            if (_scrollOffset > contentHeight - viewportHeight - _itemHeight)
            {
                _scrollOffset = 0;
            }

            scroller.ScrollToVerticalOffset(_scrollOffset);
        }
    }
}
