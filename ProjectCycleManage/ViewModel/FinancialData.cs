using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectCycleManage.Model;
using ProjectManagement.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        /// <summary>
        /// 预算执行率
        /// </summary>
        [ObservableProperty]
        private double budgetExecutionRate = 0.0;

        [ObservableProperty]
        private bool isMonthlyDataVisible = false;

        /// <summary>
        /// 月份数据
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<MonthlyData> monthlyDataList;

        /// <summary>
        /// 年度销售预测
        /// </summary>
        [ObservableProperty]
        private double investmentSalesForecast = 0;

        /// <summary>
        /// 年度预算
        /// </summary>
        [ObservableProperty]
        private double investmentBudget = 0;

        /// <summary>
        /// 投入设备数量
        /// </summary>
        [ObservableProperty]
        private int investmentQuantity = 0;

        /// <summary>
        /// 投入金额
        /// </summary>
        [ObservableProperty]
        private double _annualSales = 0;

        /// <summary>
        /// 修正设备数量
        /// </summary>
        [ObservableProperty]
        private double _correctinvestQuant = 0;

        /// <summary>
        /// 修正设备金额
        /// </summary>
        [ObservableProperty]
        private double _correctinvestsales = 0;



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
            
            //OnPropertyChanged(nameof(InvestmentQuantity));

            //OnPropertyChanged(nameof(AnnualSales));
            
            //OnPropertyChanged(nameof(InvestmentSalesForecast));
            //OnPropertyChanged(nameof(InvestmentBudget));

            //OnPropertyChanged(nameof(BudgetExecutionRate));
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
            MonthlyDataList = new ObservableCollection<MonthlyData>();
            
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
                monthlyData.CorrectedQuantity = quantityCorrectionValue;
                
                // 计算修正后的金额
                double amountCorrectionValue = GetCorrectionValue(amountCorrection, i + 1);
                monthlyData.CorrectedAmount = amountCorrectionValue;
                
                MonthlyDataList.Add(monthlyData);
            }

            double Quantitynum = 0;
            double Quantityamount = 0.0;

            foreach (var item in MonthlyDataList)
            {
                Quantitynum = item.CorrectedQuantity + Quantitynum;
                Quantityamount = item.CorrectedAmount + Quantityamount;

            }
            CorrectinvestQuant = Quantitynum + InvestmentQuantity;
            Correctinvestsales = Quantityamount + AnnualSales;
            //OnPropertyChanged(nameof(MonthlyDataList));
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

        private void SetMonthlyCorrectionValue(object correctionRecord, int month, double value)
        {
            if (correctionRecord == null)
                return;
            
            switch (month)
            {
                case 1: ((dynamic)correctionRecord).JanuaryCorrection = value; break;
                case 2: ((dynamic)correctionRecord).FebruaryCorrection = value; break;
                case 3: ((dynamic)correctionRecord).MarchCorrection = value; break;
                case 4: ((dynamic)correctionRecord).AprilCorrection = value; break;
                case 5: ((dynamic)correctionRecord).MayCorrection = value; break;
                case 6: ((dynamic)correctionRecord).JuneCorrection = value; break;
                case 7: ((dynamic)correctionRecord).JulyCorrection = value; break;
                case 8: ((dynamic)correctionRecord).AugustCorrection = value; break;
                case 9: ((dynamic)correctionRecord).SeptemberCorrection = value; break;
                case 10: ((dynamic)correctionRecord).OctoberCorrection = value; break;
                case 11: ((dynamic)correctionRecord).NovemberCorrection = value; break;
                case 12: ((dynamic)correctionRecord).DecemberCorrection = value; break;
            }
        }

        /// <summary>
        /// 验证财务数据
        /// </summary>
        /// <returns>验证是否通过</returns>
        private bool ValidateFinancialData()
        {
            // 验证年度销售预测
            if (InvestmentSalesForecast < 0)
            {
                MessageBox.Show("年度销售预测不能为负数。", "数据验证失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // 验证年度预算
            if (InvestmentBudget < 0)
            {
                MessageBox.Show("年度预算不能为负数。", "数据验证失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // 验证月度数据
            if (MonthlyDataList != null)
            {
                for (int i = 0; i < MonthlyDataList.Count; i++)
                {
                    var monthlyData = MonthlyDataList[i];
                    
                    // 验证修正数量
                    if (monthlyData.CorrectedQuantity < 0)
                    {
                        MessageBox.Show($"{monthlyData.Month}的修正数量不能为负数。", "数据验证失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }

                    // 验证修正金额
                    if (monthlyData.CorrectedAmount < 0)
                    {
                        MessageBox.Show($"{monthlyData.Month}的修正金额不能为负数。", "数据验证失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }

                    // 验证数值范围（防止过大数值）
                    if (monthlyData.CorrectedAmount > 1000000000) // 10亿
                    {
                        MessageBox.Show($"{monthlyData.Month}的修正金额过大，请检查输入。", "数据验证失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                }
            }

            return true;
        }

        [RelayCommand]
        private void ExportData()
        {
            // 导出数据逻辑
        }

        /// <summary>
        /// 保存年度销售预测与年度预算数据
        /// </summary>
        [RelayCommand]
        private void EditBudget()
        {
            try
            {
                // 数据验证 - 只验证年度数据
                if (InvestmentSalesForecast < 0)
                {
                    MessageBox.Show("年度销售预测不能为负数。", "数据验证失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (InvestmentBudget < 0)
                {
                    MessageBox.Show("年度预算不能为负数。", "数据验证失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using var context = new ProjectContext();
                int year = int.Parse(CurrentYear);

                // 保存年度销售预测数据
                var salesVolume = context.SalesVolumeTables.FirstOrDefault(s => s.Year == year);
                if (salesVolume == null)
                {
                    salesVolume = new SalesVolumeTable { Year = year };
                    context.SalesVolumeTables.Add(salesVolume);
                }
                salesVolume.SalesVolume = InvestmentSalesForecast;
                

                // 保存年度预算数据
                var annualBudget = context.AnnualBudgetTable.FirstOrDefault(a => a.Year == year);
                if (annualBudget == null)
                {
                    annualBudget = new AnnualBudget { Year = year };
                    context.AnnualBudgetTable.Add(annualBudget);
                }
                annualBudget.Budget = (int)InvestmentBudget;

                context.SaveChanges();
                
                MessageBox.Show("年度销售预测与年度预算数据已成功保存到数据库。", "保存成功", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // 重新加载数据
                LoadFinancialDataByYear(CurrentYear);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存年度数据时发生错误：{ex.Message}", "保存失败", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 保存月度修正数据
        /// </summary>
        [RelayCommand]
        private void SaveSettings()
        {
            try
            {
                // 数据验证 - 只验证月度数据
                if (MonthlyDataList != null)
                {
                    for (int i = 0; i < MonthlyDataList.Count; i++)
                    {
                        var monthlyData = MonthlyDataList[i];
                        

                        // 验证数值范围（防止过大数值）
                        if (monthlyData.CorrectedAmount > 1000000000) // 10亿
                        {
                            MessageBox.Show($"{monthlyData.Month}的修正金额过大，请检查输入。", "数据验证失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }
                }

                using var context = new ProjectContext();
                int year = int.Parse(CurrentYear);

                // 保存月度修正数据
                var quantityCorrection = context.RevOfAssetQuantTab.FirstOrDefault(r => r.Year == year);
                if (quantityCorrection == null)
                {
                    quantityCorrection = new RevisionOfAssetQuantity { Year = year };
                    context.RevOfAssetQuantTab.Add(quantityCorrection);
                }

                var amountCorrection = context.AsAmountCorrectTab.FirstOrDefault(a => a.Year == year);
                if (amountCorrection == null)
                {
                    amountCorrection = new AssetAmountCorrection { Year = year };
                    context.AsAmountCorrectTab.Add(amountCorrection);
                }

                // 设置各月的修正值
                for (int i = 0; i < MonthlyDataList.Count; i++)
                {
                    var monthlyData = MonthlyDataList[i];
                    int monthIndex = i + 1;
                    
                    // 设置资产数量修正
                    SetMonthlyCorrectionValue(quantityCorrection, monthIndex, monthlyData.CorrectedQuantity);
                    
                    // 设置资产金额修正
                    SetMonthlyCorrectionValue(amountCorrection, monthIndex, monthlyData.CorrectedAmount);
                }

                context.SaveChanges();
                
                MessageBox.Show("月度修正数据已成功保存到数据库。", "保存成功", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // 重新加载数据
                LoadFinancialDataByYear(CurrentYear);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存月度数据时发生错误：{ex.Message}", "保存失败", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
    }


    public class MonthlyData
    {
        public string Month { get; set; }

        /// <summary>
        /// 原始数量
        /// </summary>
        public double OriginalQuantity { get; set; }

        /// <summary>
        /// 原始金额
        /// </summary>
        public double OriginalAmount { get; set; }

        /// <summary>
        /// 修正数量
        /// </summary>
        public double CorrectedQuantity { get; set; }

        /// <summary>
        /// 修正金额
        /// </summary>
        public double CorrectedAmount { get; set; }
    }
}
