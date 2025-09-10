using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using Page_Navigation_App.Utilities;
using ProjectManagementFrame.Model;
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

namespace ProjectManagementFrame.ViewModel
{
    public class MainVM:ViewModelBase
    {
        #region Command
        public ICommand ConnectCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        #endregion
        private SQLSentence SQLSentence1 = new SQLSentence();

        private MySQLOperationClass DataPool = new MySQLOperationClass();

        private OverviewVM _overview = new OverviewVM();
        public OverviewVM OverviewVMInfor
        {
            get => _overview; 
            set => _overview = value;
        }

        public MainVM()
        {
            int _index = 0;
            string[] _names = new string[4] { "2022", "2023", "2024", "2025" };

            int _index1 = 0;
            string[] _names1 = new string[7] { "项目需求", "立项评审", "方案评审", "设备采购",
                                                "预验收/组装调试", "设备验收", "完成" };

            DataPool.Connect();

            List<int> lista = new List<int>();
            lista.Add(DataPool.Search(SQLSentence1.Search2022 , 2));
            lista.Add(DataPool.Search(SQLSentence1.Search2023, 2));
            lista.Add(DataPool.Search(SQLSentence1.Search2024, 2));
            lista.Add(DataPool.Search(SQLSentence1.Search2025, 2));
            OverviewVMInfor.AllProjectsInformation.Series = lista.AsPieSeries((value, series) =>
            {

                series.Name = _names[_index++ % _names.Length];
                series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle;
                series.DataLabelsSize = 15;
                series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
                series.DataLabelsFormatter =
                   point =>
                       $"{point.Coordinate.PrimaryValue} " + "/ " +
                       $"{point.StackedValue.Total} ";
                series.ToolTipLabelFormatter = point => $"{point.StackedValue.Share:P2}";
            });

            List<int> listb = new List<int>();
            listb.Add(DataPool.Search(SQLSentence1.ProjectStage_ProjectRequirements, 2));
            listb.Add(DataPool.Search(SQLSentence1.ProjectStage_ProjectInitiationReview, 2));
            listb.Add(DataPool.Search(SQLSentence1.ProjectStage_SchemeReview, 2));
            listb.Add(DataPool.Search(SQLSentence1.ProjectStage_EquipmentProcurement, 2));

            listb.Add(DataPool.Search(SQLSentence1.ProjectStage_PreAcceptanceassemblyAndCommissioning, 2));
            listb.Add(DataPool.Search(SQLSentence1.ProjectStage_EquipmentAcceptance, 2));
            listb.Add(DataPool.Search(SQLSentence1.ProjectStage_Completed, 2));
            OverviewVMInfor.ProjectStage.Series = listb.AsPieSeries((value, series) =>
            {
                series.Name = _names1[_index1++ % _names1.Length];
                series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle;
                series.DataLabelsSize = 15;
                series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
                series.DataLabelsFormatter =
                   point =>
                       $"{point.Coordinate.PrimaryValue} " + "/ " +
                       $"{point.StackedValue.Total} ";
                series.ToolTipLabelFormatter = point => $"{point.StackedValue.Share:P2}";
            });


            ConnectCommand = new RelayCommand(ConnectIcommand);
            SearchCommand = new RelayCommand(SearchIcommand);
        }

        //ProjectInfor

        #region CommandFunction
        private void ConnectIcommand(object obj)
        {
            DataPool.Connect();
        }

        private void SearchIcommand(object obj)
        {
            int op = 0;
            int i = DataPool.Search("SELECT COUNT(*) FROM `key_value3` WHERE 年份 = 2023;", op);
        }
        #endregion
    }
}
