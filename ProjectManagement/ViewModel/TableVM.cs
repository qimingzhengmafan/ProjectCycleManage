using ProjectManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.ViewModel
{
    public class TableVM
    {
        public ProjectContext Context { get; set; }
        private TreeViewModel _treeViewModel = new TreeViewModel();
        public TreeViewModel TreeViewModel
        {
            get => _treeViewModel;
            set => _treeViewModel = value;
        }


        public DetailedInformation detailedInformation = new DetailedInformation();

        public TableVM()
        {

        }

    }
}
