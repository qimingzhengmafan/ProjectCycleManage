using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using Page_Navigation_App.Utilities;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace ProjectManagement.ViewModel
{
    public class MainVM:ViewModelBase
    {
        #region Command
        public ICommand ConnectCommand { get; set; }
        //public ICommand SearchCommand { get; set; }

        #region UIIcommand
        public ICommand OverViewICommand { get; set; }
        public ICommand AddViewICommand { get; set; }
        public ICommand ChangeViewICommand { get; set; }
        public ICommand SettingViewICommand { get; set; }
        public ICommand TableViewICommand { get; set; }
        #endregion

        #endregion

        #region UIVisilibity
        private Visibility _overviewvisibility = Visibility.Visible;
        public Visibility OverViewVisibility
        {
            get => _overviewvisibility;
            set
            {
                _overviewvisibility = value;
                OnPropertyChanged();
            }
        }



        private Visibility _addviewvisibility = Visibility.Collapsed;
        public Visibility AddViewVisibility
        {
            get => _addviewvisibility;
            set
            {
                _addviewvisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _changeviewvisibility = Visibility.Collapsed;
        public Visibility ChangeViewVisibility
        {
            get => _changeviewvisibility;
            set
            {
                _changeviewvisibility = value;
                OnPropertyChanged();
            }
        }
        private Visibility _settingviewvisibility = Visibility.Collapsed;
        public Visibility SettingViewVisibility
        {
            get => _settingviewvisibility;
            set
            {
                _settingviewvisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _tableviewvisibility = Visibility.Collapsed;
        public Visibility TableViewVisibility
        {
            get => _tableviewvisibility;
            set
            {
                _tableviewvisibility = value;
                OnPropertyChanged();
            }
        }
        #endregion

        //private SQLSentence SQLSentence1 = new SQLSentence();

        //private MySQLOperationClass DataPool = new MySQLOperationClass();

        #region ViewInformation
        private OverviewVM _overview = new OverviewVM();
        public OverviewVM OverviewVMInfor
        {
            get => _overview;
            set => _overview = value;
        }

        private AddVM _addviewvm = new AddVM();
        public AddVM AddViewVMInfor
        {
            get => _addviewvm;
            set => _addviewvm = value;
        }

        private ChangeVM _changeviewvm = new ChangeVM();
        public ChangeVM ChangeViewVMInfor
        {
            get => _changeviewvm;
            set => _changeviewvm= value;
        }

        private SettingVM settingviewvm = new SettingVM();
        public SettingVM SettingVMInfor
        {
            get => settingviewvm;
            set => settingviewvm = value;
        }

        private TableVM tableviewvm = new TableVM();
        public TableVM TableVMInfor
        {
            get => tableviewvm;
            set => tableviewvm = value;
        }

        #endregion


        public MainVM()
        {
            int _index = 0;
            string[] _names = new string[4] { "2022", "2023", "2024", "2025" };

            int _index1 = 0;
            string[] _names1 = new string[7] { "项目需求", "立项评审", "方案评审", "设备采购",
                                                "预验收/组装调试", "设备验收", "完成" };

            //DataPool.Connect();

            //List<int> lista = new List<int>();
            //lista.Add(DataPool.Search(SQLSentence1.Search2022 , 2));
            //lista.Add(DataPool.Search(SQLSentence1.Search2023, 2));
            //lista.Add(DataPool.Search(SQLSentence1.Search2024, 2));
            //lista.Add(DataPool.Search(SQLSentence1.Search2025, 2));
            //OverviewVMInfor.AllProjectsInformation.Series = lista.AsPieSeries((value, series) =>
            //{

            //    series.Name = _names[_index++ % _names.Length];
            //    series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle;
            //    series.DataLabelsSize = 15;
            //    series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
            //    series.DataLabelsFormatter =
            //       point =>
            //           $"{point.Coordinate.PrimaryValue} " + "/ " +
            //           $"{point.StackedValue.Total} ";
            //    series.ToolTipLabelFormatter = point => $"{point.StackedValue.Share:P2}";
            //});

            //List<int> listb = new List<int>();
            //listb.Add(DataPool.Search(SQLSentence1.ProjectStage_ProjectRequirements, 2));
            //listb.Add(DataPool.Search(SQLSentence1.ProjectStage_ProjectInitiationReview, 2));
            //listb.Add(DataPool.Search(SQLSentence1.ProjectStage_SchemeReview, 2));
            //listb.Add(DataPool.Search(SQLSentence1.ProjectStage_EquipmentProcurement, 2));

            //listb.Add(DataPool.Search(SQLSentence1.ProjectStage_PreAcceptanceassemblyAndCommissioning, 2));
            //listb.Add(DataPool.Search(SQLSentence1.ProjectStage_EquipmentAcceptance, 2));
            //listb.Add(DataPool.Search(SQLSentence1.ProjectStage_Completed, 2));
            //OverviewVMInfor.ProjectStage.Series = listb.AsPieSeries((value, series) =>
            //{
            //    series.Name = _names1[_index1++ % _names1.Length];
            //    series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle;
            //    series.DataLabelsSize = 15;
            //    series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
            //    series.DataLabelsFormatter =
            //       point =>
            //           $"{point.Coordinate.PrimaryValue} " + "/ " +
            //           $"{point.StackedValue.Total} ";
            //    series.ToolTipLabelFormatter = point => $"{point.StackedValue.Share:P2}";
            //});


            ConnectCommand = new RelayCommand(ConnectIcommand);
            OverViewICommand = new RelayCommand(OverViewICommandFun);
            AddViewICommand = new RelayCommand(AddViewICommandFun);
            ChangeViewICommand = new RelayCommand(ChangeViewICommandFun);
            SettingViewICommand = new RelayCommand(SettingViewICommandFun);
            TableViewICommand = new RelayCommand(TableViewICommandFun);

        }

        //ProjectInfor

        #region CommandFunction
        private void ConnectIcommand(object obj)
        {
            //DataPool.Connect();
        }

        #region UIIcommandFun
        private void OverViewICommandFun(object obj)
        {
            OverViewVisibility = Visibility.Visible;

            AddViewVisibility = Visibility.Collapsed;
            ChangeViewVisibility = Visibility.Collapsed;
            SettingViewVisibility = Visibility.Collapsed;
            TableViewVisibility = Visibility.Collapsed;
        }

        private void AddViewICommandFun(object obj)
        {
            AddViewVisibility = Visibility.Visible;

            OverViewVisibility = Visibility.Collapsed;
            ChangeViewVisibility = Visibility.Collapsed;
            SettingViewVisibility = Visibility.Collapsed;
            TableViewVisibility = Visibility.Collapsed;
        }

        private void ChangeViewICommandFun(object obj)
        {
            ChangeViewVisibility = Visibility.Visible;

            SettingViewVisibility = Visibility.Collapsed;
            TableViewVisibility = Visibility.Collapsed;
            AddViewVisibility = Visibility.Collapsed;
            OverViewVisibility = Visibility.Collapsed;
        }

        private void SettingViewICommandFun(object obj)
        {
            SettingViewVisibility = Visibility.Visible;

            TableViewVisibility = Visibility.Collapsed;
            AddViewVisibility = Visibility.Collapsed;
            OverViewVisibility = Visibility.Collapsed;
            ChangeViewVisibility = Visibility.Collapsed;
        }

        private void TableViewICommandFun(object obj)
        {
            TableViewVisibility = Visibility.Visible;

            SettingViewVisibility = Visibility.Collapsed;
            AddViewVisibility = Visibility.Collapsed;
            OverViewVisibility = Visibility.Collapsed;
            ChangeViewVisibility = Visibility.Collapsed;
        }
        #endregion
        #endregion
    }
}
