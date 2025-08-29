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
