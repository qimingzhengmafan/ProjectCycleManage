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
    public partial class ProjectCardVM : ObservableObject
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
        private DateTime? _starttimme;

        [ObservableProperty]
        private string _projectstatus;

        [ObservableProperty]
        private double _projectstatusprogressdouble;

        [ObservableProperty]
        private string _projectsid;

        public Action<string> ViewDetailsaction;

        [RelayCommand]
        private void SuspendProject()
        {
            //暂停
            MessageBox.Show("SuspendProject");
            Runningstatus = "暂停";
        }

        [RelayCommand]
        private void RestartProject()
        {
            //重启
            MessageBox.Show("RestartProject");
        }

        [RelayCommand]
        private void ViewDetails()
        {
            //查看详情
            //MessageBox.Show("此项目序号为 ： " + Projectsid);
            ViewDetailsaction(Projectsid);
        }

        private void ViewDetailsFun(string ProjectID)
        {
            ViewDetailsaction(ProjectID);
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
