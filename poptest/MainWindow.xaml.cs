using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace poptest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        // 在MainWindow.xaml.cs中
        private void ShowToast(string title, string message, int duration = 3000)
        {
            ToastTitle.Text = title;
            ToastMessage.Text = message;

            ToastPopup.IsOpen = true;

            // 自动关闭
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(duration) };
            timer.Tick += (sender, args) =>
            {
                timer.Stop();
                ToastPopup.IsOpen = false;
            };
            timer.Start();
        }

        private CustomPopupPlacement[] ToastPopup_CustomPopupPlacement(Size popupSize, Size targetSize, Point offset)
        {
            // 计算屏幕右下角位置
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;

            var x = screenWidth - popupSize.Width - 10; // 距离右边10像素
            var y = screenHeight - popupSize.Height - 10; // 距离底部10像素

            return new[] { new CustomPopupPlacement(new Point(x, y), PopupPrimaryAxis.Horizontal) };
        }

        // 使用示例
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShowToast("提示", "操作成功完成！", 2000);
        }
    }
}