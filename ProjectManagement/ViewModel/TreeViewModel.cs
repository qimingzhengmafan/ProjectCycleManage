using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProjectManagement.ViewModel
{
    public partial class TreeViewModel : ObservableObject
    {
        // 定义节点点击事件
        public event Action<TreeModel> NodeClicked;
        private TreeModel _treeModel = new TreeModel();
        public TreeModel TreeModel1
        {
            get { return _treeModel; }
            set
            {
                _treeModel = value;
            }

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
            First.LevelOne = "📁  工程项目";
            First.Level = 1;

            Second.LevelOne = "📁  工程维修";
            Second.Level = 1;

            TreeModel FirstYearIList = new TreeModel();
            TreeModel SecondYearList = new TreeModel();
            FirstYearIList.LevelOne = "📅  年份";
            FirstYearIList.Level = 2;
            SecondYearList.LevelOne = "📅  年份";
            SecondYearList.Level = 2;


            //Task.Run(async () => {
            //    var (_index2, strings) = await getdata();
            //    People = strings;
            //});
            var (_index2, strings) = getdata();
            People = strings;

            for (int i = 2022; i <= year; i++)
            {
                TreeModel FirstYear = new TreeModel();
                FirstYear.LevelOne = "📆  " + i.ToString();
                FirstYear.Level = 3;
                FirstYearIList.LevelTwo.Add(FirstYear);
            }
            First.LevelTwo.Add(FirstYearIList);
            

            for (int i = 2022; i <= year; i++)
            {
                TreeModel SecondYear = new TreeModel();
                SecondYear.LevelOne = "📆  " + i.ToString();
                SecondYear.Level = 3;
                SecondYearList.LevelTwo.Add(SecondYear);
            }
            Second.LevelTwo.Add(SecondYearList);
            


            TreeModel FirstPeopleList = new TreeModel();
            TreeModel SecondPeopleList = new TreeModel();
            FirstPeopleList.LevelOne = "👥  人员";
            FirstPeopleList.Level = 2;
            SecondPeopleList.LevelOne = "👥  人员";
            SecondPeopleList.Level = 2;

            foreach (var item in People)
            {
                TreeModel peoplelist1 = new TreeModel();
                peoplelist1.LevelOne = "👤  " + item.ToString();
                peoplelist1.Level = 3;
                FirstPeopleList.LevelTwo.Add(peoplelist1);
            }
            First.LevelTwo.Add(FirstPeopleList);



            TreeModel1.LevelTwo.Add(First);
            TreeModel1.LevelTwo.Add(Second);

        }

        private ICommand _nodeClickCommand;
        public ICommand NodeClickCommand => _nodeClickCommand ??= new RelayCommand<TreeModel>(OnNodeClicked);

        private void OnNodeClicked(TreeModel clickedNode)
        {
            if (clickedNode != null)
            {
                // 获取被点击的节点信息
                string nodeName = clickedNode.LevelOne ?? "未知节点";
                int level = clickedNode.Level;
                
                // 处理点击逻辑
                MessageBox.Show($"点击了节点: {nodeName}, 层级: {level}");
                
                // 触发节点点击事件
                NodeClicked?.Invoke(clickedNode);
                
                // 可以根据层级执行不同的操作
                switch (level)
                {
                    case 1:
                        MessageBox.Show("这是一级节点");
                        break;
                    case 2:
                        MessageBox.Show("这是二级节点");
                        break;
                    default:
                        MessageBox.Show($"这是第{level}级节点");
                        break;
                }
            }
        }

        private (int count, List<string>) getdata()
        {
            using (var context = new ProjectContext())
            {
                int totalCount = context.PeopleTable.Count();

                List<string> allNames =context.PeopleTable
                    .Select(p => p.PeopleName)
                    .ToList();
                return (totalCount, allNames);
            }
        }


    }
}

