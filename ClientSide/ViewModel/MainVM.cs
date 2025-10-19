using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectManagement.Models;
using ProjectManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientSide.ViewModel
{
    public partial class MainVM : ObservableObject
    {
        [ObservableProperty]
        private Visibility _personalDataViewVisibility = Visibility.Visible;

        [ObservableProperty]
        private Visibility _addViewVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private Visibility _tableViewVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private TableVM _tableVMInfor;

        [ObservableProperty]
        private PersonalDataVM _personaldataVMInfor;

        [ObservableProperty]
        private AddVM _addVMInfor;

        public MainVM()
        {

        }





        #region UIIcommandFun


        [RelayCommand]
        private void PersonalDataViewFun()
        {
            AddViewVisibility = Visibility.Collapsed;
            PersonalDataViewVisibility = Visibility.Visible;
            TableViewVisibility = Visibility.Collapsed;
        }

        [RelayCommand]
        private void AddViewFun()
        {
            AddViewVisibility = Visibility.Visible;
            PersonalDataViewVisibility = Visibility.Collapsed;
            TableViewVisibility = Visibility.Collapsed;
        }

        [RelayCommand]
        private void TableViewFun()
        {
            TableViewVisibility = Visibility.Visible;
            AddViewVisibility = Visibility.Collapsed;
            PersonalDataViewVisibility = Visibility.Collapsed;
        }
        #endregion

    }
}
