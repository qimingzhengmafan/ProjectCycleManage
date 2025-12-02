using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProjectCycleManage.ViewModel;

namespace ProjectCycleManage
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

        /// <summary>
        /// 窗口关闭时停止监控
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            // 获取MainVM实例并停止监控
            if (DataContext is MainVM mainVM)
            {
                mainVM.StopProjectMonitoring();
            }
            
            base.OnClosed(e);
        }
    }
}