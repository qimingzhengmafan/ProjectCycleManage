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
        #region UIVisibility
        [ObservableProperty]
        private Visibility _approvalVilib = Visibility.Collapsed;

        [ObservableProperty]
        private Visibility _mainareavisilib = Visibility.Visible;


        #endregion






        private ApprovalAuthorityVM _approvalauthorityvm;
        public ApprovalAuthorityVM ApprovalauthorityVM
        {
            get => _approvalauthorityvm;
            set
            {
                _approvalauthorityvm = value;
                OnPropertyChanged();
            }
        }


        public SettingVM()
        {
            Mainareavisilib = Visibility.Visible;

            ApprovalVilib = Visibility.Collapsed;
        }


        #region ClickFun

        [RelayCommand]
        private void AmountCard()
        {
            ApprovalauthorityVM = new ApprovalAuthorityVM();
            ApprovalVilib = Visibility.Visible;

            Mainareavisilib = Visibility.Collapsed;

            //MessageBox.Show("Amount Click!");
        }

        #endregion

    }
}
