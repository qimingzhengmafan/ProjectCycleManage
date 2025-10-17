using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DrawerTest
{
    public partial class DrawerUIVM: ObservableObject
    {
        //[ObservableProperty] 
        //private double _currentPressurex;
        [ObservableProperty]
        private int _year;

        [ObservableProperty]
        private int _percent;

        [ObservableProperty] 
        private Action _detailedinformationfun;

        [ObservableProperty] 
        private int _uiheight;

        [ObservableProperty]
        private int _allprojectsNum = 10;

        [ObservableProperty]
        private int _completeProjects;

        //[ObservableProperty]
        private bool IsShow { get; set; } = false;

        public DrawerUIVM()
        {
            _detailedinformationfun = tt;
        }



        [RelayCommand]
        private void ShowICommand()
        {
            IsShow = !IsShow;
            if (IsShow)
            {
                Uiheight = AllprojectsNum * 60;
            }
            else
            {
                Uiheight = 0;
            }
        }

        private void tt()
        {
            MessageBox.Show("test");
        }

        [RelayCommand]
        private void DetailedInformation()
        {
            DetailedInformationACT(_detailedinformationfun);
        }


        private void DetailedInformationACT(Action action)
        {
            action();
        }

    }
}
