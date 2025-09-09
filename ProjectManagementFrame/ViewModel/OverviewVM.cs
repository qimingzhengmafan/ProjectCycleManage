using Page_Navigation_App.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProjectManagementFrame.ViewModel
{
    public class OverviewVM:ViewModelBase
    {

        public ICommand First { get; set; }

        private string _currentyear = "2025";
        public string CurrentYear
        {
            get => _currentyear;
            set
            {
                _currentyear = value;
            }
        }

        public AllProjects _allprojectsinformation = new AllProjects();
        public AllProjects AllProjectsInformation
        {
            get => _allprojectsinformation;
            set => _allprojectsinformation = value;
        }

        public AllProjects _projectstage = new AllProjects();
        public AllProjects ProjectStage
        {
            get => _projectstage;
            set => _projectstage = value;
        }
        public OverviewVM()
        {
            First = new RelayCommand(FirstIcommand);
        }

        private void FirstIcommand(object obj)
        {
            MessageBox.Show("Connected");
        }
    }
}
