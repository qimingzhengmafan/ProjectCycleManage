﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using ProjectCycleManage.Model;
using ProjectManagement.Data;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectCycleManage.ViewModel
{
    public partial class AllProjectsVM : ObservableObject
    {
        #region 属性

        /// <summary>
        /// 搜索文本
        /// </summary>
        [ObservableProperty]
        private string _searchText;

        /// <summary>
        /// 项目列表
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<ProjectListDisplayModel> _projectList;

        /// <summary>
        /// 过滤后的项目列表
        /// </summary>
        private ObservableCollection<ProjectListDisplayModel> _allProjects;

        /// <summary>
        /// 选中的项目
        /// </summary>
        [ObservableProperty]
        private ProjectListDisplayModel _selectedProject;

        /// <summary>
        /// 年份列表
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<string> _yearList;

        /// <summary>
        /// 选中的年份
        /// </summary>
        [ObservableProperty]
        private string _selectedYear;

        /// <summary>
        /// 人员列表
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<PeopleTable> _personList;

        /// <summary>
        /// 选中的人员
        /// </summary>
        [ObservableProperty]
        private PeopleTable _selectedPerson;

        #endregion

        #region 构造函数

        public AllProjectsVM()
        {
            // 初始化集合
            ProjectList = new ObservableCollection<ProjectListDisplayModel>();
            _allProjects = new ObservableCollection<ProjectListDisplayModel>();
            YearList = new ObservableCollection<string>();
            PersonList = new ObservableCollection<PeopleTable>();

            // 加载数据
            LoadYearList();
            LoadPersonList();
            LoadProjectData();
        }

        #endregion

        #region 数据加载方法

        /// <summary>
        /// 加载年份列表
        /// </summary>
        private void LoadYearList()
        {
            try
            {
                using (var context = new ProjectContext())
                {
                    var years = context.Projects
                        .Where(p => p.Year.HasValue)
                        .Select(p => p.Year.Value.ToString())
                        .Distinct()
                        .OrderByDescending(y => y)
                        .ToList();

                    YearList.Clear();
                    YearList.Add("全部年份");
                    foreach (var year in years)
                    {
                        YearList.Add(year);
                    }

                    SelectedYear = "全部年份";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载年份列表失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 加载人员列表
        /// </summary>
        private void LoadPersonList()
        {
            try
            {
                using (var context = new ProjectContext())
                {
                    var people = context.PeopleTable
                        .OrderBy(p => p.PeopleName)
                        .ToList();

                    PersonList.Clear();
                    PersonList.Add(new PeopleTable { PeopleId = 0, PeopleName = "全部人员" });
                    foreach (var person in people)
                    {
                        PersonList.Add(person);
                    }

                    SelectedPerson = PersonList.First();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载人员列表失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 加载项目数据
        /// </summary>
        private void LoadProjectData()
        {
            try
            {
                using (var context = new ProjectContext())
                {
                    var projects = context.Projects
                        .Include(p => p.ProjectLeader)
                        .Include(p => p.type)
                        .Include(p => p.equipmenttype)
                        .ToList();

                    _allProjects.Clear();
                    foreach (var project in projects)
                    {
                        var displayModel = new ProjectListDisplayModel
                        {
                            ProjectsId = project.ProjectsId,
                            Projectname = project.ProjectName,
                            Projectidentifyingnumber = project.ProjectIdentifyingNumber,
                            ManagerName = project.ProjectLeader?.PeopleName ?? "未指定",
                            Projectyear = project.Year,
                            Annualbudget = ParseDecimal(project.Budget),
                            ActualExpense = ParseDecimal(project.ActualExpenditure),
                            Progress = project.ProjectProgress ?? 0,
                            HealthStatus = CalculateHealthStatus(project.ProjectProgress ?? 0),
                            ProjectType = project.type?.TypeName ?? "未指定",
                            Department = project.UsingDepartment ?? "未指定",
                            Remarks = project.remarks
                        };

                        _allProjects.Add(displayModel);
                    }

                    ApplyFilters();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载项目数据失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 解析字符串为Decimal
        /// </summary>
        private decimal? ParseDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (decimal.TryParse(value, out decimal result))
                return result;

            return null;
        }

        /// <summary>
        /// 计算健康状况
        /// </summary>
        private string CalculateHealthStatus(int progress)
        {
            if (progress >= 60)
                return "良好";
            else if (progress >= 30)
                return "警告";
            else
                return "严重";
        }

        #endregion

        #region 筛选和搜索

        /// <summary>
        /// 搜索文本变化时触发
        /// </summary>
        partial void OnSearchTextChanged(string value)
        {
            ApplyFilters();
        }

        /// <summary>
        /// 年份选择变化时触发
        /// </summary>
        partial void OnSelectedYearChanged(string value)
        {
            ApplyFilters();
        }

        /// <summary>
        /// 人员选择变化时触发
        /// </summary>
        partial void OnSelectedPersonChanged(PeopleTable value)
        {
            ApplyFilters();
        }

        /// <summary>
        /// 应用筛选条件
        /// </summary>
        private void ApplyFilters()
        {
            var filtered = _allProjects.AsEnumerable();

            // 搜索文本筛选
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(p =>
                    (p.Projectname?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (p.Projectidentifyingnumber?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (p.ManagerName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)
                );
            }

            // 年份筛选
            if (!string.IsNullOrEmpty(SelectedYear) && SelectedYear != "全部年份")
            {
                if (int.TryParse(SelectedYear, out int year))
                {
                    filtered = filtered.Where(p => p.Projectyear == year);
                }
            }

            // 人员筛选
            if (SelectedPerson != null && SelectedPerson.PeopleId != 0)
            {
                filtered = filtered.Where(p => p.ManagerName == SelectedPerson.PeopleName);
            }

            ProjectList = new ObservableCollection<ProjectListDisplayModel>(filtered);
        }

        #endregion

        #region 命令

        /// <summary>
        /// 导出数据命令
        /// </summary>
        [RelayCommand]
        private void ExportData()
        {
            try
            {
                MessageBox.Show("导出数据功能待实现", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                // TODO: 实现导出功能
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出数据失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 刷新数据命令
        /// </summary>
        [RelayCommand]
        private void RefreshData()
        {
            LoadProjectData();
        }

        #endregion
    }
}
