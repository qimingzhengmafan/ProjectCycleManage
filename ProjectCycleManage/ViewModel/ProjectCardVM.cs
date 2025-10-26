using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Reflection;
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


        [RelayCommand]
        private void SuspendProject()
        {
            MessageBox.Show("SuspendProject");
            Runningstatus = "暂停";
        }

        [RelayCommand]
        private void RestartProject()
        {
            MessageBox.Show("RestartProject");
        }

        [RelayCommand]
        private void ViewDetails()
        {
            MessageBox.Show("ViewDetails");
        }

        private enum runningstatus
        {
            [Description("进行中")]
            进行中,

            [Description("暂停")]
            暂停,

            [Description("已完成")]
            已完成,

            [Description("审核中")]
            审核中
        }
    }
}
