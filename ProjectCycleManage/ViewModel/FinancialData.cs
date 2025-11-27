using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCycleManage.ViewModel
{
    public partial class FinancialData:ObservableObject
    {

        public FinancialData()
        {

        }



        [RelayCommand]
        private void SelectYear()
        {

        }


    }

    //public class YearButtonItem : ObservableObject
    //{
    //    [ObservableProperty]
    //    private string _year;

    //}
}
