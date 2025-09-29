using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeViewTest
{
    public class TreeModel
    {
        public string? Name { get; set; }
        public ObservableCollection<TreeModel> Children { get; set; } = new ObservableCollection<TreeModel>();
    }
}
