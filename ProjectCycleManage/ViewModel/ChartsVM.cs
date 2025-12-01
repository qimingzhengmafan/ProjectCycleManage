using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ProjectCycleManage.Model;
using ProjectManagement.Data;
using ProjectManagement.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

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


        [ObservableProperty]
        private IEnumerable<ISeries> _projectbudget_pie;
        public void GetProjectBudget_Pie()
        {
            using (var context = new ProjectContext())
            {
                var ProjectNum = context.Projects
                    .Where(p => p.Year >= _startyear && p.Year <= _endyear)
                    .ToList();
                var Budget = context.AnnualBudgetTable
                    .Where(p => p.Year >= _startyear && p.Year <= _endyear)
                    .ToList();

                if (Budget == null)
                    return;

                double BudgetAmount = 0.0;
                var Completeproject = ProjectNum.Where(p => p.ProjectPhaseStatusId == 104).ToList();
                var Unfinished = ProjectNum.Where(p => p.ProjectPhaseStatusId != 104).ToList();

                foreach (var item in Budget)
                {
                    BudgetAmount += item.Budget;
                }

                //已完成金额
                //Amount
                double CompleteAmount = 0.0;
                foreach (var item in Completeproject)
                {
                    if (double.TryParse(item.ActualExpenditure, out double expenditure))
                    {
                        CompleteAmount += expenditure;
                    }
                }

                //未完成金额
                double UnfinishedAmount = 0.0;
                foreach (var item in Unfinished)
                {
                    if (double.TryParse(item.ActualExpenditure, out double expenditure))
                    {
                        UnfinishedAmount += expenditure;
                    }
                }



                List<(string, double)> projectstagenum = new List<(string, double)>();
                projectstagenum.Add(("已完成" , CompleteAmount));
                projectstagenum.Add(("未完成", UnfinishedAmount));
                projectstagenum.Add(("剩余金额", BudgetAmount - CompleteAmount - UnfinishedAmount));

                List<string> _names = new();
                List<double> values = new();

                foreach (var data in projectstagenum)
                {
                    _names.Add(data.Item1);
                    values.Add(data.Item2);
                }
                int _index = 0;
                
                Projectbudget_pie =
                    values.AsPieSeries((value, series) =>
                    {
                        series.Name = _names[_index++ % _names.Count];

                        series.Pushout = 5;
                    });

            }
        }

        /// <summary>
        /// 获取年度预算
        /// </summary>
        //public void GetAnnualbudget()
        //{
        //    using (var context = new ProjectContext())
        //    {
        //        var annualBudget = context.AnnualBudgetTable
        //            .Where(p => p.Year >= _startyear && p.Year <= _endyear)
        //            .Select(p => new {p.Year , p.Budget})
        //            .OrderBy(p => p.Year)
        //            .ToList();

        //        var projectExpenditures = context.Projects
        //            .Where(p => p.Year >= _startyear && p.Year <= _endyear)
        //            .Select(p => new { p.Year, p.ActualExpenditure })
        //            .OrderBy(p => p.Year)
        //            .ToList();

        //        Dictionary<double, double> dict1 = new Dictionary<double, double>();
        //        Dictionary<double, double> dict2 = new Dictionary<double, double>();

        //        foreach (var item in annualBudget)
        //        {
        //            var projectdata = projectExpenditures
        //                .Where(p => p.Year == item.Year)
        //                .ToList();
        //            double totalExpenditure = 0;
        //            foreach (var project in projectdata)
        //            {
        //                if (double.TryParse(project.ActualExpenditure, out double expenditure))
        //                {
        //                    totalExpenditure += expenditure;
        //                }
        //            }

        //            dict1.Add(item.Year , totalExpenditure);
        //            dict2.Add(item.Year , item.Budget);
        //        }


        //        //string getdata = "";
        //        //foreach (var item in dict1)
        //        //{
        //        //    getdata += item.Key.ToString() + "--" + item.Value.ToString() + "--";
        //        //}
        //        //MessageBox.Show(getdata);
        //        List<double> years = dict2.Values.ToList();

        //        var valus = dict1.Values.ToList();

        //        List<string> AxisLabels = new List<string>();
        //        foreach (var item in dict1)
        //        {
        //            AxisLabels.Add(item.Key.ToString());
        //        }
        //        //Application.Current.Dispatcher.BeginInvoke(new Action(() =>
        //        //{
        //        Series =
        //        [
        //            new ColumnSeries<double>
        //            {
        //                Name = "预算金额",
        //                Values = years,
        //                MaxBarWidth = 30,
        //                Fill = new SolidColorPaint(new SKColor(76, 172, 250)),
        //            },

        //            new ColumnSeries<double>
        //            {
        //                Name = "实际支出",
        //                Values = valus,
        //                MaxBarWidth = 30,
        //                Fill = new SolidColorPaint(new SKColor(109, 203, 112)),
        //            }

        //        ];

        //        XAxes =
        //        [
        //            new Axis
        //            {
        //                Labels = AxisLabels,
        //                LabelsRotation = 0,
        //                SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
        //                SeparatorsAtCenter = false,
        //                TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
        //                TicksAtCenter = true,
        //                ForceStepToMin = true,
        //                MinStep = 1,
        //                LabelsPaint = new SolidColorPaint(SKColors.Black){SKTypeface = SKFontManager.Default.MatchCharacter('汉')}, // 标签颜色
        //            }
        //        ];
        //        //}));

        //    }
        //}
        #endregion

        #region 历史投入分析

        [ObservableProperty]
        private ISeries[] _historinvest_amount;

        [ObservableProperty]
        private ISeries[] _historinvest_quantity;


        public void Historinvest_Amount()
        {
            using (var context = new ProjectContext())
            {
                // 检查年份范围是否有效
                if (_startyear > _endyear)
                {
                    Historinvest_amount = Array.Empty<ISeries>();
                    return;
                }

                var ProjectNum = context.Projects
                    .Where(p => p.Year >= _startyear && p.Year <= _endyear)
                    .GroupBy(p => p.Year)
                    .ToList();

                //年度预算
                var Budget = context.AnnualBudgetTable
                    .Where(p => p.Year >= _startyear && p.Year <= _endyear)
                    .OrderBy(p => p.Year)
                    .ToList();

                ///销售预测
                var salesvolume = context.SalesVolumeTables
                    .Where(p => p.Year >= _startyear && p.Year <= _endyear)
                    .OrderBy(p => p.Year)
                    .ToList();

                int NumberYear = _endyear - _startyear + 1;

                // 检查数据是否有效
                if (NumberYear <= 0 || Budget.Count == 0 || salesvolume.Count == 0 || ProjectNum.Count == 0)
                {
                    Historinvest_amount = Array.Empty<ISeries>();
                    return;
                }

                // 确保数据长度一致
                NumberYear = Math.Min(NumberYear, Math.Min(Budget.Count, Math.Min(salesvolume.Count, ProjectNum.Count)));

                // 创建并初始化数组元素
                Historinvest_amount = new LineSeries<ObservablePoint>[3]
                {
                    new LineSeries<ObservablePoint> { Name = "年度预算" },
                    new LineSeries<ObservablePoint> { Name = "销售预测" },
                    new LineSeries<ObservablePoint> { Name = "项目支出" }
                };

                ObservablePoint[] BudgetPoint = new ObservablePoint[NumberYear];
                for (int j = 0; j < NumberYear; j++)
                {
                    if (j < Budget.Count)
                        BudgetPoint[j] = new ObservablePoint(_startyear + j, Budget[j].Budget);
                    else
                        BudgetPoint[j] = new ObservablePoint(_startyear + j, 0);
                }

                ObservablePoint[] salesvolumePoint = new ObservablePoint[NumberYear];
                for (int j = 0; j < NumberYear; j++)
                {
                    if (j < salesvolume.Count)
                        salesvolumePoint[j] = new ObservablePoint(_startyear + j, salesvolume[j].SalesVolume);
                    else
                        salesvolumePoint[j] = new ObservablePoint(_startyear + j, 0);
                }

                List<(int , double)> data = new List<(int, double)> ();

                foreach (var item in ProjectNum)
                {
                    double value = 0.0;

                    foreach (var item1 in item)
                    {
                        if (double.TryParse(item1.ActualExpenditure, out double expenditure))
                        {
                            value += expenditure;
                        }
                    }
                    data.Add((item.Key.GetValueOrDefault(), value));
                }

                ObservablePoint[] ProjectPoint = new ObservablePoint[NumberYear];
                for (int j = 0; j < NumberYear; j++)
                {
                    if (j < data.Count)
                        ProjectPoint[j] = new ObservablePoint(data[j].Item1, data[j].Item2);
                    else
                        ProjectPoint[j] = new ObservablePoint(_startyear + j, 0);
                }

                // 安全赋值
                if (Historinvest_amount[0] != null)
                    Historinvest_amount[0].Values = BudgetPoint;
                if (Historinvest_amount[1] != null)
                    Historinvest_amount[1].Values = salesvolumePoint;
                if (Historinvest_amount[2] != null)
                    Historinvest_amount[2].Values = ProjectPoint;

            }

        }

        public void Historinvest_Quantity()
        {
            using (var context = new ProjectContext())
            {
                //// 检查年份范围是否有效
                //if (_startyear > _endyear)
                //{
                //    Historinvest_quantity = Array.Empty<ISeries>();
                //    return;
                //}

                // 当起始年份等于结束年份时，按月份分组统计
                if (_startyear == _endyear)
                {
                    // 获取该年份的所有项目，并按月份分组
                    var monthlyData = context.Projects
                        .Where(p => p.Year == _startyear && p.ApplicationTime.HasValue)
                        .AsEnumerable() // 在内存中处理，因为EF Core不支持复杂的日期分组
                        .GroupBy(p => p.ApplicationTime.Value.Month)
                        .Select(g => new
                        {
                            Month = g.Key,
                            ProjectCount = g.Count(),
                            TotalExpenditure = g.Sum(p => 
                                double.TryParse(p.ActualExpenditure, out double expenditure) ? expenditure : 0)
                        })
                        .OrderBy(x => x.Month)
                        .ToList();

                    // 确保包含所有月份（1-12月），即使没有数据
                    var allMonthsData = new List<(int Month, int ProjectCount, double TotalExpenditure)>();
                    for (int month = 1; month <= 12; month++)
                    {
                        var monthData = monthlyData.FirstOrDefault(m => m.Month == month);
                        allMonthsData.Add((month, monthData?.ProjectCount ?? 0, monthData?.TotalExpenditure ?? 0));
                    }

                    // 创建项目数量系列
                    var projectCountPoints = allMonthsData.Select(d => 
                        new ObservablePoint(d.Month, d.ProjectCount)).ToArray();

                    // 创建支出总和系列
                    var expenditurePoints = allMonthsData.Select(d => 
                        new ObservablePoint(d.Month, d.TotalExpenditure)).ToArray();

                    // 设置X轴标签（月份名称）
                    var monthLabels = new[] { "1月", "2月", "3月", "4月", "5月", "6月", 
                                              "7月", "8月", "9月", "10月", "11月", "12月" };

                    Historinvest_quantity = new LineSeries<ObservablePoint>[2]
                    {
                        new LineSeries<ObservablePoint> 
                        { 
                            Name = "项目数量",
                            Values = projectCountPoints,
                            Fill = new SolidColorPaint(new SKColor(76, 172, 250))
                        },
                        new LineSeries<ObservablePoint> 
                        { 
                            Name = "支出总和",
                            Values = expenditurePoints,
                            Fill = new SolidColorPaint(new SKColor(109, 203, 112))
                        }
                    };
                }
                else
                {
                    // 当起始年份不等于结束年份时，按年份分组统计
                    // 先获取数据到内存中，然后在内存中进行类型转换
                    var projects = context.Projects
                        .Where(p => p.Year >= _startyear && p.Year <= _endyear)
                        .Select(p => new { p.Year, p.ActualExpenditure })
                        .AsEnumerable() // 切换到内存中操作
                        .ToList();

                    var yearlyData = projects
                        .GroupBy(p => p.Year)
                        .Select(g => new
                        {
                            Year = g.Key,
                            ProjectCount = g.Count(),
                            TotalExpenditure = g.Sum(p => 
                                double.TryParse(p.ActualExpenditure, out double expenditure) ? expenditure : 0)
                        })
                        .OrderBy(x => x.Year)
                        .ToList();

                    // 确保包含所有年份，即使没有数据
                    var allYearsData = new List<(int Year, int ProjectCount, double TotalExpenditure)>();
                    for (int year = _startyear; year <= _endyear; year++)
                    {
                        var yearData = yearlyData.FirstOrDefault(y => y.Year == year);
                        allYearsData.Add((year, yearData?.ProjectCount ?? 0, yearData?.TotalExpenditure ?? 0));
                    }

                    // 创建项目数量系列
                    var projectCountPoints = allYearsData.Select(d => 
                        new ObservablePoint(d.Year, d.ProjectCount)).ToArray();

                    // 创建支出总和系列
                    var expenditurePoints = allYearsData.Select(d => 
                        new ObservablePoint(d.Year, d.TotalExpenditure)).ToArray();

                    Historinvest_quantity = new LineSeries<ObservablePoint>[2]
                    {
                        new LineSeries<ObservablePoint> 
                        { 
                            Name = "项目数量",
                            Values = projectCountPoints,
                            Fill = new SolidColorPaint(new SKColor(76, 172, 250))
                        },
                        new LineSeries<ObservablePoint> 
                        { 
                            Name = "支出总和",
                            Values = expenditurePoints,
                            Fill = new SolidColorPaint(new SKColor(109, 203, 112))
                        }
                    };
                }
            }
        }



        #endregion


        #region 项目支出
        [ObservableProperty]
        private ISeries[] _projectExpendituresseries;

        [ObservableProperty]
        private Axis[] _projectExpendituresXAxes;

        /// <summary>
        /// 获取年度预算
        /// </summary>
        public void GetProjectExpenditures()
        {
            using (var context = new ProjectContext())
            {
                var projectExpenditures = context.Projects
                    .Where(p => p.Year >= _startyear && p.Year <= _endyear)
                    .Select(p => new { p.ProjectsId, p.ProjectName, p.Year, p.Budget, p.ActualExpenditure })
                    .GroupBy(p => p.Year)
                    .Select(g => new
                    {
                        Year = g.Key,
                        Projects = g.OrderBy(p => p.ProjectsId).ToList()
                    })
                    .OrderBy(x => x.Year)
                    .ToList();

                List<(double, string, double, double , double)> dict1 = new List<(double, string , double, double, double)>();

                foreach (var yeargroup in projectExpenditures)
                {
                    foreach (var item in yeargroup.Projects)
                    {
                        int id = item.ProjectsId;
                        string name = item.ProjectName;
                        int year = item.Year.GetValueOrDefault();
                        double Budget = 0;
                        double ActualExpenditure = 0;

                        if (double.TryParse(item.ActualExpenditure, out double expenditure))
                        {
                            ActualExpenditure = expenditure;
                        }

                        if (double.TryParse(item.Budget, out double expenditure1))
                        {
                            Budget = expenditure1;
                        }
                        dict1.Add((id , name , year , Budget , ActualExpenditure));

                    }
                }


                List<double> projectBudget = new();
                List<double> projectActualExpenditure = new();
                List<string> projectname = new();

                foreach (var item in dict1)
                {
                    projectBudget.Add(item.Item4);
                    projectActualExpenditure.Add(item.Item5);
                    projectname.Add(item.Item3.ToString() + item.Item2);
                }
                ////Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                ////{
                ProjectExpendituresseries =
                [
                    new ColumnSeries<double>
                    {
                        Name = "预算金额",
                        Values = projectBudget,
                        MaxBarWidth = 30,
                        Fill = new SolidColorPaint(new SKColor(76, 172, 250)),
                    },

                    new ColumnSeries<double>
                    {
                        Name = "实际支出",
                        Values = projectActualExpenditure,
                        MaxBarWidth = 30,
                        Fill = new SolidColorPaint(new SKColor(109, 203, 112)),
                    }

                ];

                ProjectExpendituresXAxes =
                [
                    new Axis
                    {
                        Labels = projectname,
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
        #endregion

        #region 项目进度
        
        
        [ObservableProperty]
        private IEnumerable<ISeries> _projectprogressSeries;

        [ObservableProperty]
        private ISeries[] _annualprojectnumSeries;

        [ObservableProperty]
        private ISeries[] _hisannualprojectnumSeries;
        public void GetProjectProgressSeries()
        {
            using (var context = new ProjectContext())
            {
                var ProjectNum = context.Projects
                    .Where(p => p.Year >= _startyear && p.Year <= _endyear)
                    .ToList();

                var ProjectPhaseStatus = context.ProjectPhaseStatus
                    .OrderBy(p => p.ProjectPhaseStatusId)
                    .ToList();

                List<(string, int)> projectstatusnum = new List<(string, int)>();
                foreach (var projectstatus in ProjectPhaseStatus)
                {
                    var num = ProjectNum
                        .Where(p => p.ProjectPhaseStatusId == projectstatus.ProjectPhaseStatusId)
                        .Count();
                    projectstatusnum.Add((projectstatus.ProjectPhaseStatusName , num));
                }
                List<string> _names = new ();
                List<int> values = new ();

                foreach (var data in projectstatusnum)
                {
                    _names.Add(data.Item1);
                    values.Add(data.Item2);
                }
                int _index = 0;

                ProjectprogressSeries = 
                    values.AsPieSeries((value, series) =>
                    {
                        series.Name = _names[_index++ % _names.Count];
                        //if (value != 6) return;

                        series.Pushout = 5;
                    });

            }
        }

        public void GetannualprojectnumSeries()
        {
            using (var context = new ProjectContext())
            {
                // 获取所有项目状态
                var projectStatuses = context.ProjectPhaseStatus
                    .OrderBy(p => p.ProjectPhaseStatusId)
                    .ToList();

                // 获取指定年份范围内的所有项目
                var allProjects = context.Projects
                    .Where(p => p.Year >= 2022)
                    .ToList();

                // 创建折线图系列数组，每个状态对应一条折线
                var lineSeriesList = new List<LineSeries<ObservablePoint>>();

                // 为每个项目状态创建一条折线
                foreach (var status in projectStatuses)
                {
                    // 按年份统计该状态的项目数量
                    var yearlyData = new List<(int Year, int Count)>();
                    
                    for (int year = 2022; year <= DateTime.Now.Year; year++)
                    {
                        var count = allProjects
                            .Where(p => p.Year == year && p.ProjectPhaseStatusId == status.ProjectPhaseStatusId)
                            .Count();
                        yearlyData.Add((year, count));
                    }

                    // 创建数据点
                    var dataPoints = yearlyData.Select(d => 
                        new ObservablePoint(d.Year, d.Count)).ToArray();

                    // 创建折线系列
                    var lineSeries = new LineSeries<ObservablePoint>
                    {
                        Name = status.ProjectPhaseStatusName,
                        Values = dataPoints,
                        GeometrySize = 8,
                        LineSmoothness = 0.5
                    };

                    lineSeriesList.Add(lineSeries);
                }

                // 将折线系列数组赋值给属性
                AnnualprojectnumSeries = lineSeriesList.ToArray();
            }
        }

        public void GetHisannualprojectnumSeries()
        {

        }

        // public IEnumerable<ISeries> Series { get; set; } =
        //     new[] { 6, 5, 4, 3, 2 }.AsPieSeries((value, series) =>
        //     {
        //         // pushes out the slice with the value of 6 to 30 pixels.
        //         if (value != 6) return;
        //
        //         series.Pushout = 30;
        //     });

        #endregion

        #region 个人项目情况
        [ObservableProperty]
        private IEnumerable<ISeries> _projectleaderinformation;

        [ObservableProperty]
        private IEnumerable<ISeries> _projectfollowpoepleinformation;

        [ObservableProperty]
        private IEnumerable<ISeries> _personalProjectStatusSeries;

        /// <summary>
        /// 获取个人项目状态数据（饼图）
        /// </summary>
        public void GetPersonalProjectStatusSeries()
        {
            if (SelectedEmployee == null) return;

            using (var context = new ProjectContext())
            {
                // 获取当前员工的所有项目
                var projects = context.Projects
                    .Where(p => p.ProjectLeaderId == SelectedEmployee.PeopleId && 
                               p.Year >= _startyear && p.Year <= _endyear)
                    .Include(p => p.ProjectPhaseStatus)
                    .ToList();

                // 按状态分组统计
                var statusGroups = projects
                    .GroupBy(p => p.ProjectPhaseStatus?.ProjectPhaseStatusName ?? "未知状态")
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToList();

                // 创建饼图数据
                var values = statusGroups.Select(g => (double)g.Count).ToList();
                var labels = statusGroups.Select(g => g.Status).ToList();

                int index = 0;
                PersonalProjectStatusSeries = values.AsPieSeries((value, series) =>
                {
                    series.Name = labels[index++ % labels.Count];
                    series.Pushout = 5;
                });
            }
        }

        #region 个人项目状态年度趋势折线图
        [ObservableProperty]
        private ISeries[] _personalProjectStatusLineSeries;

        /// <summary>
        /// 获取个人项目状态年度趋势数据（折线图）
        /// </summary>
        public void GetPersonalProjectStatusLineSeries()
        {
            if (SelectedEmployee == null) return;

            using (var context = new ProjectContext())
            {
                var statuses = context.ProjectPhaseStatus.ToList();
                
                // 创建按年份统计的数据
                var seriesList = new List<ISeries>();
                
                // 为每个状态创建一条折线
                foreach (var status in statuses)
                {
                    var yearlyData = new List<double>();
                    
                    // 遍历年份范围
                    for (int year = _startyear; year <= _endyear; year++)
                    {
                        var count = context.Projects
                            .Where(p => p.Year == year && 
                                       p.ProjectLeaderId == SelectedEmployee.PeopleId &&
                                       p.ProjectPhaseStatusId == status.ProjectPhaseStatusId)
                            .Count();
                        
                        yearlyData.Add(count);
                    }
                    
                    if (yearlyData.Any(d => d > 0))
                    {
                        var lineSeries = new LineSeries<double>
                        {
                            Name = status.ProjectPhaseStatusName,
                            Values = yearlyData,
                            Stroke = new SolidColorPaint(GetStatusColor(status.ProjectPhaseStatusId), 3),
                            Fill = null,
                            GeometrySize = 8,
                            GeometryStroke = new SolidColorPaint(GetStatusColor(status.ProjectPhaseStatusId), 2),
                            GeometryFill = new SolidColorPaint(SKColors.White)
                        };
                        
                        seriesList.Add(lineSeries);
                    }
                }
                
                PersonalProjectStatusLineSeries = seriesList.ToArray();
            }
        }

        private SKColor GetStatusColor(int statusId)
        {
            return statusId switch
            {
                101 => new SKColor(255, 193, 7),    // 未启动 - 黄色
                102 => new SKColor(33, 150, 243),   // 进行中 - 蓝色
                103 => new SKColor(255, 152, 0),    // 暂停 - 橙色
                104 => new SKColor(76, 175, 80),    // 已完成 - 绿色
                _ => new SKColor(158, 158, 158)     // 默认灰色
            };
        }
        #endregion

        #region 项目负责人年度项目汇总柱状图
        [ObservableProperty]
        private ISeries[] _projectLeaderAnnualSummarySeries;

        [ObservableProperty]
        private Axis[] _projectLeaderSummaryXAxes;

        /// <summary>
        /// 获取项目负责人年度项目汇总数据（柱状图）
        /// </summary>
        public void GetProjectLeaderAnnualSummary()
        {
            using (var context = new ProjectContext())
            {
                // 获取当前年份
                int currentYear = DateTime.Now.Year;
                
                // 获取所有项目负责人
                var projectLeaders = context.Projects
                    .Where(p => p.Year == currentYear && p.ProjectLeaderId != null)
                    .Include(p => p.ProjectLeader)
                    .Select(p => p.ProjectLeader)
                    .Distinct()
                    .ToList();

                // 统计每个负责人的项目状态数量
                var leaderStats = new List<LeaderProjectStats>();
                
                foreach (var leader in projectLeaders)
                {
                    var projects = context.Projects
                        .Where(p => p.Year == currentYear && p.ProjectLeaderId == leader.PeopleId)
                        .ToList();

                    var stats = new LeaderProjectStats
                    {
                        LeaderName = leader.PeopleName,
                        TotalCount = projects.Count,
                        CompletedCount = projects.Count(p => p.ProjectPhaseStatusId == 104),
                        PausedCount = projects.Count(p => p.ProjectPhaseStatusId == 103),
                        InProgressCount = projects.Count(p => p.ProjectPhaseStatusId == 102),
                        NotStartedCount = projects.Count(p => p.ProjectPhaseStatusId == 101)
                    };
                    
                    leaderStats.Add(stats);
                }

                // 按总项目数排序
                leaderStats = leaderStats.OrderByDescending(s => s.TotalCount).ToList();

                // 创建柱状图系列
                var seriesList = new List<ISeries>();
                
                // 已完成项目系列
                var completedSeries = new ColumnSeries<double>
                {
                    Name = "已完成",
                    Values = leaderStats.Select(s => (double)s.CompletedCount).ToArray(),
                    Stroke = new SolidColorPaint(SKColors.Black, 1),
                    Fill = new SolidColorPaint(new SKColor(76, 175, 80)), // 绿色
                    MaxBarWidth = 25
                };
                
                // 进行中项目系列
                var inProgressSeries = new ColumnSeries<double>
                {
                    Name = "进行中",
                    Values = leaderStats.Select(s => (double)s.InProgressCount).ToArray(),
                    Stroke = new SolidColorPaint(SKColors.Black, 1),
                    Fill = new SolidColorPaint(new SKColor(33, 150, 243)), // 蓝色
                    MaxBarWidth = 25
                };
                
                // 暂停项目系列
                var pausedSeries = new ColumnSeries<double>
                {
                    Name = "暂停",
                    Values = leaderStats.Select(s => (double)s.PausedCount).ToArray(),
                    Stroke = new SolidColorPaint(SKColors.Black, 1),
                    Fill = new SolidColorPaint(new SKColor(255, 152, 0)), // 橙色
                    MaxBarWidth = 25
                };
                
                // 未启动项目系列
                var notStartedSeries = new ColumnSeries<double>
                {
                    Name = "未启动",
                    Values = leaderStats.Select(s => (double)s.NotStartedCount).ToArray(),
                    Stroke = new SolidColorPaint(SKColors.Black, 1),
                    Fill = new SolidColorPaint(new SKColor(255, 193, 7)), // 黄色
                    MaxBarWidth = 25
                };

                seriesList.Add(completedSeries);
                seriesList.Add(inProgressSeries);
                seriesList.Add(pausedSeries);
                seriesList.Add(notStartedSeries);

                ProjectLeaderAnnualSummarySeries = seriesList.ToArray();

                // 设置X轴标签
                var xAxisLabels = leaderStats.Select(s => s.LeaderName).ToArray();
                ProjectLeaderSummaryXAxes = [
                    new Axis
                    {
                        Labels = xAxisLabels,
                        LabelsRotation = 45,
                        TextSize = 12,
                        LabelsPaint = LegendTextPaint
                    }
                ];
            }
        }

        // 项目负责人统计数据结构
        private class LeaderProjectStats
        {
            public string LeaderName { get; set; }
            public int TotalCount { get; set; }
            public int CompletedCount { get; set; }
            public int InProgressCount { get; set; }
            public int PausedCount { get; set; }
            public int NotStartedCount { get; set; }
        }
        #endregion


        private void GetPeopleInformation()
        {
            using (var context = new ProjectContext())
            {
                var ProjectleaderNum = context.Projects
                    .Where(p => p.Year >= _startyear && p.Year <= _endyear)
                    .Include(p => p.ProjectLeader)
                    .GroupBy(p => p.ProjectLeader.PeopleName)
                    .ToList();

                var ProjectFollowpeopleNum = context.Projects
                    .Where(p => p.Year >= _startyear && p.Year <= _endyear)
                    .Include(p => p.projectfollowupperson)
                    .GroupBy(p => p.projectfollowupperson.PeopleName)
                    .ToList();

                Dictionary<string, double> Leaderinfor = new Dictionary<string, double>();
                Dictionary<string, double> FollowInfor = new Dictionary<string, double>();

                foreach (var leader in ProjectleaderNum)
                {
                    if (leader.Key != null)
                    {
                        Leaderinfor.Add(leader.Key, leader.Count());
                    }
                    // 如果为空怎么办

                }

                foreach (var followpeople in ProjectFollowpeopleNum)
                {
                    if (followpeople.Key != null)
                    {
                        FollowInfor.Add(followpeople.Key, followpeople.Count());
                    }
                    // 如果为空怎么办
                }

                int _index_leader = 0;
                Projectleaderinformation =
                    Leaderinfor.Values.ToList().AsPieSeries((value, series) =>
                    {
                        series.Name = Leaderinfor.Keys.ToList()[_index_leader++ % Leaderinfor.Keys.ToList().Count];
                        //if (value != 6) return;

                        series.Pushout = 5;
                    });

                int _index_follow = 0;
                Projectfollowpoepleinformation =
                    FollowInfor.Values.ToList().AsPieSeries((value, series) =>
                    {
                        series.Name = FollowInfor.Keys.ToList()[_index_follow++ % FollowInfor.Keys.ToList().Count];
                        //if (value != 6) return;

                        series.Pushout = 5;
                    });
            }
        }
        #endregion



        public ChartsVM(int startyear , int stopyear)
        {
            _startyear = startyear;
            _endyear = stopyear;
            LoadEmployees();



            Task.Run(() =>
            {
                GetProjectBudget_Pie();
            });

            
            //
            Task.Run(() =>
            {
                //GetProjectExpenditures();
                Historinvest_Amount();
            });

            Task.Run(() =>
            {
                Historinvest_Quantity();
            });



            //
            Task.Run(() =>
            {
                GetProjectProgressSeries();
            });

            Task.Run(() =>
            {
                GetPeopleInformation();
                GetannualprojectnumSeries();
            });

            // 新增：加载项目负责人年度项目汇总柱状图数据
            Task.Run(() =>
            {
                GetProjectLeaderAnnualSummary();
            });
        }





        #region 责任人下拉框

        private ObservableCollection<PeopleTable> _employees;
        private PeopleTable _selectedEmployee;
        private string _statusMessage;

        // 员工列表 - 用于下拉框
        public ObservableCollection<PeopleTable> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged();
            }
        }

        // 选中的员工
        public PeopleTable SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged();
                UpdateStatusMessage();
                // 当选中员工改变时，重新加载个人项目状态数据
                if (value != null)
                {
                    Task.Run(() =>
                    {
                        GetPersonalProjectStatusSeries();
                        GetPersonalProjectStatusLineSeries();
                    });
                }
            }
        }

        // 状态信息
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        // 加载员工数据
        private void LoadEmployees()
        {
            try
            {
                using (var context = new ProjectContext())
                {
                    var employees = context.PeopleTable
                        .OrderBy(e => e.PeopleName)
                        .ToList();

                    Employees = new ObservableCollection<PeopleTable>(employees);

                    if (Employees.Count > 0)
                    {
                        //SelectedFollowEmployee = Employees[0];
                        SelectedEmployee = null;
                    }

                    SelectedEmployee = Employees[0];
                }


            }
            catch (Exception ex)
            {

            }
        }

        // 更新状态信息
        private void UpdateStatusMessage()
        {
            if (SelectedEmployee != null)
            {
                //StatusMessage = $"选中: {SelectedEmployee.PeopleName} ({SelectedEmployee.PeopleId})";
                //ProjectsLeaderID = SelectedEmployee.PeopleId;
            }
        }


        #endregion

    }
}