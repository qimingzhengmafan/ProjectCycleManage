using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.EntityFrameworkCore;
using Page_Navigation_App.Utilities;
using ProjectManagement.Data;
using ProjectManagement.Model;
using ProjectManagement.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Linq;

namespace ProjectManagement.ViewModel
{
    public class MainVM:ViewModelBase
    {
        #region Command
        public ICommand ConnectCommand { get; set; }
        //public ICommand SearchCommand { get; set; }

        #region UIIcommand
        public ICommand EngineeringOverViewICommand { get; set; }
        public ICommand MaintenanceOverViewICommand { get; set; }
        public ICommand AddViewICommand { get; set; }
        public ICommand ChangeViewICommand { get; set; }
        public ICommand SettingViewICommand { get; set; }
        public ICommand TableViewICommand { get; set; }
        #endregion

        #endregion

        #region UIVisilibity
        private Visibility _engineeringoverviewvisibility = Visibility.Visible;
        public Visibility EngineeringOverViewVisibility
        {
            get => _engineeringoverviewvisibility;
            set
            {
                _engineeringoverviewvisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _maintenanceoverviewvisibility = Visibility.Collapsed;
        public Visibility MaintenanceOverViewVisibility
        {
            get => _maintenanceoverviewvisibility;
            set
            {
                _maintenanceoverviewvisibility = value;
                OnPropertyChanged();
            }
        }



        private Visibility _addviewvisibility = Visibility.Collapsed;
        public Visibility AddViewVisibility
        {
            get => _addviewvisibility;
            set
            {
                _addviewvisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _changeviewvisibility = Visibility.Collapsed;
        public Visibility ChangeViewVisibility
        {
            get => _changeviewvisibility;
            set
            {
                _changeviewvisibility = value;
                OnPropertyChanged();
            }
        }
        private Visibility _settingviewvisibility = Visibility.Collapsed;
        public Visibility SettingViewVisibility
        {
            get => _settingviewvisibility;
            set
            {
                _settingviewvisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _tableviewvisibility = Visibility.Collapsed;
        public Visibility TableViewVisibility
        {
            get => _tableviewvisibility;
            set
            {
                _tableviewvisibility = value;
                OnPropertyChanged();
            }
        }
        #endregion

        //private SQLSentence SQLSentence1 = new SQLSentence();

        //private MySQLOperationClass DataPool = new MySQLOperationClass();

        #region ViewInformation
        private OverviewVM _engineeringoverviewvm = new OverviewVM()
        {
            Department = "设备",
            People1 = "朱成绪",
            People2 = "董鑫",
            People3 = "裴涛",
            People4 = "江琛",
            People5 = "王嘉豪",
            People6 = "张园园",
            People7 = "严鑫"

        };
        public OverviewVM EngineeringOverviewVMInfor
        {
            get => _engineeringoverviewvm;
            set => _engineeringoverviewvm = value;
        }

        private OverviewVM _maintenanceoverviewvm = new OverviewVM()
        {
            Department = "维修",
            People1 = "余洋",
            People2 = "胡本立",
            People3 = "姜健康"
        };
        public OverviewVM MaintenanceOverviewvmVMInfor
        {
            get => _maintenanceoverviewvm;
            set => _maintenanceoverviewvm = value;
        }

        private AddVM _addviewvm = new AddVM();
        public AddVM AddViewVMInfor
        {
            get => _addviewvm;
            set => _addviewvm = value;
        }


        private SettingVM settingviewvm = new SettingVM();
        public SettingVM SettingVMInfor
        {
            get => settingviewvm;
            set => settingviewvm = value;
        }

        private TableVM tableviewvm = new TableVM();
        public TableVM TableVMInfor
        {
            get => tableviewvm;
            set => tableviewvm = value;
        }

        #endregion

        private ProjectContext _projectContext = new ProjectContext();
        private ContextModel _contextmodel;

        private string _messagedata;
        public string Messagedata
        {
            get => _messagedata;
            set
            {
                _messagedata = value;
                IsOpen = true;
                ShowToast();
                OnPropertyChanged();
            }
        }

        private bool _isopen;
        public bool IsOpen
        {

            get => _isopen;
            set
            {
                _isopen = value;
                OnPropertyChanged();
            }
        }
        public MainVM()
        {
            _contextmodel = new ContextModel(_projectContext);


            int _index = 0;
            string[] _names = new string[4] { "2022", "2023", "2024", "2025" };

            int _index1 = 0;

            var totaoprojects = _contextmodel.GetTotalProjectsNum();
            var PerYearProjects = _contextmodel.GetProjectsForYears();


            EngineeringOverviewVMInfor.AllProjectsInformation.Series = PerYearProjects.AsPieSeries((value, series) =>
            {
                series.Name = _names[_index++ % _names.Length];
                series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle;
                series.DataLabelsSize = 15;
                series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
                series.DataLabelsFormatter =
                   point =>
                       $"{point.Coordinate.PrimaryValue} " + "/ " +
                       $"{point.StackedValue.Total} ";
                series.ToolTipLabelFormatter = point => $"{point.StackedValue.Share:P2}";
            });

            var data =  _contextmodel.GetProjectsStatues(DateTime.Now.Year);
            List<string> names = new List<string>();
            List<int> num = new List<int>();
            foreach ( var item in data)
            {
                names.Add(item.b);
                num.Add(item.a);
            }
            EngineeringOverviewVMInfor.ProjectStage.Series = num.AsPieSeries((value, series) =>
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


            ConnectCommand = new RelayCommand(ConnectIcommand);
            EngineeringOverViewICommand = new RelayCommand(EngineeringOverViewICommandFun);
            MaintenanceOverViewICommand = new RelayCommand(MaintenanceOverViewICommandFun);
            AddViewICommand = new RelayCommand(AddViewICommandFun);
            ChangeViewICommand = new RelayCommand(ChangeViewICommandFun);
            SettingViewICommand = new RelayCommand(SettingViewICommandFun);
            TableViewICommand = new RelayCommand(TableViewICommandFun);

            Messagedata = "";
            var backdata = GetYearProjectGrid(DateTime.Now.Year);
            foreach (var item in backdata)
            {
                if (item.IsCompleted)
                {
                    Messagedata = Messagedata + item.Project + "  " + item.CompletionStatus + "  " + "请处理";
                }
                else
                {
                    //Messagedata = Messagedata + item.Project + "  " + "\r\n" + item.CompletionStatus + "  " + "请处理" + "\r\n";
                }
                //Messagedata = "ghjkl";


            }
            
        }

        //ProjectInfor

        #region CommandFunction
        private void ConnectIcommand(object obj)
        {
            //DataPool.Connect();
        }

        #region UIIcommandFun
        private void EngineeringOverViewICommandFun(object obj)
        {
            EngineeringOverViewVisibility = Visibility.Visible;
            MaintenanceOverViewVisibility = Visibility.Collapsed;
            AddViewVisibility = Visibility.Collapsed;
            ChangeViewVisibility = Visibility.Collapsed;
            SettingViewVisibility = Visibility.Collapsed;
            TableViewVisibility = Visibility.Collapsed;
        }

        private void MaintenanceOverViewICommandFun(object obj)
        {
            MaintenanceOverViewVisibility = Visibility.Visible;
            EngineeringOverViewVisibility = Visibility.Collapsed;
            AddViewVisibility = Visibility.Collapsed;
            ChangeViewVisibility = Visibility.Collapsed;
            SettingViewVisibility = Visibility.Collapsed;
            TableViewVisibility = Visibility.Collapsed;
        }

        private void OverViewICommandFun(object obj)
        {
            EngineeringOverViewVisibility = Visibility.Visible;
            MaintenanceOverViewVisibility = Visibility.Collapsed;
            AddViewVisibility = Visibility.Collapsed;
            ChangeViewVisibility = Visibility.Collapsed;
            SettingViewVisibility = Visibility.Collapsed;
            TableViewVisibility = Visibility.Collapsed;
        }

        private void AddViewICommandFun(object obj)
        {
            AddViewVisibility = Visibility.Visible;
            MaintenanceOverViewVisibility = Visibility.Collapsed;
            EngineeringOverViewVisibility = Visibility.Collapsed;
            ChangeViewVisibility = Visibility.Collapsed;
            SettingViewVisibility = Visibility.Collapsed;
            TableViewVisibility = Visibility.Collapsed;
        }

        private void ChangeViewICommandFun(object obj)
        {
            ChangeViewVisibility = Visibility.Visible;
            MaintenanceOverViewVisibility = Visibility.Collapsed;
            SettingViewVisibility = Visibility.Collapsed;
            TableViewVisibility = Visibility.Collapsed;
            AddViewVisibility = Visibility.Collapsed;
            EngineeringOverViewVisibility = Visibility.Collapsed;
        }

        private void SettingViewICommandFun(object obj)
        {
            SettingViewVisibility = Visibility.Visible;
            MaintenanceOverViewVisibility = Visibility.Collapsed;
            TableViewVisibility = Visibility.Collapsed;
            AddViewVisibility = Visibility.Collapsed;
            EngineeringOverViewVisibility = Visibility.Collapsed;
            ChangeViewVisibility = Visibility.Collapsed;
        }

        private void TableViewICommandFun(object obj)
        {
            TableViewVisibility = Visibility.Visible;
            MaintenanceOverViewVisibility = Visibility.Collapsed;
            SettingViewVisibility = Visibility.Collapsed;
            AddViewVisibility = Visibility.Collapsed;
            EngineeringOverViewVisibility = Visibility.Collapsed;
            ChangeViewVisibility = Visibility.Collapsed;
        }
        #endregion


        private List<(string Project, string ProjectLeader, string CompletionStatus, bool IsCompleted)> GetYearProjectGrid(int year, string name = null)
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

                List<(string Project, string ProjectLeader, string CompletionStatus, bool IsCompleted)> values =
                    new List<(string Project, string ProjectLeader, string CompletionStatus, bool IsCompleted)>();

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

                    values.Add((project.ProjectName, project.LeaderName, completionStatus, CompleteStatus));
                }

                return values;
            }
        }

        private void ShowToast(int duration = 10000)
        {
            // 自动关闭
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(duration) };
            timer.Tick += (sender, args) =>
            {
                timer.Stop();
                IsOpen = false;
            };
            timer.Start();
        }

        #endregion
    }
}
