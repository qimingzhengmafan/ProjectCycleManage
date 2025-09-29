using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeViewTest
{
    public class TreeViewModel
    {
        private TreeModel _treeModel = new TreeModel();
        public TreeModel TreeModel1
        {
            get { return _treeModel; }
            set { _treeModel = value; }
        }

        public TreeViewModel()
        {
            InitTree();
        }
        public void InitTree()
        {
            for (int i = 0; i < 5; i++)
            {
                TreeModel treeViewModel = new TreeModel();
                treeViewModel.Name = $"第{i + 1}级";
                for (int j = 0; j < 5; j++)
                {
                    TreeModel treeViewModel1 = new TreeModel();
                    treeViewModel1.Name = $"第{i + 1}----{j + 1}级";
                    treeViewModel.Children.Add(treeViewModel1);
                }
                TreeModel1.Children.Add(treeViewModel);
            }

        }
    }
}
