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
        /// <summary>
        /// 登陆人
        /// </summary>
        private string _loginpersonname;

        /// <summary>
        /// 登陆者级别
        /// </summary>
        private int _loginpersonnamegrade;


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

        [ObservableProperty]
        private Visibility _vis_importhisvisib = Visibility.Collapsed;

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

        private ImportHistoryVM _importhistoryvm;
        public ImportHistoryVM ImportHistoryInforVM
        {
            get => _importhistoryvm;
            set
            {
                _importhistoryvm = value;
                OnPropertyChanged();
            }
        }

        public SettingVM(int loginpeoplegrade, string loginpeoplename)
        {
            _loginpersonname = loginpeoplename;
            _loginpersonnamegrade = loginpeoplegrade;

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
            Vis_importhisvisib = Visibility.Collapsed;
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
            Vis_importhisvisib = Visibility.Collapsed;
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
            Vis_importhisvisib = Visibility.Collapsed;
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
            Vis_importhisvisib = Visibility.Collapsed;
        }

        [RelayCommand]
        private void ImportHisView()
        {
            Vis_importhisvisib = Visibility.Visible;
            ImportHistoryInforVM = new ImportHistoryVM(_loginpersonnamegrade, _loginpersonname);

            Projectstagevisib = Visibility.Collapsed;
            ApprovalVilib = Visibility.Collapsed;
            Financialvisib = Visibility.Collapsed;
            Personalvisib = Visibility.Collapsed;
            Mainareavisilib = Visibility.Collapsed;

            
        }

        #endregion

    }
}
