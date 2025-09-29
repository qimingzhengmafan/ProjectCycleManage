using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
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

        private List<string> _people = new List<string>();
        public List<string> People
        {
            get => _people;
            set => _people = value;
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


            //Task.Run(async () => {
            //    var (_index2, strings) = await getdata();
            //    People = strings;
            //});
            var (_index2, strings) = getdata();
            People = strings;

            for (int i = 2022; i <= year; i++)
            {
                TreeModel FirstYear = new TreeModel();
                FirstYear.LevelOne = i.ToString();
                FirstYearIList.LevelTwo.Add(FirstYear);
            }
            First.LevelTwo.Add(FirstYearIList);
            

            for (int i = 2022; i <= year; i++)
            {
                TreeModel SecondYear = new TreeModel();
                SecondYear.LevelOne = i.ToString();
                SecondYearList.LevelTwo.Add(SecondYear);
            }
            Second.LevelTwo.Add(SecondYearList);
            


            TreeModel FirstPeopleList = new TreeModel();
            TreeModel SecondPeopleList = new TreeModel();
            FirstPeopleList.LevelOne = "人员";
            SecondPeopleList.LevelOne = "人员";

            foreach (var item in People)
            {
                TreeModel peoplelist1 = new TreeModel();
                peoplelist1.LevelOne = item.ToString();
                FirstPeopleList.LevelTwo.Add(peoplelist1);
            }
            First.LevelTwo.Add(FirstPeopleList);



            TreeModel1.LevelTwo.Add(First);
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


        private (int count, List<string>) getdata()
        {
            using (var context = new ProjectContext())
            {
                // 获取总数据条数
                int totalCount = context.PeopleTable.Count();
                Console.WriteLine($"总共有 {totalCount} 条数据");

                // 获取所有人的名字
                List<string> allNames =context.PeopleTable
                    .Select(p => p.PeopleName)
                    .ToList();
                return (totalCount, allNames);
            }
        }
    }
}

