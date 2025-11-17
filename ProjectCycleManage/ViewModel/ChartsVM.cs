using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ProjectCycleManage.Model;
using ProjectManagement.Data;
using ProjectManagement.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectCycleManage.ViewModel
{
    public partial class ChartsVM : ObservableObject
    {
        #region 公共信息
        private int _startyear;
        private int _endyear;


        #region 标签中文设置

        public SolidColorPaint LegendTextPaint { get; set; } =
            new SolidColorPaint
            {
                Color = new SKColor(51, 51, 51),
                SKTypeface = SKFontManager.Default.MatchCharacter('汉')
            };

        public SolidColorPaint TipTextPaint { get; set; } =
            new SolidColorPaint()
            {
                Color = SKColors.DarkSlateGray,
                SKTypeface = SKFontManager.Default.MatchCharacter('汉')
            };

        #endregion

        #endregion



        #region 预算
        [ObservableProperty]
        private ISeries[] _series;

        [ObservableProperty]
        private Axis[] _xAxes;

        /// <summary>
        /// 获取年度预算
        /// </summary>
        public void GetAnnualbudget()
        {
            using (var context = new ProjectContext())
            {
                var annualBudget = context.AnnualBudgetTable
                    .Where(p => p.Year >= _startyear && p.Year <= _endyear)
                    .Select(p => new {p.Year , p.Budget})
                    .OrderBy(p => p.Year)
                    .ToList();

                var projectExpenditures = context.Projects
                    .Where(p => p.Year >= _startyear && p.Year <= _endyear)
                    .Select(p => new { p.Year, p.ActualExpenditure })
                    .OrderBy(p => p.Year)
                    .ToList();

                Dictionary<double, double> dict1 = new Dictionary<double, double>();
                Dictionary<double, double> dict2 = new Dictionary<double, double>();

                foreach (var item in annualBudget)
                {
                    var projectdata = projectExpenditures
                        .Where(p => p.Year == item.Year)
                        .ToList();
                    double totalExpenditure = 0;
                    foreach (var project in projectdata)
                    {
                        if (double.TryParse(project.ActualExpenditure, out double expenditure))
                        {
                            totalExpenditure += expenditure;
                        }
                    }

                    dict1.Add(item.Year , totalExpenditure);
                    dict2.Add(item.Year , item.Budget);
                }


                string getdata = "";
                foreach (var item in dict1)
                {
                    getdata += item.Key.ToString() + "--" + item.Value.ToString() + "--";
                }
                //MessageBox.Show(getdata);
                List<double> years = dict2.Values.ToList();

                var valus = dict1.Values.ToList();

                List<string> AxisLabels = new List<string>();
                foreach (var item in dict1)
                {
                    AxisLabels.Add(item.Key.ToString());
                }
                //Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                //{
                    Series =
                    [
                        new ColumnSeries<double>
                        {
                            Name = "预算金额",
                            Values = years,
                            MaxBarWidth = 30,
                            Fill = new SolidColorPaint(new SKColor(76, 172, 250)),
                        },

                        new ColumnSeries<double>
                        {
                            Name = "实际支出",
                            Values = valus,
                            MaxBarWidth = 30,
                            Fill = new SolidColorPaint(new SKColor(109, 203, 112)),
                        }

                    ];

                    XAxes =
                    [
                        new Axis
                        {
                            Labels = AxisLabels,
                            LabelsRotation = 0,
                            SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                            SeparatorsAtCenter = false,
                            TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                            TicksAtCenter = true,
                            ForceStepToMin = true,
                            MinStep = 1,
                            LabelsPaint = new SolidColorPaint(SKColors.Black){SKTypeface = SKFontManager.Default.MatchCharacter('汉')}, // 标签颜色
                        }
                    ];
                //}));

            }
        }
        //public ISeries[] Series { get; set; } =
        //[
        //    new ColumnSeries<double>
        //    {
        //        Name = "预算金额",
        //        Values = [2000, 5000, 4000, 5000, 5000, 5000],
        //        MaxBarWidth = 30,
        //        Fill = new SolidColorPaint(new SKColor(76, 172, 250)),
        //    },

        //    new ColumnSeries<double>
        //    {
        //        Name = "实际支出",
        //        Values = [3000, 1000, 6000, 5000, 5000, 5000],
        //        MaxBarWidth = 30,
        //        Fill = new SolidColorPaint(new SKColor(109, 203, 112)),
        //    }
        //];

        //public Axis[] XAxes { get; set; } =
        //[
        //    new Axis
        //    {
        //        Labels = ["测试 1", "Category 2", "Category 3","Category 4", "Category 5", "Category 6","Category 7", "Category 8", "Category 9"],
        //        LabelsRotation = 0,
        //        SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
        //        SeparatorsAtCenter = false,
        //        TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
        //        TicksAtCenter = true,
        //        // By default the axis tries to optimize the number of 
        //        // labels to fit the available space, 
        //        // when you need to force the axis to show all the labels then you must: 
        //        ForceStepToMin = true,
        //        MinStep = 1,
        //        LabelsPaint = new SolidColorPaint(SKColors.Black){SKTypeface = SKFontManager.Default.MatchCharacter('汉')}, // 标签颜色
        //    }
        //];
        #endregion




        public ChartsVM(int startyear , int stopyear)
        {
            _startyear = startyear;
            _endyear = stopyear;

            Task.Run(() =>
            {
                GetAnnualbudget();
            });
        }

    }
}
