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
    public partial class SettingVM: ObservableObject
    {

        [RelayCommand]
        private void AmountCard()
        {
            MessageBox.Show("Amount Click!");
        }
    }
}
