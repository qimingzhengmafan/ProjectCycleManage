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
using System.Xml.Linq;

namespace DrawerTest
{
    public partial class DrawerUIVM: ObservableObject
    {
        //[ObservableProperty] 
        //private double _currentPressurex;
        [ObservableProperty]
        private int _year;

        [ObservableProperty]
        private double _percent;

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
                    Percent = Math.Round((double)CompleteProjects / (double)_allprojectsNum * 100.0 , 0);
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
                    Percent = Math.Round((double)_completeProjects / (double)AllprojectsNum * 100.0 , 1);
                OnPropertyChanged();
            }
        }

        [ObservableProperty]
        private ObservableCollection<ProjectsInformationGrid> _briefinformation = new ObservableCollection<ProjectsInformationGrid>();

        private bool IsShow { get; set; } = false;

        public DrawerUIVM()
        {
            
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


    }

    public partial class ProjectsInformationGrid : ObservableObject
    {
        [ObservableProperty]
        private string _projectname;

        public Action<string> Detailedinformationfun;

        [ObservableProperty]
        private string _projectleadername;

        [ObservableProperty]
        private string _projectstage;

        [ObservableProperty]
        private string _beltcolor;

        [ObservableProperty]
        private string _textcolor;

        public ProjectsInformationGrid()
        {

        }

        [RelayCommand]
        private void DetailedInformation()
        {
            //MessageBox.Show($"You clicked: {Projectname} (Index: {Projectleadername})");
            DetailedInformationACT(Detailedinformationfun);
        }

        private void DetailedInformationACT(Action<string> action)
        {
            action(Projectname);
        }
    }
}
