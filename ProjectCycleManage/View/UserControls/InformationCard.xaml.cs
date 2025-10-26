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


        public Visibility FileVisib
        {
            get { return (Visibility)GetValue(FileVisibProperty); }
            set { SetValue(FileVisibProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileVisib.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileVisibProperty =
            DependencyProperty.Register(nameof(FileVisib), typeof(Visibility), typeof(InformationCard), new PropertyMetadata(Visibility.Collapsed));



        public bool? FileIsExist
        {
            get { return (bool?)GetValue(FileIsExistProperty); }
            set { SetValue(FileIsExistProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileIsExist.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileIsExistProperty =
            DependencyProperty.Register(nameof(FileIsExist), typeof(bool?), typeof(InformationCard), new PropertyMetadata(null));



        public string FileName
        {
            get { return (string)GetValue(FileNameProperty); }
            set { SetValue(FileNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FileName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FileNameProperty =
            DependencyProperty.Register(nameof(FileName), typeof(string), typeof(InformationCard), new PropertyMetadata(null));


        #endregion

        #region 文档-OA


        public Visibility File_OA_Visib
        {
            get { return (Visibility)GetValue(File_OA_VisibProperty); }
            set { SetValue(File_OA_VisibProperty, value); }
        }

        // Using a DependencyProperty as the backing store for File_OA_Visib.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty File_OA_VisibProperty =
            DependencyProperty.Register(nameof(File_OA_Visib), typeof(Visibility), typeof(InformationCard), new PropertyMetadata(Visibility.Collapsed));



        public string File_OA_InData
        {
            get { return (string)GetValue(File_OA_InDataProperty); }
            set { SetValue(File_OA_InDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for File_OA_InData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty File_OA_InDataProperty =
            DependencyProperty.Register(nameof(File_OA_InData), typeof(string), typeof(InformationCard), new PropertyMetadata(null));




        public string File_OA_WriteData
        {
            get { return (string)GetValue(File_OA_WriteDataProperty); }
            set { SetValue(File_OA_WriteDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for File_OA_WriteData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty File_OA_WriteDataProperty =
            DependencyProperty.Register(nameof(File_OA_WriteData), typeof(string), typeof(InformationCard), new PropertyMetadata(null));



        public ICommand File_OA_BtnCommand
        {
            get { return (ICommand)GetValue(File_OA_BtnCommandProperty); }
            set { SetValue(File_OA_BtnCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for File_OA_BtnCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty File_OA_BtnCommandProperty =
            DependencyProperty.Register(nameof(File_OA_BtnCommand), typeof(ICommand), typeof(InformationCard), new PropertyMetadata(null));


        #endregion

        #region 信息-项目负责人/项目跟进人

        #endregion

        #region 信息-填写内容

        #endregion




    }
}
