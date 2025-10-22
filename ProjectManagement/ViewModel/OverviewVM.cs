using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.EntityFrameworkCore;
using Page_Navigation_App.Utilities;
using ProjectManagement.Data;
using ProjectManagement.Model;
using ScrollControlProjectnetcore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProjectManagement.ViewModel
{
    public class OverviewVM: ObservableObject
    {
        public ProjectContext Context = new ProjectContext();

        private ContextModel _contextmodel;


        #region ICommand

        #region VisibilityICommand

        public ICommand VisibilityICommand_OverView { get; set; }
        public ICommand VisibilityICommand_ZhuChengXu { get; set; }
        public ICommand VisibilityICommand_DongXin { get; set; }
        public ICommand VisibilityICommand_PeiTao { get; set; }
        public ICommand VisibilityICommand_JiangChen { get; set; }
        public ICommand VisibilityICommand_WangJiaHao { get; set; }
        public ICommand VisibilityICommand_ZhangYuanYuan { get; set; }
        public ICommand VisibilityICommand_Yanxin { get; set; }

        #endregion

        //public ICommand First { get; set; }
        #endregion
        public string Department { get; set; }
        public string People1 {  get; set; }
        public string People2 { get; set; }
        public string People3 { get; set; }
        public string People4 { get; set; }
        public string People5 { get; set; }
        public string People6 { get; set; }
        public string People7 { get; set; }
        public string People8 { get; set; }

        private string _currentyear;
        public string CurrentYear
        {
            get => _currentyear;
            set
            {
                _currentyear = value;
            }
        }

        #region PersonalUIVisibility

        #region 总览

        private Visibility _personalvisibiityvm_overview = Visibility.Visible;
        public Visibility PersonalVisibilityVM_OverView
        {
            get => _personalvisibiityvm_overview;
            set
            {
                _personalvisibiityvm_overview = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region 朱成旭

        private Visibility _personalvisibiityvm_chengxuzhu = Visibility.Collapsed;
        public Visibility PersonalVisibilityVM_ChengXuZhu
        {
            get => _personalvisibiityvm_chengxuzhu;
            set
            {
                _personalvisibiityvm_chengxuzhu = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region 董鑫
        private Visibility _personalvisibiityvm_dongxin = Visibility.Collapsed;
        public Visibility PersonalVisibilityVM_DongXin
        {
            get => _personalvisibiityvm_dongxin;
            set
            {
                _personalvisibiityvm_dongxin = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 裴涛
        private Visibility _personalvisibiityvm_peitao = Visibility.Collapsed;
        public Visibility PersonalVisibilityVM_PeiTao
        {
            get => _personalvisibiityvm_peitao;
            set
            {
                _personalvisibiityvm_peitao = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 江琛
        private Visibility _personalvisibiityvm_jiangchen = Visibility.Collapsed;
        public Visibility PersonalVisibilityVM_JiangChen
        {
            get => _personalvisibiityvm_jiangchen;
            set
            {
                _personalvisibiityvm_jiangchen = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 王嘉豪
        private Visibility _personalvisibiityvm_wangjiahao = Visibility.Collapsed;
        public Visibility PersonalVisibilityVM_WangJiaHao
        {
            get => _personalvisibiityvm_wangjiahao;
            set
            {
                _personalvisibiityvm_wangjiahao = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 张园园
        private Visibility _personalvisibiityvm_zhangyuanyuan = Visibility.Collapsed;
        public Visibility PersonalVisibilityVM_ZhangYuanYuan
        {
            get => _personalvisibiityvm_zhangyuanyuan;
            set
            {
                _personalvisibiityvm_zhangyuanyuan = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 严鑫
        private Visibility _personalvisibiityvm_yanxin = Visibility.Collapsed;
        public Visibility PersonalVisibilityVM_YanXin
        {
            get => _personalvisibiityvm_yanxin;
            set
            {
                _personalvisibiityvm_yanxin = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region PeopleChange

        #region 朱成旭

        private PersonalDataVM _personaldatavm_chengxuzhu = new PersonalDataVM();
        public PersonalDataVM PersonalDataVM_ChengXuZhu
        {
            get => _personaldatavm_chengxuzhu;
            set => _personaldatavm_chengxuzhu = value;
        }

        #endregion

        #region 董鑫
        private PersonalDataVM _personaldatavm_dongxin = new PersonalDataVM();
        public PersonalDataVM PersonalDataVM_DongXin
        {
            get => _personaldatavm_dongxin;
            set => _personaldatavm_dongxin = value;
        }
        #endregion

        #region 裴涛
        private PersonalDataVM _personaldatavm_peitao = new PersonalDataVM();
        public PersonalDataVM PersonalDataVM_PeiTao
        {
            get => _personaldatavm_peitao;
            set => _personaldatavm_peitao = value;
        }
        #endregion

        #region 江琛
        private PersonalDataVM _personaldatavm_jiangchen = new PersonalDataVM();
        public PersonalDataVM PersonalDataVM_JiangChen
        {
            get => _personaldatavm_jiangchen;
            set => _personaldatavm_jiangchen = value;
        }
        #endregion

        #region 王嘉豪
        private PersonalDataVM _personaldatavm_wangjiahao = new PersonalDataVM();
        public PersonalDataVM PersonalDataVM_WangJiaHao
        {
            get => _personaldatavm_wangjiahao;
            set => _personaldatavm_wangjiahao = value;
        }
        #endregion

        #region 张园园
        private PersonalDataVM _personaldatavm_zhangyuanyuan = new PersonalDataVM();
        public PersonalDataVM PersonalDataVM_ZhangYuanYuan
        {
            get => _personaldatavm_zhangyuanyuan;
            set => _personaldatavm_zhangyuanyuan = value;
        }
        #endregion

        #region 严鑫
        private PersonalDataVM _personaldatavm_yanxin = new PersonalDataVM();
        public PersonalDataVM PersonalDataVM_YanXin
        {
            get => _personaldatavm_yanxin;
            set => _personaldatavm_yanxin = value;
        }

        #endregion




        #endregion



        private AllProjects _allprojectsinformation = new AllProjects();
        public AllProjects AllProjectsInformation
        {
            get => _allprojectsinformation;
            set => _allprojectsinformation = value;
        }

        private AllProjects _projectstage = new AllProjects();
        public AllProjects ProjectStage
        {
            get => _projectstage;
            set => _projectstage = value;
        }

        private ObservableCollection<SeamlessLoopingScroll.ProjectItem> _allprojectslist = new ObservableCollection<SeamlessLoopingScroll.ProjectItem>();

        public ObservableCollection<SeamlessLoopingScroll.ProjectItem> AllProjectsList
        {
            get => _allprojectslist;
            set => _allprojectslist = value;
        }



        public OverviewVM()
        {
            _contextmodel = new ContextModel(Context);

            //First = new RelayCommand(FirstIcommand);
            VisibilityICommand_OverView = new RelayCommand(OverViewICommandFun);
            VisibilityICommand_ZhuChengXu = new RelayCommand(ZhuChengXuICommandFun);
            VisibilityICommand_DongXin = new RelayCommand(DongXinICommandFun);
            VisibilityICommand_PeiTao = new RelayCommand(PeiTaoICommandFun);
            VisibilityICommand_JiangChen = new RelayCommand(JiangChenICommandFun);
            VisibilityICommand_WangJiaHao = new RelayCommand(WangJiaHaoICommandFun);
            VisibilityICommand_ZhangYuanYuan = new RelayCommand(ZhangYuanYuanICommandFun);
            VisibilityICommand_Yanxin = new RelayCommand(YanxinICommandFun);

            CurrentYear = DateTime.Now.Year.ToString();

            Task.Run(() =>
            {
                #region 总览
                var data = GetProjectGrid1(DateTime.Now.Year);
                foreach (var item in data)
                {
                    AllProjectsList.Add(new SeamlessLoopingScroll.ProjectItem()
                    {
                        ProjectName = item.Project,
                        CountDown = item.DaysDiff,
                        FileProgress = item.FileProgress,
                        Owner = item.ProjectLeader
                    });
                }
                #endregion


                #region 朱成绪
                var personaldata_zhuchengxu = GetPPersonalProjectGrid(DateTime.Now.Year, "朱成绪");
                //PersonalDataVM_ChengXuZhu
                if (personaldata_zhuchengxu.Count != 0)
                {
                    PersonalDataVM_ChengXuZhu.PersonalProjectsList.Clear();
                    foreach (var item in personaldata_zhuchengxu)
                    {
                        PersonalDataVM_ChengXuZhu.PersonalProjectsList.Add(new SeamlessLoopingScroll.ProjectItem()
                        {
                            ProjectName = item.Project,
                            CountDown = item.DaysDiff,
                            FileProgress = item.FileProgress,
                            Owner = item.ProjectLeader
                        });

                    }
                }
                (int AllProjects_zhuchengxu, int ComProjects_zhuchengxu) = GetLeaderCompleteProjects(DateTime.Now.Year, "朱成绪");
                PersonalDataVM_ChengXuZhu.Allprojectsnum = AllProjects_zhuchengxu;
                PersonalDataVM_ChengXuZhu.CompleteProjectsNum = ComProjects_zhuchengxu;
                var data_zhuchengxu = _contextmodel.GetPersonalProjectsStatues(DateTime.Now.Year, "朱成绪");
                List<string> names = new List<string>();
                List<int> num = new List<int>();
                int _index1 = 0;
                foreach (var item in data_zhuchengxu)
                {
                    names.Add(item.Status);
                    num.Add(item.count);
                }
                PersonalDataVM_ChengXuZhu.Personalprojectsinformation.Series = num.AsPieSeries((value, series) =>
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
                #endregion

                #region 董鑫
                var personaldata_dongxin = GetPPersonalProjectGrid(DateTime.Now.Year, "董鑫");
                //PersonalDataVM_ChengXuZhu  PersonalDataVM_DongXin
                if (personaldata_dongxin.Count != 0)
                {
                    PersonalDataVM_DongXin.PersonalProjectsList.Clear();
                    foreach (var item in personaldata_dongxin)
                    {
                        PersonalDataVM_DongXin.PersonalProjectsList.Add(new SeamlessLoopingScroll.ProjectItem()
                        {
                            ProjectName = item.Project,
                            CountDown = item.DaysDiff,
                            FileProgress = item.FileProgress,
                            Owner = item.ProjectLeader
                        });

                    }
                }

                (int AllProjects_dongxin, int ComProjects_dongxin) = GetLeaderCompleteProjects(DateTime.Now.Year, "董鑫");
                PersonalDataVM_DongXin.Allprojectsnum = AllProjects_dongxin;
                PersonalDataVM_DongXin.CompleteProjectsNum = ComProjects_dongxin;
                var data_dongxin = _contextmodel.GetPersonalProjectsStatues(DateTime.Now.Year, "董鑫");
                List<string> namesdongxin = new List<string>();
                List<int> numdongxin = new List<int>();
                _index1 = 0;
                foreach (var item in data_dongxin)
                {
                    namesdongxin.Add(item.Status);
                    numdongxin.Add(item.count);
                }
                PersonalDataVM_DongXin.Personalprojectsinformation.Series = numdongxin.AsPieSeries((value, series) =>
                {
                    series.Name = namesdongxin[_index1++ % namesdongxin.Count];
                    series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle;
                    series.DataLabelsSize = 15;
                    series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
                    series.DataLabelsFormatter =
                       point =>
                           $"{point.Coordinate.PrimaryValue} " + "/ " +
                           $"{point.StackedValue.Total} ";
                    series.ToolTipLabelFormatter = point => $"{point.StackedValue.Share:P2}";
                });
                #endregion

                #region 裴涛
                var personaldata_peitao = GetPFollowCompletePersonalProjectGrid(DateTime.Now.Year, "裴涛");
                //PersonalDataVM_PeiTao
                if (personaldata_peitao.Count != 0)
                {
                    PersonalDataVM_PeiTao.PersonalProjectsList.Clear();
                    foreach (var item in personaldata_peitao)
                    {
                        PersonalDataVM_PeiTao.PersonalProjectsList.Add(new SeamlessLoopingScroll.ProjectItem()
                        {
                            ProjectName = item.Project,
                            CountDown = item.DaysDiff,
                            FileProgress = item.FileProgress,
                            Owner = item.ProjectLeader
                        });

                    }
                }

                (int AllProjects_peitao, int ComProjects_peitao) = GetFollowCompleteProjects(DateTime.Now.Year, "裴涛");
                PersonalDataVM_PeiTao.Allprojectsnum = AllProjects_peitao;
                PersonalDataVM_PeiTao.CompleteProjectsNum = ComProjects_peitao;
                var data_peitao = _contextmodel.GetPersonalProjectsStatues(DateTime.Now.Year, "裴涛");
                List<string> namespeitao = new List<string>();
                List<int> numpeitao = new List<int>();
                _index1 = 0;
                foreach (var item in data_peitao)
                {
                    namespeitao.Add(item.Status);
                    numpeitao.Add(item.count);
                }
                PersonalDataVM_PeiTao.Personalprojectsinformation.Series = numpeitao.AsPieSeries((value, series) =>
                {
                    series.Name = namespeitao[_index1++ % namespeitao.Count];
                    series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle;
                    series.DataLabelsSize = 15;
                    series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
                    series.DataLabelsFormatter =
                       point =>
                           $"{point.Coordinate.PrimaryValue} " + "/ " +
                           $"{point.StackedValue.Total} ";
                    series.ToolTipLabelFormatter = point => $"{point.StackedValue.Share:P2}";
                });
                #endregion

                #region 江琛
                var personaldata_jiangchen = GetPFollowCompletePersonalProjectGrid(DateTime.Now.Year, "裴涛");
                //PersonalDataVM_JiangChen
                if (personaldata_jiangchen.Count != 0)
                {
                    PersonalDataVM_JiangChen.PersonalProjectsList.Clear();
                    foreach (var item in personaldata_jiangchen)
                    {
                        PersonalDataVM_JiangChen.PersonalProjectsList.Add(new SeamlessLoopingScroll.ProjectItem()
                        {
                            ProjectName = item.Project,
                            CountDown = item.DaysDiff,
                            FileProgress = item.FileProgress,
                            Owner = item.ProjectLeader
                        });

                    }
                }

                (int AllProjects_jiangchen, int ComProjects_jiangchen) = GetFollowCompleteProjects(DateTime.Now.Year, "江琛");
                PersonalDataVM_JiangChen.Allprojectsnum = AllProjects_jiangchen;
                PersonalDataVM_JiangChen.CompleteProjectsNum = ComProjects_jiangchen;
                var data_jiangchen = _contextmodel.GetPersonalProjectsStatues(DateTime.Now.Year, "江琛");
                List<string> namesjiangchen = new List<string>();
                List<int> numjiangchen = new List<int>();
                _index1 = 0;
                foreach (var item in data_jiangchen)
                {
                    namesjiangchen.Add(item.Status);
                    numjiangchen.Add(item.count);
                }
                PersonalDataVM_JiangChen.Personalprojectsinformation.Series = numjiangchen.AsPieSeries((value, series) =>
                {
                    series.Name = namesjiangchen[_index1++ % namesjiangchen.Count];
                    series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle;
                    series.DataLabelsSize = 15;
                    series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
                    series.DataLabelsFormatter =
                       point =>
                           $"{point.Coordinate.PrimaryValue} " + "/ " +
                           $"{point.StackedValue.Total} ";
                    series.ToolTipLabelFormatter = point => $"{point.StackedValue.Share:P2}";
                });
                #endregion

                #region 王嘉豪
                //PersonalDataVM_PeiTao
                //personaldata_peitao
                //AllProjects_peitao
                //ComProjects_peitao
                //data_peitao
                //namespeitao
                //numpeitao
                var personaldata_wangjiahao = GetPFollowCompletePersonalProjectGrid(DateTime.Now.Year, "裴涛");
                if (personaldata_wangjiahao.Count != 0)
                {
                    PersonalDataVM_WangJiaHao.PersonalProjectsList.Clear();
                    foreach (var item in personaldata_wangjiahao)
                    {
                        PersonalDataVM_WangJiaHao.PersonalProjectsList.Add(new SeamlessLoopingScroll.ProjectItem()
                        {
                            ProjectName = item.Project,
                            CountDown = item.DaysDiff,
                            FileProgress = item.FileProgress,
                            Owner = item.ProjectLeader
                        });

                    }
                }

                (int AllProjects_wangjiahao, int ComProjects_wangjiahao) = GetFollowCompleteProjects(DateTime.Now.Year, "裴涛");
                PersonalDataVM_WangJiaHao.Allprojectsnum = AllProjects_wangjiahao;
                PersonalDataVM_WangJiaHao.CompleteProjectsNum = ComProjects_wangjiahao;
                var data_wangjiahao = _contextmodel.GetPersonalProjectsStatues(DateTime.Now.Year, "裴涛");
                List<string> nameswangjiahao = new List<string>();
                List<int> numwangjiahao = new List<int>();
                _index1 = 0;
                foreach (var item in data_wangjiahao)
                {
                    nameswangjiahao.Add(item.Status);
                    numwangjiahao.Add(item.count);
                }
                PersonalDataVM_WangJiaHao.Personalprojectsinformation.Series = numwangjiahao.AsPieSeries((value, series) =>
                {
                    series.Name = nameswangjiahao[_index1++ % nameswangjiahao.Count];
                    series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle;
                    series.DataLabelsSize = 15;
                    series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
                    series.DataLabelsFormatter =
                       point =>
                           $"{point.Coordinate.PrimaryValue} " + "/ " +
                           $"{point.StackedValue.Total} ";
                    series.ToolTipLabelFormatter = point => $"{point.StackedValue.Share:P2}";
                });
                #endregion

                #region 张园园
                //PersonalDataVM_PeiTao
                //personaldata_peitao
                //AllProjects_peitao
                //ComProjects_peitao
                //data_peitao
                //namespeitao
                //numpeitao
                var personaldata_zhangyuanyuan = GetPFollowCompletePersonalProjectGrid(DateTime.Now.Year, "裴涛");
                if (personaldata_zhangyuanyuan.Count != 0)
                {
                    PersonalDataVM_ZhangYuanYuan.PersonalProjectsList.Clear();
                    foreach (var item in personaldata_zhangyuanyuan)
                    {
                        PersonalDataVM_ZhangYuanYuan.PersonalProjectsList.Add(new SeamlessLoopingScroll.ProjectItem()
                        {
                            ProjectName = item.Project,
                            CountDown = item.DaysDiff,
                            FileProgress = item.FileProgress,
                            Owner = item.ProjectLeader
                        });

                    }
                }

                (int AllProjects_zhangyuanyuan, int ComProjects_zhangyuanyuan) = GetFollowCompleteProjects(DateTime.Now.Year, "裴涛");
                PersonalDataVM_ZhangYuanYuan.Allprojectsnum = AllProjects_zhangyuanyuan;
                PersonalDataVM_ZhangYuanYuan.CompleteProjectsNum = ComProjects_zhangyuanyuan;
                var data_zhangyuanyuan = _contextmodel.GetPersonalProjectsStatues(DateTime.Now.Year, "裴涛");
                List<string> nameszhangyuanyuan = new List<string>();
                List<int> numzhangyuanyuan = new List<int>();
                _index1 = 0;
                foreach (var item in data_zhangyuanyuan)
                {
                    nameszhangyuanyuan.Add(item.Status);
                    numzhangyuanyuan.Add(item.count);
                }
                PersonalDataVM_ZhangYuanYuan.Personalprojectsinformation.Series = numzhangyuanyuan.AsPieSeries((value, series) =>
                {
                    series.Name = nameszhangyuanyuan[_index1++ % nameszhangyuanyuan.Count];
                    series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle;
                    series.DataLabelsSize = 15;
                    series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
                    series.DataLabelsFormatter =
                       point =>
                           $"{point.Coordinate.PrimaryValue} " + "/ " +
                           $"{point.StackedValue.Total} ";
                    series.ToolTipLabelFormatter = point => $"{point.StackedValue.Share:P2}";
                });
                #endregion

                #region 严鑫
                //PersonalDataVM_PeiTao
                //personaldata_peitao
                //AllProjects_peitao
                //ComProjects_peitao
                //data_peitao
                //namespeitao
                //numpeitao
                var personaldata_yanxin = GetPFollowCompletePersonalProjectGrid(DateTime.Now.Year, "裴涛");
                if (personaldata_yanxin.Count != 0)
                {
                    PersonalDataVM_YanXin.PersonalProjectsList.Clear();
                    foreach (var item in personaldata_yanxin)
                    {
                        PersonalDataVM_YanXin.PersonalProjectsList.Add(new SeamlessLoopingScroll.ProjectItem()
                        {
                            ProjectName = item.Project,
                            CountDown = item.DaysDiff,
                            FileProgress = item.FileProgress,
                            Owner = item.ProjectLeader
                        });

                    }
                }

                (int AllProjects_yanxin, int ComProjects_yanxin) = GetFollowCompleteProjects(DateTime.Now.Year, "裴涛");
                PersonalDataVM_YanXin.Allprojectsnum = AllProjects_yanxin;
                PersonalDataVM_YanXin.CompleteProjectsNum = ComProjects_yanxin;
                var data_yanxin = _contextmodel.GetPersonalProjectsStatues(DateTime.Now.Year, "裴涛");
                List<string> namesyanxin = new List<string>();
                List<int> numyanxin = new List<int>();
                _index1 = 0;
                foreach (var item in data_yanxin)
                {
                    namesyanxin.Add(item.Status);
                    numyanxin.Add(item.count);
                }
                PersonalDataVM_YanXin.Personalprojectsinformation.Series = numyanxin.AsPieSeries((value, series) =>
                {
                    series.Name = namesyanxin[_index1++ % namesyanxin.Count];
                    series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle;
                    series.DataLabelsSize = 15;
                    series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
                    series.DataLabelsFormatter =
                       point =>
                           $"{point.Coordinate.PrimaryValue} " + "/ " +
                           $"{point.StackedValue.Total} ";
                    series.ToolTipLabelFormatter = point => $"{point.StackedValue.Share:P2}";
                });
                #endregion

            });
            

            //_ = GetProjectGrid(2022);


        }

        #region VisibilityIcommandFunc

        private void OverViewICommandFun(object obj)
        {
            PersonalVisibilityVM_OverView = Visibility.Visible;
                
            PersonalVisibilityVM_ChengXuZhu = Visibility.Collapsed;
            PersonalVisibilityVM_DongXin = Visibility.Collapsed;
            PersonalVisibilityVM_PeiTao = Visibility.Collapsed;
            PersonalVisibilityVM_JiangChen = Visibility.Collapsed;
            PersonalVisibilityVM_WangJiaHao = Visibility.Collapsed;
            PersonalVisibilityVM_ZhangYuanYuan = Visibility.Collapsed;
            PersonalVisibilityVM_YanXin = Visibility.Collapsed;
        }
        
        private void ZhuChengXuICommandFun(object obj)
        {
            PersonalVisibilityVM_ChengXuZhu = Visibility.Visible;
                
            PersonalVisibilityVM_OverView = Visibility.Collapsed;
            PersonalVisibilityVM_DongXin = Visibility.Collapsed;
            PersonalVisibilityVM_PeiTao = Visibility.Collapsed;
            PersonalVisibilityVM_JiangChen = Visibility.Collapsed;
            PersonalVisibilityVM_WangJiaHao = Visibility.Collapsed;
            PersonalVisibilityVM_ZhangYuanYuan = Visibility.Collapsed;
            PersonalVisibilityVM_YanXin = Visibility.Collapsed;
        }
        
        private void DongXinICommandFun(object obj)
        {
            PersonalVisibilityVM_DongXin = Visibility.Visible;
                
            PersonalVisibilityVM_ChengXuZhu = Visibility.Collapsed;
            PersonalVisibilityVM_OverView = Visibility.Collapsed;
            PersonalVisibilityVM_PeiTao = Visibility.Collapsed;
            PersonalVisibilityVM_JiangChen = Visibility.Collapsed;
            PersonalVisibilityVM_WangJiaHao = Visibility.Collapsed;
            PersonalVisibilityVM_ZhangYuanYuan = Visibility.Collapsed;
            PersonalVisibilityVM_YanXin = Visibility.Collapsed;
        }
        
        private void PeiTaoICommandFun(object obj)
        {
            PersonalVisibilityVM_PeiTao = Visibility.Visible;
                
            PersonalVisibilityVM_ChengXuZhu = Visibility.Collapsed;
            PersonalVisibilityVM_DongXin = Visibility.Collapsed;
            PersonalVisibilityVM_OverView = Visibility.Collapsed;
            PersonalVisibilityVM_JiangChen = Visibility.Collapsed;
            PersonalVisibilityVM_WangJiaHao = Visibility.Collapsed;
            PersonalVisibilityVM_ZhangYuanYuan = Visibility.Collapsed;
            PersonalVisibilityVM_YanXin = Visibility.Collapsed;
        }
        
        private void JiangChenICommandFun(object obj)
        {
            PersonalVisibilityVM_JiangChen = Visibility.Visible;
                
            PersonalVisibilityVM_ChengXuZhu = Visibility.Collapsed;
            PersonalVisibilityVM_DongXin = Visibility.Collapsed;
            PersonalVisibilityVM_PeiTao = Visibility.Collapsed;
            PersonalVisibilityVM_OverView = Visibility.Collapsed;
            PersonalVisibilityVM_WangJiaHao = Visibility.Collapsed;
            PersonalVisibilityVM_ZhangYuanYuan = Visibility.Collapsed;
            PersonalVisibilityVM_YanXin = Visibility.Collapsed;
        }
        
        private void WangJiaHaoICommandFun(object obj)
        {
            PersonalVisibilityVM_WangJiaHao = Visibility.Visible;
                
            PersonalVisibilityVM_ChengXuZhu = Visibility.Collapsed;
            PersonalVisibilityVM_DongXin = Visibility.Collapsed;
            PersonalVisibilityVM_PeiTao = Visibility.Collapsed;
            PersonalVisibilityVM_JiangChen = Visibility.Collapsed;
            PersonalVisibilityVM_OverView = Visibility.Collapsed;
            PersonalVisibilityVM_ZhangYuanYuan = Visibility.Collapsed;
            PersonalVisibilityVM_YanXin = Visibility.Collapsed;
        }
        
        private void ZhangYuanYuanICommandFun(object obj)
        {
            PersonalVisibilityVM_ZhangYuanYuan = Visibility.Visible;
                
            PersonalVisibilityVM_ChengXuZhu = Visibility.Collapsed;
            PersonalVisibilityVM_DongXin = Visibility.Collapsed;
            PersonalVisibilityVM_PeiTao = Visibility.Collapsed;
            PersonalVisibilityVM_JiangChen = Visibility.Collapsed;
            PersonalVisibilityVM_WangJiaHao = Visibility.Collapsed;
            PersonalVisibilityVM_OverView = Visibility.Collapsed;
            PersonalVisibilityVM_YanXin = Visibility.Collapsed;
        }
        
        private void YanxinICommandFun(object obj)
        {
            PersonalVisibilityVM_YanXin = Visibility.Visible;
                
            PersonalVisibilityVM_ChengXuZhu = Visibility.Collapsed;
            PersonalVisibilityVM_DongXin = Visibility.Collapsed;
            PersonalVisibilityVM_PeiTao = Visibility.Collapsed;
            PersonalVisibilityVM_JiangChen = Visibility.Collapsed;
            PersonalVisibilityVM_WangJiaHao = Visibility.Collapsed;
            PersonalVisibilityVM_ZhangYuanYuan = Visibility.Collapsed;
            PersonalVisibilityVM_OverView = Visibility.Collapsed;
        }

        #endregion


        #region OtherFun

        private List<(string Project, int DaysDiff, int FileProgress, string ProjectLeader)> GetProjectGrid1(int year)
        {
            using (var context = new ProjectContext())
            {
                var projects = context.Projects
                    .Where(p => p.Year == year)
                    .Include(p => p.ProjectLeader) // 加载负责人导航属性
                    .Select(p => new
                    {
                        p.ProjectName,
                        p.DaysDiff,
                        p.FileProgress,
                        LeaderName = p.ProjectLeader.PeopleName // 获取负责人姓名
                    })
                    .OrderBy(p => p.ProjectName) // 按项目名称排序
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
                return values;

            }
        }

        private List<(string Project, int DaysDiff, int FileProgress, string ProjectLeader)> GetPPersonalProjectGrid(int year , string Name)
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


        /// <summary>
        /// 获取项目跟进人
        /// </summary>
        /// <param name="year"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        private (int AllProjectNum, int Completeprojects) GetFollowCompleteProjects(int year, string Name)
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

        private List<(string Project, int DaysDiff, int FileProgress, string ProjectLeader)> GetPFollowCompletePersonalProjectGrid(int year, string Name)
        {
            using (var context = new ProjectContext())
            {
                var projects = context.Projects
                    .Where(p => p.Year == year && p.projectfollowupperson.PeopleName == Name)
                    .Include(p => p.projectfollowupperson)
                    .Select(p => new
                    {
                        p.ProjectName,
                        p.DaysDiff,
                        p.FileProgress,
                        LeaderName = p.projectfollowupperson.PeopleName
                    })
                    .OrderBy(p => p.ProjectName)
                    .ToList();

                List<(string Project, int DaysDiff, int FileProgress, string projectfollowupperson)> values =
                    new List<(string Project, int DaysDiff, int FileProgress, string projectfollowupperson)>();

                foreach (var project in projects)
                {
                    (string Project, int DaysDiff, int FileProgress, string projectfollowupperson) projectvaule;
                    projectvaule.Project = project.ProjectName;
                    projectvaule.FileProgress = (int)project.FileProgress.GetValueOrDefault();
                    projectvaule.DaysDiff = project.DaysDiff.GetValueOrDefault();
                    projectvaule.projectfollowupperson = project.LeaderName;
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

        #endregion
    }
}
