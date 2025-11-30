using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore.Query.Internal;
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

        [ObservableProperty]
        private Visibility _financialvisib = Visibility.Collapsed;

        [ObservableProperty]
        private Visibility _personalvisib = Visibility.Collapsed;

        [ObservableProperty]
        private Visibility _projectstagevisib = Visibility.Collapsed;

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

        private FinancialData _financialdatavm;
        public FinancialData FinancialDataVM
        {
            get => _financialdatavm;
            set
            {
                _financialdatavm = value;
                OnPropertyChanged();
            }
        }

        private PersonManagVM _personalmanvm;
        public PersonManagVM PersonManagVM
        {
            get => _personalmanvm;
            set
            {
                _personalmanvm = value;
                OnPropertyChanged();
            }
        }


        private ProjectStageVM _projectstagevm;
        public ProjectStageVM ProjectstageviewVM
        {
            get => _projectstagevm;
            set
            {
                _projectstagevm = value;
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
            Personalvisib = Visibility.Collapsed;
            Projectstagevisib = Visibility.Collapsed;
            Financialvisib = Visibility.Collapsed;
            //MessageBox.Show("Amount Click!");
        }

        [RelayCommand]
        private void FinancialDataCard()
        {
            FinancialDataVM = new ();
            Financialvisib = Visibility.Visible;

            Mainareavisilib = Visibility.Collapsed;
            Personalvisib = Visibility.Collapsed;
            Projectstagevisib = Visibility.Collapsed;
            ApprovalVilib = Visibility.Collapsed;
        }


        [RelayCommand]
        private void PersonalVMCard()
        {
            PersonManagVM = new();
            Personalvisib = Visibility.Visible;

            ApprovalVilib = Visibility.Collapsed;
            Financialvisib = Visibility.Collapsed;
            Projectstagevisib = Visibility.Collapsed;
            Mainareavisilib = Visibility.Collapsed;
        }

        [RelayCommand]
        private void ProjectStageVMCard()
        {
            ProjectstageviewVM = new();
            Projectstagevisib = Visibility.Visible;

            ApprovalVilib = Visibility.Collapsed;
            Financialvisib = Visibility.Collapsed;
            Personalvisib = Visibility.Collapsed;
            Mainareavisilib = Visibility.Collapsed;
        }



        #endregion

    }
}
