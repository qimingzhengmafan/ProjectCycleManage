using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using OpenTK.Graphics.OpenGL;
using ProjectCycleManage.Model;
using ProjectCycleManage.View;
using ProjectManagement.Data;
using ProjectManagement.Models;
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

            // 创建数据库上下文
            using var context = new ProjectContext();

            // 查询2025年的所有项目
            var projects2025 = context.Projects
                .Where(p => p.Year == 2025)
                .Include(p => p.ProjectStage)
                .Include(p => p.type)
                .Include(p => p.ProjectPhaseStatus)
                .Include(p => p.ProjectLeader)
                .ToList();

            // 输出结果
            foreach (var project in projects2025)
            {
                ProjectShowAreaCard.Add(new ProjectCardVM()
                {
                    Projectname = project.ProjectName,
                    Projectleader = project.ProjectLeader.PeopleName,
                    Projectstatus = project.ProjectPhaseStatus.ProjectPhaseStatusName,
                    Projectstatusprogressdouble = (double)project.FileProgress.GetValueOrDefault(),
                    Runningstatus = project.ProjectStage.ProjectStageName,
                    Starttimme = project.ApplicationTime.GetValueOrDefault().ToString(),
                });

            }
            Task.Run(() =>
            {

            });

        }

    }
}
