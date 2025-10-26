using CommunityToolkit.Mvvm.ComponentModel;
using OpenTK.Graphics.OpenGL;
using ProjectCycleManage.Model;
using ProjectCycleManage.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCycleManage.ViewModel
{
    public partial class OverviewVM: ObservableObject
    {
        private ObservableCollection<ProjectCardVM> _projectshowarea = new ObservableCollection<ProjectCardVM>()
        {
            //ProjectName
            //RunningStatus
            //ProjectLeader
            //StartTime
            //ProjectStatus
            //ProjectStatusProgressdouble
            new ProjectCardVM()
            {
                Projectname = "磁钢机",
                Runningstatus = "进行中",
                Projectleader = "朱成绪",
                Starttimme = "2025/10/25",
                Projectstatus = "项目评审",
                Projectstatusprogressdouble = 10.0,
            }
        };

        public ObservableCollection<ProjectCardVM> ProjectShowAreaCard
        {
            get => _projectshowarea;
            set => _projectshowarea = value;
        }
        public OverviewVM()
        {

        }

    }
}
