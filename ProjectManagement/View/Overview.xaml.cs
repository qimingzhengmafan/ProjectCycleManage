using ProjectManagement.ViewModel;
using System;
using System.Collections.Generic;
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

namespace ProjectManagement.View
{
    /// <summary>
    /// Overview.xaml 的交互逻辑
    /// </summary>
    public partial class Overview : UserControl
    {
        //// 定义依赖属性，用于接收主视图的子ViewModel
        //public static readonly DependencyProperty ChildViewModelProperty =
        //    DependencyProperty.Register(
        //        "ChildViewModel",
        //        typeof(OverviewVM),
        //        typeof(Overview),
        //        new PropertyMetadata(null));

        //// 依赖属性的CLR包装器
        //public OverviewVM ViewModelInfor
        //{
        //    get => (OverviewVM)GetValue(ChildViewModelProperty);
        //    set => SetValue(ChildViewModelProperty, value);
        //}
        public Overview()
        {
            InitializeComponent();
        }
    }
}
