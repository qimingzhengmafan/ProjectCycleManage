using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectManagement.Data;
using ProjectCycleManage.Model;
using ProjectManagement.Models;
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
        int startYear = 2022;

        [ObservableProperty]
        private string currentYear;

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

        [ObservableProperty]
        private int investmentQuantity = 156;

        [ObservableProperty]
        private double investmentSalesForecast = 9560000;

        [ObservableProperty]
        private double investmentBudget = 8450000;

        public FinancialData()
        {
            InitializeAvailableYears();
            LoadFinancialDataByYear(CurrentYear);
        }

        private void InitializeAvailableYears()
        {
            int currentYear = DateTime.Now.Year;

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
            if (!int.TryParse(year, out int selectedYear))
                return;

            using var context = new ProjectContext();
            
            // 1. 从SalesVolume表获取年度销售预测
            var salesVolume = context.SalesVolumeTables
                .FirstOrDefault(s => s.Year == selectedYear);
            
            if (salesVolume != null)
            {
                InvestmentSalesForecast = salesVolume.SalesVolume;
            }
            
            // 2. 从AnnualBudget表获取年度预算
            var annualBudget = context.AnnualBudgetTable
                .FirstOrDefault(a => a.Year == selectedYear);
            
            if (annualBudget != null)
            {
                InvestmentBudget = annualBudget.Budget;
            }
            
            // 3. 从Projects表获取投入设备数量和投入金额
            var projects = context.Projects
                .Where(p => p.Year == selectedYear)
                .ToList();
            
            // 计算投入设备数量（总设备数）
            InvestmentQuantity = projects.Count;
            
            // 计算投入金额（所有设备的ActualExpenditure总和）
            double totalExpenditure = 0;
            foreach (var project in projects)
            {
                if (double.TryParse(project.ActualExpenditure, out double expenditure))
                {
                    totalExpenditure += expenditure;
                }
            }
            AnnualSales = totalExpenditure;
            
            // 4. 计算预算执行率
            if (InvestmentBudget > 0)
            {
                BudgetExecutionRate = Math.Round((AnnualSales / InvestmentBudget) * 100, 1);
            }
            else
            {
                BudgetExecutionRate = 0;
            }
            
            // 5. 加载月度数据修正
            LoadMonthlyCorrectionData(selectedYear);
            
            // 通知UI属性已更新
            

            OnPropertyChanged(nameof(AnnualBudget));
            OnPropertyChanged(nameof(InvestmentQuantity));

            OnPropertyChanged(nameof(AnnualSales));
            OnPropertyChanged(nameof(BudgetExecutionRate));
            OnPropertyChanged(nameof(InvestmentSalesForecast));
            OnPropertyChanged(nameof(InvestmentBudget));
        }

        private void LoadMonthlyCorrectionData(int year)
        {
            using var context = new ProjectContext();
            
            // 获取资产数量修正数据
            var quantityCorrection = context.RevOfAssetQuantTab
                .FirstOrDefault(r => r.Year == year);
            
            // 获取资产金额修正数据
            var amountCorrection = context.AsAmountCorrectTab
                .FirstOrDefault(a => a.Year == year);
            
            // 初始化月度数据列表
            MonthlyDataList = new List<MonthlyData>();
            
            // 定义月份名称数组
            string[] monthNames = { "一月", "二月", "三月", "四月", "五月", "六月", 
                                  "七月", "八月", "九月", "十月", "十一月", "十二月" };
            
            // 获取原始值（投入设备数量和投入金额）
            double originalQuantity = InvestmentQuantity;
            double originalAmount = AnnualSales;
            
            for (int i = 0; i < 12; i++)
            {
                var monthlyData = new MonthlyData
                {
                    Month = monthNames[i],
                    OriginalQuantity = originalQuantity,
                    OriginalAmount = originalAmount
                };
                
                // 计算修正后的数量
                double quantityCorrectionValue = GetCorrectionValue(quantityCorrection, i + 1);
                monthlyData.CorrectedQuantity = originalQuantity + quantityCorrectionValue;
                
                // 计算修正后的金额
                double amountCorrectionValue = GetCorrectionValue(amountCorrection, i + 1);
                monthlyData.CorrectedAmount = originalAmount + amountCorrectionValue;
                
                MonthlyDataList.Add(monthlyData);
            }
            
            OnPropertyChanged(nameof(MonthlyDataList));
        }

        private double GetCorrectionValue(object correctionRecord, int month)
        {
            if (correctionRecord == null)
                return 0;
            
            switch (month)
            {
                case 1: return ((dynamic)correctionRecord).JanuaryCorrection ?? 0;
                case 2: return ((dynamic)correctionRecord).FebruaryCorrection ?? 0;
                case 3: return ((dynamic)correctionRecord).MarchCorrection ?? 0;
                case 4: return ((dynamic)correctionRecord).AprilCorrection ?? 0;
                case 5: return ((dynamic)correctionRecord).MayCorrection ?? 0;
                case 6: return ((dynamic)correctionRecord).JuneCorrection ?? 0;
                case 7: return ((dynamic)correctionRecord).JulyCorrection ?? 0;
                case 8: return ((dynamic)correctionRecord).AugustCorrection ?? 0;
                case 9: return ((dynamic)correctionRecord).SeptemberCorrection ?? 0;
                case 10: return ((dynamic)correctionRecord).OctoberCorrection ?? 0;
                case 11: return ((dynamic)correctionRecord).NovemberCorrection ?? 0;
                case 12: return ((dynamic)correctionRecord).DecemberCorrection ?? 0;
                default: return 0;
            }
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
                new MonthlyData { Month = "一月", OriginalQuantity = 12, OriginalAmount = 750000, CorrectedQuantity = 12, CorrectedAmount = 750000 },
                new MonthlyData { Month = "二月", OriginalQuantity = 14, OriginalAmount = 820000, CorrectedQuantity = 14, CorrectedAmount = 820000 },
                new MonthlyData { Month = "三月", OriginalQuantity = 13, OriginalAmount = 780000, CorrectedQuantity = 13, CorrectedAmount = 780000 },
                new MonthlyData { Month = "四月", OriginalQuantity = 15, OriginalAmount = 850000, CorrectedQuantity = 15, CorrectedAmount = 850000 },
                new MonthlyData { Month = "五月", OriginalQuantity = 16, OriginalAmount = 900000, CorrectedQuantity = 16, CorrectedAmount = 900000 },
                new MonthlyData { Month = "六月", OriginalQuantity = 14, OriginalAmount = 820000, CorrectedQuantity = 14, CorrectedAmount = 820000 },
                new MonthlyData { Month = "七月", OriginalQuantity = 17, OriginalAmount = 950000, CorrectedQuantity = 17, CorrectedAmount = 950000 },
                new MonthlyData { Month = "八月", OriginalQuantity = 15, OriginalAmount = 850000, CorrectedQuantity = 15, CorrectedAmount = 850000 },
                new MonthlyData { Month = "九月", OriginalQuantity = 18, OriginalAmount = 980000, CorrectedQuantity = 18, CorrectedAmount = 980000 },
                new MonthlyData { Month = "十月", OriginalQuantity = 16, OriginalAmount = 900000, CorrectedQuantity = 16, CorrectedAmount = 900000 },
                new MonthlyData { Month = "十一月", OriginalQuantity = 14, OriginalAmount = 820000, CorrectedQuantity = 14, CorrectedAmount = 820000 },
                new MonthlyData { Month = "十二月", OriginalQuantity = 17, OriginalAmount = 950000, CorrectedQuantity = 17, CorrectedAmount = 950000 }
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
                new MonthlyData { Month = "一月", OriginalQuantity = 10, OriginalAmount = 650000, CorrectedQuantity = 10, CorrectedAmount = 650000 },
                new MonthlyData { Month = "二月", OriginalQuantity = 11, OriginalAmount = 680000, CorrectedQuantity = 11, CorrectedAmount = 680000 },
                new MonthlyData { Month = "三月", OriginalQuantity = 12, OriginalAmount = 720000, CorrectedQuantity = 12, CorrectedAmount = 720000 },
                new MonthlyData { Month = "四月", OriginalQuantity = 13, OriginalAmount = 750000, CorrectedQuantity = 13, CorrectedAmount = 750000 },
                new MonthlyData { Month = "五月", OriginalQuantity = 14, OriginalAmount = 780000, CorrectedQuantity = 14, CorrectedAmount = 780000 },
                new MonthlyData { Month = "六月", OriginalQuantity = 15, OriginalAmount = 820000, CorrectedQuantity = 15, CorrectedAmount = 820000 },
                new MonthlyData { Month = "七月", OriginalQuantity = 16, OriginalAmount = 850000, CorrectedQuantity = 16, CorrectedAmount = 850000 },
                new MonthlyData { Month = "八月", OriginalQuantity = 15, OriginalAmount = 820000, CorrectedQuantity = 15, CorrectedAmount = 820000 },
                new MonthlyData { Month = "九月", OriginalQuantity = 14, OriginalAmount = 780000, CorrectedQuantity = 14, CorrectedAmount = 780000 },
                new MonthlyData { Month = "十月", OriginalQuantity = 13, OriginalAmount = 750000, CorrectedQuantity = 13, CorrectedAmount = 750000 },
                new MonthlyData { Month = "十一月", OriginalQuantity = 12, OriginalAmount = 720000, CorrectedQuantity = 12, CorrectedAmount = 720000 },
                new MonthlyData { Month = "十二月", OriginalQuantity = 11, OriginalAmount = 680000, CorrectedQuantity = 11, CorrectedAmount = 680000 }
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
                new MonthlyData { Month = "一月", OriginalQuantity = 11, OriginalAmount = 680000, CorrectedQuantity = 11, CorrectedAmount = 680000 },
                new MonthlyData { Month = "二月", OriginalQuantity = 12, OriginalAmount = 720000, CorrectedQuantity = 12, CorrectedAmount = 720000 },
                new MonthlyData { Month = "三月", OriginalQuantity = 13, OriginalAmount = 750000, CorrectedQuantity = 13, CorrectedAmount = 750000 },
                new MonthlyData { Month = "四月", OriginalQuantity = 14, OriginalAmount = 780000, CorrectedQuantity = 14, CorrectedAmount = 780000 },
                new MonthlyData { Month = "五月", OriginalQuantity = 15, OriginalAmount = 820000, CorrectedQuantity = 15, CorrectedAmount = 820000 },
                new MonthlyData { Month = "六月", OriginalQuantity = 16, OriginalAmount = 850000, CorrectedQuantity = 16, CorrectedAmount = 850000 },
                new MonthlyData { Month = "七月", OriginalQuantity = 17, OriginalAmount = 880000, CorrectedQuantity = 17, CorrectedAmount = 880000 },
                new MonthlyData { Month = "八月", OriginalQuantity = 16, OriginalAmount = 850000, CorrectedQuantity = 16, CorrectedAmount = 850000 },
                new MonthlyData { Month = "九月", OriginalQuantity = 15, OriginalAmount = 820000, CorrectedQuantity = 15, CorrectedAmount = 820000 },
                new MonthlyData { Month = "十月", OriginalQuantity = 14, OriginalAmount = 780000, CorrectedQuantity = 14, CorrectedAmount = 780000 },
                new MonthlyData { Month = "十一月", OriginalQuantity = 13, OriginalAmount = 750000, CorrectedQuantity = 13, CorrectedAmount = 750000 },
                new MonthlyData { Month = "十二月", OriginalQuantity = 12, OriginalAmount = 720000, CorrectedQuantity = 12, CorrectedAmount = 720000 }
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
        public double OriginalQuantity { get; set; }
        public double OriginalAmount { get; set; }
        public double CorrectedQuantity { get; set; }
        public double CorrectedAmount { get; set; }
    }
}
