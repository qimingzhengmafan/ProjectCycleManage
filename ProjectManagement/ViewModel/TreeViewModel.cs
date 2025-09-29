using ProjectManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.ViewModel
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
            DateTime currenttime = DateTime.Now;
            int year = currenttime.Year;

            TreeModel First = new TreeModel();
            TreeModel Second = new TreeModel();
            First.LevelOne = "工程";

            Second.LevelOne = "设备";

            TreeModel FirstYearIList = new TreeModel();
            TreeModel SecondYearList = new TreeModel();
            FirstYearIList.LevelOne = "年份";
            SecondYearList.LevelOne = "年份";



            for (int i = 2022; i <= year; i++)
            {
                TreeModel FirstYear = new TreeModel();
                FirstYear.LevelOne = i.ToString();
                FirstYearIList.LevelTwo.Add(FirstYear);
            }
            First.LevelTwo.Add(FirstYearIList);
            TreeModel1.LevelTwo.Add(First);

            for (int i = 2022; i <= year; i++)
            {
                TreeModel SecondYear = new TreeModel();
                SecondYear.LevelOne = i.ToString();
                SecondYearList.LevelTwo.Add(SecondYear);
            }
            Second.LevelTwo.Add(SecondYearList);
            TreeModel1.LevelTwo.Add(Second);




            //for (int i = 0; i < 5; i++)
            //{
            //    TreeModel treeViewModel = new TreeModel();
            //    treeViewModel.LevelOne = $"第{i + 1}级";
            //    for (int j = 0; j < 5; j++)
            //    {
            //        TreeModel treeViewModel1 = new TreeModel();
            //        treeViewModel1.LevelOne = $"第{i + 1}----{j + 1}级";
            //        treeViewModel.LevelTwo.Add(treeViewModel1);
            //    }
            //    TreeModel1.LevelTwo.Add(treeViewModel);
            //}

        }
    }
}

