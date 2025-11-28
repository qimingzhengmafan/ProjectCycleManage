using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectCycleManage.ViewModel
{
    public partial class FinancialData : ObservableObject
    {
        [ObservableProperty]
        private string currentYear = "2024";

        [ObservableProperty]
        private List<string> availableYears;

        [ObservableProperty]
        private List<QuarterlyData> quarterlyDataList;

        [ObservableProperty]
        private double annualSales = 8560000;

        [ObservableProperty]
        private double annualBudget = 7200000;

        [ObservableProperty]
        private double budgetExecutionRate = 118.9;

        [ObservableProperty]
        private string budgetApprover = "财务部 - 王总监";

        [ObservableProperty]
        private string lastUpdateTime = "2024-03-15";

        [ObservableProperty]
        private string budgetStatus = "已审批";

        [ObservableProperty]
        private int relatedProjectsCount = 24;

        [ObservableProperty]
        private bool isMonthlyDataVisible = false;

        [ObservableProperty]
        private List<MonthlyData> monthlyDataList;

        public FinancialData()
        {
            InitializeAvailableYears();
            InitializeQuarterlyData();
            InitializeMonthlyData();
        }

        private void InitializeAvailableYears()
        {
            int currentYear = DateTime.Now.Year;
            int startYear = 2022;
            
            AvailableYears = new List<string>();
            for (int year = startYear; year <= currentYear; year++)
            {
                AvailableYears.Add(year.ToString());
            }
            
            // 设置当前年份为最新的年份
            CurrentYear = currentYear.ToString();
        }

        private void InitializeQuarterlyData()
        {
            QuarterlyDataList = new List<QuarterlyData>
            {
                new QuarterlyData { Quarter = "第一季度", Sales = "¥ 1,850,000", Budget = "¥ 1,800,000", CompletionRate = "102.8%", Status = "超额完成" },
                new QuarterlyData { Quarter = "第二季度", Sales = "¥ 2,150,000", Budget = "¥ 1,800,000", CompletionRate = "119.4%", Status = "超额完成" },
                new QuarterlyData { Quarter = "第三季度", Sales = "¥ 2,380,000", Budget = "¥ 1,800,000", CompletionRate = "132.2%", Status = "超额完成" },
                new QuarterlyData { Quarter = "第四季度", Sales = "¥ 2,180,000", Budget = "¥ 1,800,000", CompletionRate = "121.1%", Status = "超额完成" }
            };
        }

        [RelayCommand]
        private void SelectYear(string year)
        {
            CurrentYear = year;
            // 根据选择的年份加载数据
            LoadFinancialDataByYear(year);
        }

        private void LoadFinancialDataByYear(string year)
        {
            // 根据年份加载不同的财务数据
            switch (year)
            {
                case "2022":
                    AnnualSales = 7200000;
                    AnnualBudget = 6000000;
                    BudgetExecutionRate = 120.0;
                    InitializeQuarterlyDataFor2022();
                    InitializeMonthlyDataFor2022();
                    break;
                case "2023":
                    AnnualSales = 7800000;
                    AnnualBudget = 6500000;
                    BudgetExecutionRate = 120.0;
                    InitializeQuarterlyDataFor2023();
                    InitializeMonthlyDataFor2023();
                    break;
                case "2024":
                default:
                    AnnualSales = 8560000;
                    AnnualBudget = 7200000;
                    BudgetExecutionRate = 118.9;
                    InitializeQuarterlyData();
                    InitializeMonthlyData();
                    break;
            }
            
            // 通知UI属性已更新
            OnPropertyChanged(nameof(AnnualSales));
            OnPropertyChanged(nameof(AnnualBudget));
            OnPropertyChanged(nameof(BudgetExecutionRate));
        }

        [RelayCommand]
        private void ExportData()
        {
            // 导出数据逻辑
        }

        [RelayCommand]
        private void EditBudget()
        {
            // 编辑预算逻辑
        }

        [RelayCommand]
        private void BackToSettings()
        {
            // 返回系统设置逻辑
        }

        [RelayCommand]
        private void AddInformation()
        {
            // 添加信息逻辑
        }

        [RelayCommand]
        private void ToggleMonthlyData()
        {
            IsMonthlyDataVisible = !IsMonthlyDataVisible;
        }

        private void InitializeMonthlyData()
        {
            MonthlyDataList = new List<MonthlyData>
            {
                new MonthlyData { Month = "一月", Quantity = 12, SalesForecast = 750000, Budget = 650000 },
                new MonthlyData { Month = "二月", Quantity = 14, SalesForecast = 820000, Budget = 700000 },
                new MonthlyData { Month = "三月", Quantity = 13, SalesForecast = 780000, Budget = 680000 },
                new MonthlyData { Month = "四月", Quantity = 15, SalesForecast = 850000, Budget = 720000 },
                new MonthlyData { Month = "五月", Quantity = 16, SalesForecast = 900000, Budget = 780000 },
                new MonthlyData { Month = "六月", Quantity = 14, SalesForecast = 820000, Budget = 700000 },
                new MonthlyData { Month = "七月", Quantity = 17, SalesForecast = 950000, Budget = 800000 },
                new MonthlyData { Month = "八月", Quantity = 15, SalesForecast = 850000, Budget = 720000 },
                new MonthlyData { Month = "九月", Quantity = 18, SalesForecast = 980000, Budget = 820000 },
                new MonthlyData { Month = "十月", Quantity = 16, SalesForecast = 900000, Budget = 780000 },
                new MonthlyData { Month = "十一月", Quantity = 14, SalesForecast = 820000, Budget = 700000 },
                new MonthlyData { Month = "十二月", Quantity = 17, SalesForecast = 950000, Budget = 800000 }
            };
        }

        private void InitializeQuarterlyDataFor2022()
        {
            QuarterlyDataList = new List<QuarterlyData>
            {
                new QuarterlyData { Quarter = "第一季度", Sales = "¥ 1,550,000", Budget = "¥ 1,500,000", CompletionRate = "103.3%", Status = "超额完成" },
                new QuarterlyData { Quarter = "第二季度", Sales = "¥ 1,750,000", Budget = "¥ 1,500,000", CompletionRate = "116.7%", Status = "超额完成" },
                new QuarterlyData { Quarter = "第三季度", Sales = "¥ 1,980,000", Budget = "¥ 1,500,000", CompletionRate = "132.0%", Status = "超额完成" },
                new QuarterlyData { Quarter = "第四季度", Sales = "¥ 1,920,000", Budget = "¥ 1,500,000", CompletionRate = "128.0%", Status = "超额完成" }
            };
        }

        private void InitializeMonthlyDataFor2022()
        {
            MonthlyDataList = new List<MonthlyData>
            {
                new MonthlyData { Month = "一月", Quantity = 10, SalesForecast = 650000, Budget = 550000 },
                new MonthlyData { Month = "二月", Quantity = 11, SalesForecast = 680000, Budget = 580000 },
                new MonthlyData { Month = "三月", Quantity = 12, SalesForecast = 720000, Budget = 610000 },
                new MonthlyData { Month = "四月", Quantity = 13, SalesForecast = 750000, Budget = 640000 },
                new MonthlyData { Month = "五月", Quantity = 14, SalesForecast = 780000, Budget = 670000 },
                new MonthlyData { Month = "六月", Quantity = 15, SalesForecast = 820000, Budget = 700000 },
                new MonthlyData { Month = "七月", Quantity = 16, SalesForecast = 850000, Budget = 730000 },
                new MonthlyData { Month = "八月", Quantity = 15, SalesForecast = 820000, Budget = 700000 },
                new MonthlyData { Month = "九月", Quantity = 14, SalesForecast = 780000, Budget = 670000 },
                new MonthlyData { Month = "十月", Quantity = 13, SalesForecast = 750000, Budget = 640000 },
                new MonthlyData { Month = "十一月", Quantity = 12, SalesForecast = 720000, Budget = 610000 },
                new MonthlyData { Month = "十二月", Quantity = 11, SalesForecast = 680000, Budget = 580000 }
            };
        }

        private void InitializeQuarterlyDataFor2023()
        {
            QuarterlyDataList = new List<QuarterlyData>
            {
                new QuarterlyData { Quarter = "第一季度", Sales = "¥ 1,680,000", Budget = "¥ 1,625,000", CompletionRate = "103.4%", Status = "超额完成" },
                new QuarterlyData { Quarter = "第二季度", Sales = "¥ 1,920,000", Budget = "¥ 1,625,000", CompletionRate = "118.2%", Status = "超额完成" },
                new QuarterlyData { Quarter = "第三季度", Sales = "¥ 2,150,000", Budget = "¥ 1,625,000", CompletionRate = "132.3%", Status = "超额完成" },
                new QuarterlyData { Quarter = "第四季度", Sales = "¥ 2,050,000", Budget = "¥ 1,625,000", CompletionRate = "126.2%", Status = "超额完成" }
            };
        }

        private void InitializeMonthlyDataFor2023()
        {
            MonthlyDataList = new List<MonthlyData>
            {
                new MonthlyData { Month = "一月", Quantity = 11, SalesForecast = 680000, Budget = 580000 },
                new MonthlyData { Month = "二月", Quantity = 12, SalesForecast = 720000, Budget = 620000 },
                new MonthlyData { Month = "三月", Quantity = 13, SalesForecast = 750000, Budget = 650000 },
                new MonthlyData { Month = "四月", Quantity = 14, SalesForecast = 780000, Budget = 680000 },
                new MonthlyData { Month = "五月", Quantity = 15, SalesForecast = 820000, Budget = 710000 },
                new MonthlyData { Month = "六月", Quantity = 16, SalesForecast = 850000, Budget = 740000 },
                new MonthlyData { Month = "七月", Quantity = 17, SalesForecast = 880000, Budget = 770000 },
                new MonthlyData { Month = "八月", Quantity = 16, SalesForecast = 850000, Budget = 740000 },
                new MonthlyData { Month = "九月", Quantity = 15, SalesForecast = 820000, Budget = 710000 },
                new MonthlyData { Month = "十月", Quantity = 14, SalesForecast = 780000, Budget = 680000 },
                new MonthlyData { Month = "十一月", Quantity = 13, SalesForecast = 750000, Budget = 650000 },
                new MonthlyData { Month = "十二月", Quantity = 12, SalesForecast = 720000, Budget = 620000 }
            };
        }
    }

    public class QuarterlyData
    {
        public string Quarter { get; set; }
        public string Sales { get; set; }
        public string Budget { get; set; }
        public string CompletionRate { get; set; }
        public string Status { get; set; }
    }

    public class MonthlyData
    {
        public string Month { get; set; }
        public int Quantity { get; set; }
        public double SalesForecast { get; set; }
        public double Budget { get; set; }
    }
}
