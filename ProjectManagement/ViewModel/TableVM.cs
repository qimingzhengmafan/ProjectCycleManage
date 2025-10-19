using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DrawerTest;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Models;
using ProjectManagement.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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
        
        [ObservableProperty]
        private ProjectDetailsVM _projectdetailsvm = new ProjectDetailsVM();

        public TreeViewModel TreeViewModel
        {
            get => _treeViewModel;
            set => _treeViewModel = value;
        }



        public async void ShowingCtrl(string data)
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

            Projects projectdata = await GetProjectByProjectNameAsync(data);

            Projectdetailsvm.Projectsid = projectdata.ProjectsId;
            Projectdetailsvm.ProjectName = projectdata.ProjectName;
            Projectdetailsvm.Equipmentname = projectdata.EquipmentName;
            Projectdetailsvm.ProjectIdentifyingNumber = projectdata.ProjectIdentifyingNumber;
            Projectdetailsvm.FinishTime = projectdata.FinishTime;
            Projectdetailsvm.StartTime = projectdata.StartTime;
            Projectdetailsvm.Budget = projectdata.Budget;
            Projectdetailsvm.ActualExpenditure = projectdata.ActualExpenditure;
            Projectdetailsvm.Assetnumber = projectdata.AssetNumber;
            Projectdetailsvm.Remarkks = projectdata.remarks;

            //Projectdetailsvm.SelectedEquipmentType
            using (var context = new ProjectContext())
            {
                try
                {
                    var equipmenttypedata = await context.Projects
                        .Include(c => c.equipmenttype)
                        .FirstAsync(c => c.equipmenttypeId == projectdata.equipmenttypeId.GetValueOrDefault());
                    if (equipmenttypedata != null)
                    {
                        Projectdetailsvm.SelectedEquipmentType = equipmenttypedata.equipmenttype;
                    }
                }
                catch (Exception)
                {

                }
            };

            //Projectdetailsvm.SelectedType.TypeId = projectdata.typeId.GetValueOrDefault();
            using (var context = new ProjectContext())
            {
                try
                {
                    var typedata = await context.Projects
                        .Include(c => c.type)
                        .FirstAsync(c => c.typeId == projectdata.typeId.GetValueOrDefault());
                    if (typedata != null)
                    {
                        Projectdetailsvm.SelectedType = typedata.type;
                    }
                }
                catch (Exception)
                {

                }
            };

            //Projectdetailsvm.Selectedprojectstage.ProjectStageId = projectdata.ProjectStageId;
            using (var context = new ProjectContext())
            {
                try
                {
                    var projectstagedata = await context.Projects
                        .Include(c => c.ProjectStage)
                        .FirstAsync(c => c.ProjectStageId == projectdata.ProjectStageId);
                    if (projectstagedata != null)
                    {
                        Projectdetailsvm.Selectedprojectstage = projectstagedata.ProjectStage;
                    }
                }
                catch (Exception)
                {

                }
            };

            //Projectdetailsvm.Selectedprojectphasestatus.ProjectPhaseStatusId = projectdata.ProjectPhaseStatusId.GetValueOrDefault();
            using (var context = new ProjectContext())
            {
                try
                {
                    var ProjectPhaseStatusdata = await context.Projects
                        .Include(c => c.ProjectPhaseStatus)
                        .FirstAsync(c => c.ProjectPhaseStatusId == projectdata.ProjectPhaseStatusId);
                    if (ProjectPhaseStatusdata != null)
                    {
                        Projectdetailsvm.Selectedprojectphasestatus = ProjectPhaseStatusdata.ProjectPhaseStatus;
                    }
                }
                catch (Exception)
                {

                }
            };

            //Projectdetailsvm.SelectedEmployee.PeopleId = projectdata.ProjectLeaderId.GetValueOrDefault();
            using (var context = new ProjectContext())
            {
                try
                {
                    var ProjectLearderdata = await context.Projects
                        .Include(c => c.ProjectLeader)
                        .FirstAsync(c => c.ProjectLeaderId == projectdata.ProjectLeaderId);
                    if (ProjectLearderdata != null)
                    {
                        Projectdetailsvm.SelectedEmployee = ProjectLearderdata.ProjectLeader;
                    }
                }
                catch (Exception)
                {

                }
            };

            //Projectdetailsvm.SelectedFollowEmployee.PeopleId = projectdata.projectfollowuppersonId.GetValueOrDefault();
            using (var context = new ProjectContext())
            {
                try
                {
                    var ProjectFollowdata = await context.Projects
                        .Include(c => c.projectfollowupperson)
                        .FirstAsync(c => c.projectfollowuppersonId == projectdata.projectfollowuppersonId);
                    if (ProjectFollowdata != null)
                    {
                        Projectdetailsvm.SelectedFollowEmployee = ProjectFollowdata.projectfollowupperson;
                    }
                }
                catch (Exception)
                {

                }
            };

        }


        //public DetailedInformation detailedInformation = new DetailedInformation();

        public TableVM()
        {
            int year = DateTime.Now.Year;

            DetailedInformationvm.DataCollection = new System.Collections.ObjectModel.ObservableCollection<DrawerTest.DrawerUIVM>();
            for (int i = 2022; i <= year; i++)
            {
                (int AllCounts , int CompletsCounts) = GetYearsCompleteProjects(i);
                var backdata = GetYearProjectGrid(i);
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

        }

        [RelayCommand]
        private void GetPersonalDatalistFun()
        {
            int year = DateTime.Now.Year;

            DetailedInformationvm.DataCollection = new System.Collections.ObjectModel.ObservableCollection<DrawerTest.DrawerUIVM>();
            for (int i = 2022; i <= year; i++)
            {
                (int AllCounts, int CompletsCounts) = GetYearsCompleteProjectsleadername(i , "朱成绪");
                var backdata = GetYearProjectGrid(i , "朱成绪");
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

        /// <summary>
        /// 项目负责人查找
        /// </summary>
        /// <param name="year"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private (int AllProjectNum, int Completeprojects) GetYearsCompleteProjectsleadername(int year, string name = null)
        {
            int? allProjectNum;
            int? completeProjects;

            using (var context = new ProjectContext())
            {
                // 基础查询条件
                var query = context.Projects
                    .Where(p => p.Year == year)
                    .AsNoTracking();

                // 如果name不为空，添加名称过滤条件
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(p => p.ProjectLeader.PeopleName.Contains(name));
                }

                // 执行查询
                var projects = query.ToList();

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
        
        public async Task<Projects> GetProjectByProjectNameAsync(string name)
        {
            using var context = new ProjectContext();

            return await context.Projects
                .FirstAsync(u => u.ProjectName == name);
            
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
