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
        private OverviewVM _overviewvm = new OverviewVM();
        public OverviewVM OverView
        {
            get => _overviewvm;
            set => _overviewvm = value;
        }
    }
}
