using Page_Navigation_App.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementFrame.ViewModel
{
    public class OverviewVM:ViewModelBase
    {
        private string _currentyear = "2023";
        public string CurrentYear
        {
            get => _currentyear;
            set
            {
                _currentyear = value;
            }
        }
    }
}
