using CommunityToolkit.Mvvm.ComponentModel;
using DrawerTest;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectManagement.ViewModel
{
    public partial class TableVM: ObservableObject
    {
        public ProjectContext Context { get; set; }
        private TreeViewModel _treeViewModel = new TreeViewModel();
        private bool IsShow { get; set; }

        [ObservableProperty]
        private int _showwidth = 0;

        [ObservableProperty]
        private DetailedInformation _detailedInformationvm = new DetailedInformation();


        public TreeViewModel TreeViewModel
        {
            get => _treeViewModel;
            set => _treeViewModel = value;
        }
        

        public void ShowingCtrl(string data)
        {
            IsShow = !IsShow;
            if (IsShow)
            {
                Showwidth = 900;
            }
            else
            {
                Showwidth = 0;
            }



        }


        //public DetailedInformation detailedInformation = new DetailedInformation();

        public TableVM()
        {
            int year = DateTime.Now.Year;

            DetailedInformationvm.DataCollection = new System.Collections.ObjectModel.ObservableCollection<DrawerTest.DrawerUIVM>();
            for (int i = 2022; i <= year; i++)
            {
                (int AllCounts , int CompletsCounts) = GetYearsCompleteProjects(i);
                var backdata = GetYearProjectGrid(2022);
                var RecvBriefinformationdata = new System.Collections.ObjectModel.ObservableCollection<ProjectsInformationGrid>();

                foreach (var project in backdata)
                {
                    var Briefinformationdata = new ProjectsInformationGrid();
                    Briefinformationdata.Projectname = project.Project;
                    Briefinformationdata.Projectstage = project.CompletionStatus;
                    Briefinformationdata.Projectleadername = project.ProjectLeader;
                    Briefinformationdata.Detailedinformationfun = ShowingCtrl;
                    if (project.IsCompleted)
                    {
                        Briefinformationdata.Beltcolor = StatusColor.CompletedColors.BeltColor;
                        Briefinformationdata.Textcolor = StatusColor.CompletedColors.TextColor;
                    }
                    else
                    {
                        Briefinformationdata.Beltcolor = StatusColor.UnfinishedColors.BeltColor;
                        Briefinformationdata.Textcolor = StatusColor.UnfinishedColors.TextColor;
                    }



                    RecvBriefinformationdata.Add(Briefinformationdata);
                }


                DetailedInformationvm.DataCollection.Add(new DrawerUIVM()
                {
                    Year = i,
                    AllprojectsNum = AllCounts,
                    CompleteProjects = CompletsCounts,
                    Unit = "年",
                    Briefinformation = RecvBriefinformationdata,
                });
            }


            //DetailedInformationvm.DataCollection[0].Detailedinformationfun = ShowingCtrl;

        }



        #region OtherFun

        private (int AllProjectNum, int Completeprojects) GetYearsCompleteProjects(int year)
        {
            int? allProjectNum;
            int? completeProjects;

            using (var context = new ProjectContext())
            {
                var projects = context.Projects
                .Where(p => p.Year == year )
                .AsNoTracking()
                .ToList();

                // 计算项目总数
                allProjectNum = projects.Count;

                // 计算已完成项目数
                completeProjects = projects.Count(p =>
                    p.ProjectStageId == 105 &&
                    p.ProjectPhaseStatusId == 104);
            }
            return (allProjectNum.GetValueOrDefault(), completeProjects.GetValueOrDefault());

        }

        private (int AllProjectNum, int Completeprojects) GetfollowuppersonCompleteProjects(int year, string Name)
        {
            int? allProjectNum;
            int? completeProjects;

            using (var context = new ProjectContext())
            {
                // 查询该人员在该年份的所有项目
                var projects = context.Projects
                .Where(p => p.Year == year &&
                           p.projectfollowupperson.PeopleName == Name)
                .AsNoTracking()
                .ToList();

                // 计算项目总数
                allProjectNum = projects.Count;

                // 计算已完成项目数
                completeProjects = projects.Count(p =>
                    p.ProjectStageId == 105 &&
                    p.ProjectPhaseStatusId == 104);
            }
            return (allProjectNum.GetValueOrDefault(), completeProjects.GetValueOrDefault());

        }


        private List<(string Project, string ProjectLeader, string CompletionStatus , bool IsCompleted)> GetYearProjectGrid(int year, string name = null)
        {
            using (var context = new ProjectContext())
            {
                IQueryable<Projects> projectsQuery = context.Projects
                    .Where(p => p.Year == year)
                    .Include(p => p.ProjectLeader); // 加载负责人导航属性

                // 如果提供了负责人姓名，进行过滤
                if (!string.IsNullOrEmpty(name))
                {
                    projectsQuery = projectsQuery.Where(p => p.ProjectLeader.PeopleName.Contains(name));
                }

                var projects = projectsQuery
                    .Select(p => new
                    {
                        p.ProjectName,
                        LeaderName = p.ProjectLeader.PeopleName, // 获取负责人姓名
                        p.ProjectStageId,
                        p.ProjectPhaseStatusId,
                        p.FinishTime,
                        IsCompleted = p.ProjectStageId == 105 && p.ProjectPhaseStatusId == 104
                    })
                    .OrderBy(p => p.ProjectName) // 按项目名称排序
                    .ToList();

                List<(string Project, string ProjectLeader, string CompletionStatus , bool IsCompleted)> values =
                    new List<(string Project, string ProjectLeader, string CompletionStatus , bool IsCompleted)>();

                foreach (var project in projects)
                {
                    string completionStatus;
                    bool CompleteStatus;

                    if (project.IsCompleted)
                    {
                        // 已完成项目
                        completionStatus = $"已完成{project.FinishTime?.ToString("(yyyy-MM-dd)") ?? ""}";
                        CompleteStatus = true;
                    }
                    else
                    {
                        // 未完成项目
                        completionStatus = $"未完成{project.FinishTime?.ToString("(yyyy-MM-dd)") ?? ""}";
                        CompleteStatus = false;
                    }

                    values.Add((project.ProjectName, project.LeaderName, completionStatus , CompleteStatus));
                }

                return values;
            }
        }


        #endregion

    }


    public class CompletedColor
    {
        public string BeltColor { get; set; } = "#F0FDF4";
        public string TextColor { get; set; } = "#10B981";
    }

    public class Unfinished
    {
        public string BeltColor { get; set; } = "#FFFBEB";
        public string TextColor { get; set; } = "#FAC42F";
    }

    public static class StatusColor
    {
        public static CompletedColor CompletedColors { get; set; } = new CompletedColor();
        public static Unfinished UnfinishedColors { get; set; } = new Unfinished();
    }
}
