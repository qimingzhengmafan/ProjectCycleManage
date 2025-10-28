using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing.IndexedProperties;
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

        /// <summary>
        /// 登陆者等级
        /// </summary>
        public int Loginpersonnamegrade
        {
            get { return (int)GetValue(LoginpersonnamegradeProperty); }
            set { SetValue(LoginpersonnamegradeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Loginpersonnamegrade.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoginpersonnamegradeProperty =
            DependencyProperty.Register(nameof(Loginpersonnamegrade), typeof(int), typeof(InformationCard), new PropertyMetadata(0));



        public bool CardIsEnable
        {
            get { return (bool)GetValue(CardIsEnableProperty); }
            set { SetValue(CardIsEnableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CardIsEnable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CardIsEnableProperty =
            DependencyProperty.Register(nameof(CardIsEnable), typeof(bool), typeof(InformationCard), new PropertyMetadata(true));



        /// <summary>
        /// 控件级别
        /// </summary>
        public string InformationTag
        {
            get { return (string)GetValue(InformationTagProperty); }
            set { SetValue(InformationTagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InformationTag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InformationTagProperty =
            DependencyProperty.Register(nameof(InformationTag), typeof(string), typeof(InformationCard), new PropertyMetadata(null , OnInformationgradeChanged));

        private static void OnInformationgradeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var inforcardctrl = (InformationCard)d;
            string newdata = (string)e.NewValue;

            if (inforcardctrl.Loginpersonnamegrade == 10)
            {
                inforcardctrl.CardIsEnable = true;
            }
            else
            {
                if (inforcardctrl.Loginpersonnamegrade == Convert.ToInt32(newdata))
                {
                    inforcardctrl.CardIsEnable = true;
                }
                else
                {
                    inforcardctrl.CardIsEnable = false;
                }
            }
        }

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
            var inforcardctrl = (InformationCard)d;
            string newdata = (string)e.NewValue;

            switch (newdata)
            {
                case "文档":
                    inforcardctrl.FileVisib = Visibility.Visible;

                    inforcardctrl.Infor_People_Visib = Visibility.Collapsed;
                    inforcardctrl.Infor_Text_Visib = Visibility.Collapsed;
                    inforcardctrl.File_OA_Visib = Visibility.Collapsed;
                    inforcardctrl.Infor_Date = Visibility.Collapsed;
                    break;

                case "信息-下拉框":
                    inforcardctrl.Infor_People_Visib = Visibility.Visible;

                    inforcardctrl.FileVisib = Visibility.Collapsed;
                    inforcardctrl.Infor_Text_Visib = Visibility.Collapsed;
                    inforcardctrl.File_OA_Visib = Visibility.Collapsed;
                    inforcardctrl.Infor_Date = Visibility.Collapsed;

                    break;

                case "信息-填写":
                    inforcardctrl.Infor_Text_Visib = Visibility.Visible;

                    inforcardctrl.FileVisib = Visibility.Collapsed;
                    inforcardctrl.Infor_People_Visib = Visibility.Collapsed;
                    inforcardctrl.File_OA_Visib = Visibility.Collapsed;
                    inforcardctrl.Infor_Date = Visibility.Collapsed;
                    break;

                case "文档-OA":
                    inforcardctrl.File_OA_Visib = Visibility.Visible;

                    inforcardctrl.FileVisib = Visibility.Collapsed;
                    inforcardctrl.Infor_People_Visib = Visibility.Collapsed;
                    inforcardctrl.Infor_Text_Visib = Visibility.Collapsed;
                    inforcardctrl.Infor_Date = Visibility.Collapsed;

                    break;

                case "信息-日期":
                    inforcardctrl.Infor_Date = Visibility.Visible;

                    inforcardctrl.FileVisib = Visibility.Collapsed;
                    inforcardctrl.Infor_People_Visib = Visibility.Collapsed;
                    inforcardctrl.Infor_Text_Visib = Visibility.Collapsed;
                    inforcardctrl.File_OA_Visib = Visibility.Collapsed;

                    break;

                default:
                    inforcardctrl.Infor_Date = Visibility.Collapsed;

                    inforcardctrl.FileVisib = Visibility.Collapsed;
                    inforcardctrl.Infor_People_Visib = Visibility.Collapsed;
                    inforcardctrl.Infor_Text_Visib = Visibility.Collapsed;
                    inforcardctrl.File_OA_Visib = Visibility.Collapsed;
                    break;

            }
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



        public ICommand CheckCommand
        {
            get { return (ICommand)GetValue(CheckCommandProperty); }
            set { SetValue(CheckCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CheckCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CheckCommandProperty =
            DependencyProperty.Register(nameof(CheckCommand), typeof(ICommand), typeof(InformationCard), new PropertyMetadata(null));



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

        #region 信息-下拉框-项目负责人/项目跟进人



        public Visibility Infor_People_Visib
        {
            get { return (Visibility)GetValue(Infor_People_VisibProperty); }
            set { SetValue(Infor_People_VisibProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Infor_People_Visib.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Infor_People_VisibProperty =
            DependencyProperty.Register(nameof(Infor_People_Visib), typeof(Visibility), typeof(InformationCard), new PropertyMetadata(Visibility.Collapsed));




        public string Infor_Prople
        {
            get { return (string)GetValue(Infor_PropleProperty); }
            set { SetValue(Infor_PropleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Infor_Prople.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Infor_PropleProperty =
            DependencyProperty.Register(nameof(Infor_Prople), typeof(string), typeof(InformationCard), new PropertyMetadata(null));



        public ObservableCollection<PeopleTable> Infor_People_Ob
        {
            get { return (ObservableCollection<PeopleTable>)GetValue(Infor_People_ObProperty); }
            set { SetValue(Infor_People_ObProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Infor_People_Ob.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Infor_People_ObProperty =
            DependencyProperty.Register(nameof(Infor_People_Ob), typeof(ObservableCollection<PeopleTable>), typeof(InformationCard), new PropertyMetadata(null));




        public PeopleTable Infor_Sele_People
        {
            get { return (PeopleTable)GetValue(Infor_Sele_PeopleProperty); }
            set { SetValue(Infor_Sele_PeopleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Infor_Sele_People.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Infor_Sele_PeopleProperty =
            DependencyProperty.Register(nameof(Infor_Sele_People), typeof(PeopleTable), typeof(InformationCard), new PropertyMetadata(null));




        public ICommand Infor_People_BtnCom
        {
            get { return (ICommand)GetValue(Infor_People_BtnComProperty); }
            set { SetValue(Infor_People_BtnComProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Infor_People_BtnCom.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Infor_People_BtnComProperty =
            DependencyProperty.Register(nameof(Infor_People_BtnCom), typeof(ICommand), typeof(InformationCard), new PropertyMetadata(null));



        #endregion


        #region 信息-填写内容



        public Visibility Infor_Text_Visib
        {
            get { return (Visibility)GetValue(Infor_Text_VisibProperty); }
            set { SetValue(Infor_Text_VisibProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Infor_Text_Visib.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Infor_Text_VisibProperty =
            DependencyProperty.Register(nameof(Infor_Text_Visib), typeof(Visibility), typeof(InformationCard), new PropertyMetadata(Visibility.Collapsed));




        public string Infor_Text_In
        {
            get { return (string)GetValue(Infor_Text_InProperty); }
            set { SetValue(Infor_Text_InProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Infor_Text_In.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Infor_Text_InProperty =
            DependencyProperty.Register(nameof(Infor_Text_In), typeof(string), typeof(InformationCard), new PropertyMetadata(null));



        public string Infor_Text_Write
        {
            get { return (string)GetValue(Infor_Text_WriteProperty); }
            set { SetValue(Infor_Text_WriteProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Infor_Text_Write.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Infor_Text_WriteProperty =
            DependencyProperty.Register(nameof(Infor_Text_Write), typeof(string), typeof(InformationCard), new PropertyMetadata(null));



        public ICommand Infor_Text_BtnCom
        {
            get { return (ICommand)GetValue(Infor_Text_BtnComProperty); }
            set { SetValue(Infor_Text_BtnComProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Infor_Text_BtnCom.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Infor_Text_BtnComProperty =
            DependencyProperty.Register(nameof(Infor_Text_BtnCom), typeof(ICommand), typeof(InformationCard), new PropertyMetadata(null));


        #endregion

        #region 信息-日期


        public Visibility Infor_Date
        {
            get { return (Visibility)GetValue(Infor_DateProperty); }
            set { SetValue(Infor_DateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Infor_Date.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Infor_DateProperty =
            DependencyProperty.Register(nameof(Infor_Date), typeof(Visibility), typeof(InformationCard), new PropertyMetadata(Visibility.Collapsed));



        public string Infor_Data_Text_In
        {
            get { return (string)GetValue(Infor_Data_Text_InProperty); }
            set { SetValue(Infor_Data_Text_InProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Infor_Data_Text_In.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Infor_Data_Text_InProperty =
            DependencyProperty.Register(nameof(Infor_Data_Text_In), typeof(string), typeof(InformationCard), new PropertyMetadata(null));



        public DateTime? Infor_Date_SelectTime
        {
            get { return (DateTime?)GetValue(Infor_Date_SelectTimeProperty); }
            set { SetValue(Infor_Date_SelectTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Infor_Date_SelectTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Infor_Date_SelectTimeProperty =
            DependencyProperty.Register(nameof(Infor_Date_SelectTime), typeof(DateTime?), typeof(InformationCard), new PropertyMetadata(null));





        public ICommand Infor_Date_Btn_Com
        {
            get { return (ICommand)GetValue(Infor_Date_Btn_ComProperty); }
            set { SetValue(Infor_Date_Btn_ComProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Infor_Date_Btn_Com.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Infor_Date_Btn_ComProperty =
            DependencyProperty.Register(nameof(Infor_Date_Btn_Com), typeof(ICommand), typeof(InformationCard), new PropertyMetadata(null));


        #endregion
    }

}
