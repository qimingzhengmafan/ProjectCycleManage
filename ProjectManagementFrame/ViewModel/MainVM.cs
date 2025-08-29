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
    public class MainVM:ViewModelBase
    {
        public ICommand ConnectCommand { get; set; }

        private OverviewVM _overview = new OverviewVM();
        public OverviewVM OverviewVMInfor
        {
            get => _overview; 
            set => _overview = value;
        }

        public MainVM()
        {
            ConnectCommand = new RelayCommand(ConnectIcommand);
        }



        private void ConnectIcommand(object obj)
        {
            MessageBox.Show("Connected");
        }
    }
}
