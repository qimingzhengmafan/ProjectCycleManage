using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCycleManage.ViewModel
{
    public partial class MainVM:ObservableObject
    {
        [ObservableProperty]
        private string _loginpersonname;

        /// <summary>
        /// 登陆者级别
        /// </summary>
        [ObservableProperty]
        private int _loginpersonnamegrade = 2;

        private OverviewVM _overviewvm;
        public OverviewVM OverView
        {
            get => _overviewvm;
            set => _overviewvm = value;
        }

        public MainVM()
        {
            _overviewvm = new OverviewVM(_loginpersonnamegrade);
        }
    }
}
