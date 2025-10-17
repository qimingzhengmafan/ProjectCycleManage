using CommunityToolkit.Mvvm.ComponentModel;
using ProjectManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.ViewModel
{
    public partial class TableVM: ObservableObject
    {
        public ProjectContext Context { get; set; }
        private TreeViewModel _treeViewModel = new TreeViewModel();
        private bool IsShow { get; set; }

        [ObservableProperty]
        private int _showwidth = 0;

        [ObservableProperty]
        private DetailedInformation _detailedInformationvm = new DetailedInformation();


        public TreeViewModel TreeViewModel
        {
            get => _treeViewModel;
            set => _treeViewModel = value;
        }
        

        private void ShowingCtrl()
        {
            IsShow = !IsShow;
            if (IsShow)
            {
                Showwidth = 900;
            }
            else
            {
                Showwidth = 0;
            }
        }


        //public DetailedInformation detailedInformation = new DetailedInformation();

        public TableVM()
        {
            DetailedInformationvm.YourDataCollection[0].Detailedinformationfun = ShowingCtrl;
        }



        #region OtherFun




        #endregion

    }
}
