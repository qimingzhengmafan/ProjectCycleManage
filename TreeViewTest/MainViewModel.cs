using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeViewTest
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TreeNode> TreeData { get; set; }
        private TreeNode _selectedNode;

        public TreeNode SelectedNode
        {
            get => _selectedNode;
            set { _selectedNode = value; OnPropertyChanged(nameof(SelectedNode)); }
        }

        public MainViewModel()
        {
            TreeData = new ObservableCollection<TreeNode>();
            LoadSampleData();
        }

        private void LoadSampleData()
        {
            // 创建示例数据
            var root1 = new TreeNode("部门组织") { IsExpanded = true };

            var hr = new TreeNode("人力资源部");
            hr.Children.Add(new TreeNode("招聘组"));
            hr.Children.Add(new TreeNode("培训组"));

            var dev = new TreeNode("技术开发部") { IsExpanded = true };
            dev.Children.Add(new TreeNode("前端团队"));
            dev.Children.Add(new TreeNode("后端团队"));
            dev.Children.Add(new TreeNode("测试团队"));

            root1.Children.Add(hr);
            root1.Children.Add(dev);

            var root2 = new TreeNode("项目分类") { IsExpanded = true };
            root2.Children.Add(new TreeNode("进行中项目"));
            root2.Children.Add(new TreeNode("已完成项目"));

            TreeData.Add(root1);
            TreeData.Add(root2);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
