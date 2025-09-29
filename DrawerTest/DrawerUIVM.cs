using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawerTest
{
    public partial class DrawerUIVM: ObservableObject
    {
        [ObservableProperty] private double _currentPressurex;

        [ObservableProperty] 
        private int _uiheight;

        [ObservableProperty]
        private int _allprojectsNum = 0;

        [ObservableProperty]
        private int _completeProjects;

        //[ObservableProperty]
        private bool IsShow { get; set; } = false;

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

        
    }
}
