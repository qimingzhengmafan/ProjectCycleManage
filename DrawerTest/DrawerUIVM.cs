using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private string _unit;

        /// <summary>
        /// 所有项目
        /// </summary>
        private int _allprojectsNum = 10;
        public int AllprojectsNum
        {
            get => _allprojectsNum;
            set
            {
                _allprojectsNum = value;
                if (value == 0)
                    Percent = 0;
                else
                    Percent = CompleteProjects / _allprojectsNum * 100;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 已完成
        /// </summary>
        private int _completeProjects;
        public int CompleteProjects
        {
            get => _completeProjects;
            set
            {
                _completeProjects = value;
                if (AllprojectsNum == 0)
                    Percent = 0;
                else
                    Percent = _completeProjects / AllprojectsNum * 100;

                OnPropertyChanged();
            }
        }

        [ObservableProperty]
        private ObservableCollection<ProjectsInformationGrid> _briefinformation = new ObservableCollection<ProjectsInformationGrid>();

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

    public partial class ProjectsInformationGrid : ObservableObject
    {
        [ObservableProperty]
        private string _projectname;

        [ObservableProperty]
        private string _projectleadername;

        [ObservableProperty]
        private string _projectstage = "ghjk";

        [ObservableProperty]
        private string _beltcolor;

        [ObservableProperty]
        private string _textcolor;
    }
}
