using CommunityToolkit.Mvvm.ComponentModel;
using ProjectManagement.Data;
using ProjectManagement.ViewModel;
using ScrollControlProjectnetcore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSide.ViewModel
{
    public partial class PersonalDataVM : ObservableObject
    {
        public ProjectContext Context { get; set; }

        [ObservableProperty]
        private AllProjects _personalprojectsinformation = new AllProjects();

        private int _allprojectsnum = 0;
        public int Allprojectsnum
        {
            get => _allprojectsnum;
            set
            {
                _allprojectsnum = value;
                Unfinished = _allprojectsnum - CompleteProjectsNum;
                Projectcompletionrate = CompleteProjectsNum / _allprojectsnum * 100;
                OnPropertyChanged();
            }
        }

        private int _completeProjectsNum = 0;
        public int CompleteProjectsNum
        {
            get => _completeProjectsNum;
            set
            {
                _completeProjectsNum = value;
                Unfinished = Allprojectsnum - _completeProjectsNum;
                Projectcompletionrate = _completeProjectsNum / Allprojectsnum * 100;
                OnPropertyChanged();
            }
        }



        /// <summary>
        /// 项目完成率
        /// </summary>
        [ObservableProperty]
        private int _projectcompletionrate = 0;

        /// <summary>
        /// 未完成
        /// </summary>
        [ObservableProperty]
        private int _unfinished = 0;




        public AllProjects PersonalProjectsInformation
        {
            get => _personalprojectsinformation;
            set => _personalprojectsinformation = value;
        }

        public PersonalDataVM()
        {

        }

        private ObservableCollection<SeamlessLoopingScroll.ProjectItem> _personalprojectslist = new ObservableCollection<SeamlessLoopingScroll.ProjectItem>()
        {
            new SeamlessLoopingScroll.ProjectItem
            {
                ProjectName = "紧急：产线升级",
                FileProgress = 30,
                CountDown = 50,
                Owner = "负责人：A"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                ProjectName = "常规：设备维护",
                FileProgress = 65,
                CountDown = 50,
                Owner = "负责人：B"
            },
            new SeamlessLoopingScroll.ProjectItem()
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                FileProgress = 30,
                CountDown = 50,
                Owner = "负责人：C"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                FileProgress = 30,
                CountDown = 50,
                Owner = "负责人：D"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                FileProgress = 30,
                CountDown = 50,
                Owner = "负责人：E"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                FileProgress = 30,
                CountDown = 50,
                Owner = "负责人：F"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                FileProgress = 30,
                CountDown = 50,
                Owner = "负责人：G"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                FileProgress = 30,
                CountDown = 50,
                Owner = "负责人：H"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                FileProgress = 30,
                CountDown = 50,
                Owner = "负责人：I"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                FileProgress = 30,
                CountDown = 50,
                Owner = "负责人：J"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                FileProgress = 30,
                CountDown = 50,
                Owner = "负责人：K"
            }
        };

        public ObservableCollection<SeamlessLoopingScroll.ProjectItem> PersonalProjectsList
        {
            get => _personalprojectslist;
            set => _personalprojectslist = value;
        }
    }
}
