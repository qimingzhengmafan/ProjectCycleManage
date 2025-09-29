using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TreeViewTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            //_viewModel = new MainViewModel();
            //DataContext = _viewModel;
        }

        //private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{
        //    if (e.NewValue is TreeNode node)
        //    {
        //        _viewModel.SelectedNode = node;
        //    }
        //}
    }
}