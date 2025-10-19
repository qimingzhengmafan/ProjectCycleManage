using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Model;
using ProjectManagement.Models;
using ProjectManagement.ViewModel;
using ScrollControlProjectnetcore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientSide.ViewModel
{
    public partial class MainVM : ObservableObject
    {
        private ProjectContext _projectContext = new ProjectContext();
        private ContextModel _contextmodel;

        [ObservableProperty]
        private Visibility _personalDataViewVisibility = Visibility.Visible;

        [ObservableProperty]
        private Visibility _addViewVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private Visibility _tableViewVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private TablePVM _tableVMInfor;

        [ObservableProperty]
        private PersonalDataVM _personaldataVMInfor = new PersonalDataVM();

        [ObservableProperty]
        private AddVM _addVMInfor = new AddVM();

        public MainVM(string LoginNameInput)
        {
            _contextmodel = new ContextModel(_projectContext);
            _tableVMInfor = new TablePVM(LoginNameInput);
            int _index1 = 0;
            var data = _contextmodel.GetPersonalProjectsStatues(DateTime.Now.Year , LoginNameInput);
            List<string> names = new List<string>();
            List<int> num = new List<int>();
            foreach (var item in data)
            {
                names.Add(item.Status);
                num.Add(item.count);
            }

            PersonaldataVMInfor.PersonalProjectsInformation.Series = num.AsPieSeries((value, series) =>
            {
                series.Name = names[_index1++ % names.Count];
                series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle;
                series.DataLabelsSize = 15;
                series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
                series.DataLabelsFormatter =
                   point =>
                       $"{point.Coordinate.PrimaryValue} " + "/ " +
                       $"{point.StackedValue.Total} ";
                series.ToolTipLabelFormatter = point => $"{point.StackedValue.Share:P2}";
            });

            var personaldata_zhuchengxu = GetPPersonalProjectGrid(DateTime.Now.Year, LoginNameInput);
            //PersonalDataVM_ChengXuZhu
            if (personaldata_zhuchengxu.Count != 0)
            {
                PersonaldataVMInfor.PersonalProjectsList.Clear();
                foreach (var item in personaldata_zhuchengxu)
                {
                    PersonaldataVMInfor.PersonalProjectsList.Add(new SeamlessLoopingScroll.ProjectItem()
                    {
                        ProjectName = item.Project,
                        CountDown = item.DaysDiff,
                        FileProgress = item.FileProgress,
                        Owner = item.ProjectLeader
                    });

                }
            }

            (int AllProjects_zhuchengxu, int ComProjects_zhuchengxu) = GetLeaderCompleteProjects(DateTime.Now.Year, LoginNameInput);
            PersonaldataVMInfor.Allprojectsnum = AllProjects_zhuchengxu;
            PersonaldataVMInfor.CompleteProjectsNum = ComProjects_zhuchengxu;





        }





        #region UIIcommandFun


        [RelayCommand]
        private void PersonalDataViewFun()
        {
            AddViewVisibility = Visibility.Collapsed;
            PersonalDataViewVisibility = Visibility.Visible;
            TableViewVisibility = Visibility.Collapsed;
        }

        [RelayCommand]
        private void AddViewFun()
        {
            AddViewVisibility = Visibility.Visible;
            PersonalDataViewVisibility = Visibility.Collapsed;
            TableViewVisibility = Visibility.Collapsed;
        }

        [RelayCommand]
        private void TableViewFun()
        {
            TableViewVisibility = Visibility.Visible;
            AddViewVisibility = Visibility.Collapsed;
            PersonalDataViewVisibility = Visibility.Collapsed;
        }
        #endregion


        #region OtherFuns

        private List<(string Project, int DaysDiff, int FileProgress, string ProjectLeader)> GetPPersonalProjectGrid(int year, string Name)
        {
            using (var context = new ProjectContext())
            {
                var projects = context.Projects
                    .Where(p => p.Year == year && p.ProjectLeader.PeopleName == Name)
                    .Include(p => p.ProjectLeader)
                    .Select(p => new
                    {
                        p.ProjectName,
                        p.DaysDiff,
                        p.FileProgress,
                        LeaderName = p.ProjectLeader.PeopleName
                    })
                    .OrderBy(p => p.ProjectName)
                    .ToList();

                List<(string Project, int DaysDiff, int FileProgress, string ProjectLeader)> values =
                    new List<(string Project, int DaysDiff, int FileProgress, string ProjectLeader)>();

                foreach (var project in projects)
                {
                    (string Project, int DaysDiff, int FileProgress, string ProjectLeader) projectvaule;
                    projectvaule.Project = project.ProjectName;
                    projectvaule.FileProgress = (int)project.FileProgress.GetValueOrDefault();
                    projectvaule.DaysDiff = project.DaysDiff.GetValueOrDefault();
                    projectvaule.ProjectLeader = project.LeaderName;
                    values.Add(projectvaule);
                }

                //foreach (var item in values)
                //{
                //    PersonalDataVM_YanXin.PersonalProjectsList.Add(new SeamlessLoopingScroll.ProjectItem()
                //    {
                //        ProjectName = item.Project,
                //        CountDown = item.DaysDiff,
                //        FileProgress = item.FileProgress,
                //        Owner = item.ProjectLeader
                //    });
                //}

                return values;

            }
        }

        /// <summary>
        /// 获取项目负责人
        /// </summary>
        /// <param name="year"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        private (int AllProjectNum, int Completeprojects) GetLeaderCompleteProjects(int year, string Name)
        {
            int? allProjectNum;
            int? completeProjects;

            using (var context = new ProjectContext())
            {
                // 查询该人员在该年份的所有项目
                var projects = context.Projects
                .Where(p => p.Year == year &&
                           p.ProjectLeader.PeopleName == Name)
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

        #endregion
    }
}
