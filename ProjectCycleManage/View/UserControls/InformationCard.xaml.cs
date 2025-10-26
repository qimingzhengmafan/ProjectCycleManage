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

namespace ProjectCycleManage.View.UserControls
{
    /// <summary>
    /// InformationCard.xaml 的交互逻辑
    /// </summary>
    public partial class InformationCard : UserControl
    {
        public InformationCard()
        {
            InitializeComponent();
        }



        public string InformationTag
        {
            get { return (string)GetValue(InformationTagProperty); }
            set { SetValue(InformationTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InformationTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InformationTagProperty =
            DependencyProperty.Register(nameof(InformationTag), typeof(string), typeof(InformationCard), new PropertyMetadata(null));


        public string InformationTypes
        {
            get { return (string)GetValue(InformationTypesProperty); }
            set { SetValue(InformationTypesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InformationTypes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InformationTypesProperty =
            DependencyProperty.Register(nameof(InformationTypes), typeof(string), typeof(InformationCard), new PropertyMetadata(null , OnInformationTypesChanged));

        private static void OnInformationTypesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        #region 文档

        #endregion

        #region 文档-OA

        #endregion

        #region 信息-项目负责人/项目跟进人

        #endregion

        #region 信息-填写内容

        #endregion




    }
}
