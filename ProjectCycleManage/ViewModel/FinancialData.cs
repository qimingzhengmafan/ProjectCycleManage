using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCycleManage.ViewModel
{
    public partial class FinancialData : ObservableObject
    {
        [ObservableProperty]
        private string currentYear = "2024";

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

        public FinancialData()
        {
            InitializeQuarterlyData();
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
            // 这里可以添加根据年份加载不同数据的逻辑
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
    }

    public class QuarterlyData
    {
        public string Quarter { get; set; }
        public string Sales { get; set; }
        public string Budget { get; set; }
        public string CompletionRate { get; set; }
        public string Status { get; set; }
    }
}
