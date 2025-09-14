using Page_Navigation_App.Utilities;
using ScrollControlProject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementFrame.ViewModel
{
    public class PersonalDataVM :ViewModelBase
    {
        private AllProjects _personalprojectsinformation = new AllProjects();
        public AllProjects PersonalProjectsInformation
        {
            get => _personalprojectsinformation;
            set => _personalprojectsinformation = value;
        }

        private string _kpidata = "96";
        public string KPIData
        {
            get => _kpidata;
            set
            {
                _kpidata = value;
                OnPropertyChanged();
            }
        }

        public PersonalDataVM()
        {

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
                Materials = "图纸001",
                Owner = "负责人：C"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                Progress = 30,
                Materials = "图纸001",
                Owner = "负责人：D"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                Progress = 30,
                Materials = "图纸001",
                Owner = "负责人：E"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                Progress = 30,
                Materials = "图纸001",
                Owner = "负责人：F"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                Progress = 30,
                Materials = "图纸001",
                Owner = "负责人：G"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                Progress = 30,
                Materials = "图纸001",
                Owner = "负责人：H"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                Progress = 30,
                Materials = "图纸001",
                Owner = "负责人：I"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                Progress = 30,
                Materials = "图纸001",
                Owner = "负责人：J"
            },
            new SeamlessLoopingScroll.ProjectItem
            {
                IsTimeout = true,
                ProjectName = "紧急：产线升级",
                Progress = 30,
                Materials = "图纸001",
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
