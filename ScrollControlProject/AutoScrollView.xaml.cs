using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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

namespace ScrollControlProject
{
    /// <summary>
    /// AutoScrollView.xaml 的交互逻辑
    /// </summary>
    public partial class AutoScrollView : UserControl
    {
        // 定时器控制滚动
        private readonly DispatcherTimer _scrollTimer = new DispatcherTimer();
        // 滚动速度（像素/步）
        private double _scrollSpeed = 2;
        // 当前滚动位置
        private double _currentOffset = 0;
        // 列表项高度
        private const double ITEM_HEIGHT = 60;

        // 数据集合
        public ObservableCollection<ScrollItem> Items { get; set; }

        public AutoScrollView()
        {
            InitializeComponent();

            // 初始化数据
            Items = new ObservableCollection<ScrollItem>();
            for (int i = 1; i <= 10; i++)
            {
                Items.Add(new ScrollItem
                {
                    Content = $"这是第 {i} 条数据",
                    Background = i % 2 == 0 ? Brushes.LightGray : Brushes.White
                });
            }

            DataContext = this;

            // 配置定时器
            _scrollTimer.Interval = TimeSpan.FromMilliseconds(30);
            _scrollTimer.Tick += ScrollTimer_Tick;
        }

        private void ScrollTimer_Tick(object sender, EventArgs e)
        {
            if (scrollList.Items.Count == 0) return;

            // 获取ScrollViewer
            var scrollViewer = GetScrollViewer(scrollList);
            if (scrollViewer == null) return;

            // 计算最大滚动距离
            double maxOffset = scrollViewer.ScrollableHeight;

            // 增加偏移量
            _currentOffset += _scrollSpeed * (speedSlider.Value / 10);

            // 循环处理：当滚动到底部时，回到顶部
            if (_currentOffset >= maxOffset)
            {
                // 计算需要滚动的项数
                int itemsToScroll = (int)(_currentOffset / ITEM_HEIGHT) + 1;
                int newIndex = itemsToScroll % Items.Count;

                // 平滑过渡到下一个循环点
                _currentOffset = newIndex * ITEM_HEIGHT;
            }

            // 设置滚动位置
            scrollViewer.ScrollToVerticalOffset(_currentOffset);
        }

        // 获取ListBox中的ScrollViewer
        private ScrollViewer GetScrollViewer(DependencyObject element)
        {
            if (element is ScrollViewer)
                return (ScrollViewer)element;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                var result = GetScrollViewer(child);
                if (result != null)
                    return result;
            }
            return null;
        }

        private void StartScroll_Click(object sender, RoutedEventArgs e)
        {
            _scrollTimer.Start();
        }

        private void StopScroll_Click(object sender, RoutedEventArgs e)
        {
            _scrollTimer.Stop();
        }
    }

    // 数据项模型
    public class ScrollItem
    {
        public string Content { get; set; }
        public Brush Background { get; set; }
    }
}
