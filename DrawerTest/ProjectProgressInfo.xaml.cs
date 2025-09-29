using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace DrawerTest
{
    /// <summary>
    /// ProjectProgressInfo.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectProgressInfo : UserControl, INotifyPropertyChanged
    {
        // 基础尺寸常量 - 固定高度
        private const double FIXED_HEIGHT = 40;
        private const double CIRCLE_DIAMETER = 30;
        private const double RECTANGLE_HEIGHT = 30;
        private const double MIN_RECT_WIDTH = 20; // 矩形最小宽度
        private const double TEXT_PADDING = 15; // 文本两侧内边距

        public ProjectProgressInfo()
        {
            InitializeComponent();

            // 设置固定高度
            this.Height = FIXED_HEIGHT;

            // 正确的数据上下文设置
            this.DataContext = this;
            this.Loaded += ProjectProgressInfo_Loaded;
        }

        private void ProjectProgressInfo_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDisplayText();
            CalculateAndUpdateLayout();
        }

        #region 依赖属性

        // 主要文本内容
        public string ContentText
        {
            get { return (string)GetValue(ContentTextProperty); }
            set { SetValue(ContentTextProperty, value); }
        }

        public static readonly DependencyProperty ContentTextProperty =
            DependencyProperty.Register("ContentText", typeof(string), typeof(ProjectProgressInfo),
            new PropertyMetadata("默认内容", OnContentTextChanged));

        // 时间信息
        public DateTime? DisplayTime
        {
            get { return (DateTime?)GetValue(DisplayTimeProperty); }
            set { SetValue(DisplayTimeProperty, value); }
        }

        public static readonly DependencyProperty DisplayTimeProperty =
            DependencyProperty.Register("DisplayTime", typeof(DateTime?), typeof(ProjectProgressInfo),
            new PropertyMetadata(null, OnDisplayTimeChanged));

        // 同步带颜色
        public Brush BeltColor
        {
            get { return (Brush)GetValue(BeltColorProperty); }
            set { SetValue(BeltColorProperty, value); }
        }

        public static readonly DependencyProperty BeltColorProperty =
            DependencyProperty.Register("BeltColor", typeof(Brush), typeof(ProjectProgressInfo),
            new PropertyMetadata(Brushes.SteelBlue, OnColorChanged));

        // 文本颜色
        public Brush TextColor
        {
            get { return (Brush)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register("TextColor", typeof(Brush), typeof(ProjectProgressInfo),
            new PropertyMetadata(Brushes.White, OnColorChanged));

        // 固定宽度（可选，如果设置为0则自适应）
        public double FixedWidth
        {
            get { return (double)GetValue(FixedWidthProperty); }
            set { SetValue(FixedWidthProperty, value); }
        }

        public static readonly DependencyProperty FixedWidthProperty =
            DependencyProperty.Register("FixedWidth", typeof(double), typeof(ProjectProgressInfo),
            new PropertyMetadata(0.0, OnLayoutChanged));

        #endregion

        #region 计算属性

        // 显示的完整文本（内部使用）
        public string DisplayText
        {
            get { return _displayText; }
            set
            {
                if (_displayText != value)
                {
                    _displayText = value;
                    OnPropertyChanged(nameof(DisplayText));
                }
            }
        }
        private string _displayText;

        #endregion

        #region 事件处理

        private static void OnContentTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ProjectProgressInfo;
            control?.UpdateDisplayText();
        }

        private static void OnDisplayTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ProjectProgressInfo;
            control?.UpdateDisplayText();
        }

        private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ProjectProgressInfo;
            control?.UpdateVisual();
        }

        private static void OnLayoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ProjectProgressInfo;
            control?.CalculateAndUpdateLayout();
        }

        // TextBlock加载完成时计算文本宽度
        private void ContentTextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            CalculateAndUpdateLayout();
        }

        // TextBlock尺寸变化时重新布局
        private void ContentTextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
            {
                CalculateAndUpdateLayout();
            }
        }

        #endregion

        #region 布局计算方法

        private void UpdateDisplayText()
        {
            string newText;
            if (DisplayTime.HasValue)
            {
                newText = $"{ContentText}\n{DisplayTime.Value:yyyy-MM-dd HH:mm:ss}";
            }
            else
            {
                newText = ContentText;
            }

            // 只有当文本实际改变时才更新
            if (DisplayText != newText)
            {
                DisplayText = newText;

                // 文本更新后重新计算布局
                Dispatcher.BeginInvoke(new Action(CalculateAndUpdateLayout),
                    DispatcherPriority.Loaded);
            }
        }

        private void UpdateVisual()
        {
            // 可以在这里添加额外的视觉更新逻辑
            OnPropertyChanged(nameof(BeltColor));
            OnPropertyChanged(nameof(TextColor));
        }

        private void CalculateAndUpdateLayout()
        {
            if (ContentTextBlock == null || MainCanvas == null || !IsLoaded)
                return;

            try
            {
                // 强制Measure以确保获取正确的文本尺寸
                ContentTextBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                double textWidth = ContentTextBlock.DesiredSize.Width;

                // 计算矩形宽度（文本宽度 + 两侧内边距）
                double rectWidth = Math.Max(textWidth + TEXT_PADDING * 2, MIN_RECT_WIDTH);

                // 如果设置了固定宽度，使用固定宽度
                if (FixedWidth > 0)
                {
                    rectWidth = FixedWidth - CIRCLE_DIAMETER; // 减去两个圆的直径
                }

                // 更新矩形尺寸
                if (MiddleRect != null)
                {
                    MiddleRect.Width = rectWidth;
                }

                // 更新右圆位置（左圆位置固定）
                if (RightCircle != null)
                {
                    Canvas.SetLeft(RightCircle, 20 + rectWidth - 15); // 20是左圆位置+偏移
                }

                // 更新文本区域
                if (TextBorder != null)
                {
                    TextBorder.Width = rectWidth;
                }

                // 更新Canvas宽度（高度固定为40）
                MainCanvas.Width = 20 + rectWidth + 5; // 左圆位置 + 矩形宽度 + 右圆偏移

                // 更新UserControl的宽度，使其能够自适应
                this.Width = MainCanvas.Width;
            }
            catch (Exception ex)
            {
                // 调试用：输出错误信息
                System.Diagnostics.Debug.WriteLine($"布局计算错误: {ex.Message}");
            }
        }

        #endregion

        #region 公共方法

        // 便捷方法：设置颜色主题
        public void SetColorTheme(Color beltColor)
        {
            BeltColor = new SolidColorBrush(beltColor);

            // 根据背景色自动计算文本颜色（确保可读性）
            var luminance = (0.299 * beltColor.R + 0.587 * beltColor.G + 0.114 * beltColor.B) / 255;
            TextColor = luminance > 0.5 ? Brushes.Black : Brushes.White;
        }

        // 强制刷新布局（在外部内容变化后调用）
        public void RefreshLayout()
        {
            CalculateAndUpdateLayout();
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
