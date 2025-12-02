using System.Text;
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

namespace DrawerTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _textCounter = 1;
        private Color[] _colors = {
            Colors.LightBlue, Colors.LightGreen, Colors.LightCoral,
            Colors.LightGoldenrodYellow, Colors.LightPink, Colors.LightGray
        };

        public MainWindow()
        {
            InitializeComponent();

            // 初始化动态控件
            //DynamicControl.DisplayTime = DateTime.Now;
        }

        //private void UpdateShortText_Click(object sender, RoutedEventArgs e)
        //{
        //    DynamicControl.ContentText = $"短文本 {_textCounter++}";
        //    DynamicControl.DisplayTime = DateTime.Now;
        //}

        //private void UpdateLongText_Click(object sender, RoutedEventArgs e)
        //{
        //    DynamicControl.ContentText = $"这是一个非常长的动态更新文本内容，计数为：{_textCounter++}，时间：{DateTime.Now:HH:mm:ss}";
        //    DynamicControl.DisplayTime = DateTime.Now;
        //}

        //private void ChangeColor_Click(object sender, RoutedEventArgs e)
        //{
        //    Random rnd = new Random();
        //    Color randomColor = _colors[rnd.Next(_colors.Length)];
        //    DynamicControl.SetColorTheme(randomColor);
        //}

    }
}