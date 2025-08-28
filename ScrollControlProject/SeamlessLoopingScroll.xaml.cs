using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ScrollControlProject
{
    /// <summary>
    /// SeamlessLoopingScroll.xaml 的交互逻辑
    /// </summary>
    public partial class SeamlessLoopingScroll : UserControl
    {
        private readonly DispatcherTimer _timer;
        private double _scrollOffset;
        private double _itemHeight = 30; // 预估的每项高度
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
            Loaded += (s, e) => _timer.Start();
        }

        private void InitializeData()
        {
            for (int i = 1; i <= _itemCount; i++)
            {
                var textBlock = new TextBlock
                {
                    Text = $"数据条目 {i}",
                    Style = (Style)FindResource("DataItemStyle")
                };

                itemsPanel.Children.Add(textBlock);
            }

            // 复制前几个项目添加到末尾以实现无缝循环
            for (int i = 0; i < 5; i++) // 复制前5个项目
            {
                var original = itemsPanel.Children[i] as TextBlock;
                var copy = new TextBlock
                {
                    Text = original.Text,
                    Style = original.Style
                };

                itemsPanel.Children.Add(copy);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // 计算滚动位置
            _scrollOffset += 0.5; // 控制滚动速度

            // 获取实际内容高度
            double contentHeight = itemsPanel.ActualHeight;

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

    public class LinearEase : IEasingFunction
    {
        public double Ease(double normalizedTime)
        {
            return normalizedTime;
        }
    }

}
