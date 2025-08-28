using Page_Navigation_App.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectManagementFrame.ViewModel
{
    public class MainVM:ViewModelBase
    {
        private string _currentyear = "2025";
        public string CurrentYear
        {
            get => _currentyear;
            set
            {
                _currentyear = value;
                OnPropertyChanged();
            }
        }
    }
}
