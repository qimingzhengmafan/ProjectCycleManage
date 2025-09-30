using Page_Navigation_App.Utilities;
using ProjectManagement.Data;
using ScrollControlProjectnetcore;
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
    public class OverviewVM:ViewModelBase
    {
        public ProjectContext Context { get; set; }
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

        private string _currentyear = "2025";
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

        private ObservableCollection<SeamlessLoopingScroll.ProjectItem> _personalprojectslist = new ObservableCollection<SeamlessLoopingScroll.ProjectItem>()
        {
            new SeamlessLoopingScroll.ProjectItem
            {
                ProjectName = "紧急：产线升级",
                Progress = 30,
                Materials = "图纸001",
                Owner = "负责人：A"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                ProjectName = "常规：设备维护",
                Progress = 65,
                Materials = "手册002",
                Owner = "负责人：B"
            },
            new SeamlessLoopingScroll.ProjectItem()
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                Progress = 30,
                Materials = "图纸1 规范2",
                Owner = "负责人：C"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                Progress = 30,
                Materials = "图纸1 规范2",
                Owner = "负责人：D"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                Progress = 30,
                Materials = "图纸1 规范2",
                Owner = "负责人：E"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                Progress = 30,
                Materials = "图纸1 规范2",
                Owner = "负责人：F"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                Progress = 30,
                Materials = "图纸1 规范2",
                Owner = "负责人：G"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                Progress = 30,
                Materials = "图纸1 规范2",
                Owner = "负责人：H"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                Progress = 30,
                Materials = "图纸1 规范2",
                Owner = "负责人：I"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                Progress = 30,
                Materials = "图纸1 规范2",
                Owner = "负责人：J"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                Progress = 30,
                Materials = "图纸1 规范2",
                Owner = "负责人：K"
            }
        };

        public ObservableCollection<SeamlessLoopingScroll.ProjectItem> PersonalProjectsList
        {
            get => _personalprojectslist;
            set => _personalprojectslist = value;
        }



        public OverviewVM()
        {
            //First = new RelayCommand(FirstIcommand);
            VisibilityICommand_OverView = new RelayCommand(OverViewICommandFun);
            VisibilityICommand_ZhuChengXu = new RelayCommand(ZhuChengXuICommandFun);
            VisibilityICommand_DongXin = new RelayCommand(DongXinICommandFun);
            VisibilityICommand_PeiTao = new RelayCommand(PeiTaoICommandFun);
            VisibilityICommand_JiangChen = new RelayCommand(JiangChenICommandFun);
            VisibilityICommand_WangJiaHao = new RelayCommand(WangJiaHaoICommandFun);
            VisibilityICommand_ZhangYuanYuan = new RelayCommand(ZhangYuanYuanICommandFun);
            VisibilityICommand_Yanxin = new RelayCommand(YanxinICommandFun);

        }

        private void FirstIcommand(object obj)
        {
            MessageBox.Show("Connected");
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
    }
}
