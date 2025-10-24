using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectCycleManage.Model
{
    public partial class ProjectCardVM: ObservableObject
    {
        //ProjectName
        //RunningStatus
        //ProjectLeader
        //StartTime
        //ProjectStatus
        //ProjectStatusProgressdouble

        [ObservableProperty]
        private string _projectname;

        [ObservableProperty]
        private string _runningstatus;

        [ObservableProperty]
        private string _projectleader;

        [ObservableProperty]
        private string _starttimme;

        [ObservableProperty]
        private string _projectstatus;

        [ObservableProperty]
        private double _projectstatusprogressdouble;


        //SuspendProject
        //RestartProject

        [RelayCommand]
        private void SuspendProject()
        {
            MessageBox.Show("SuspendProject");
        }

        [RelayCommand]
        private void RestartProject()
        {
            MessageBox.Show("RestartProject");
        }
    }
}
