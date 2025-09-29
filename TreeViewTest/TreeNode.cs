using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeViewTest
{
    public class TreeNode : INotifyPropertyChanged
    {
        private string _name;
        private ObservableCollection<TreeNode> _children;
        private bool _isExpanded;
        private bool _isSelected;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public ObservableCollection<TreeNode> Children
        {
            get => _children;
            set { _children = value; OnPropertyChanged(nameof(Children)); }
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set { _isExpanded = value; OnPropertyChanged(nameof(IsExpanded)); }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }

        public TreeNode(string name)
        {
            Name = name;
            Children = new ObservableCollection<TreeNode>();
        }

        // 新增图标属性
        private string _icon;
        private string _description;
        private TreeNodeType _nodeType;

        public string Icon
        {
            get => _icon;
            set { _icon = value; OnPropertyChanged(nameof(Icon)); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        public TreeNodeType NodeType
        {
            get => _nodeType;
            set { _nodeType = value; OnPropertyChanged(nameof(NodeType)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum TreeNodeType
    {
        Folder,
        Document,
        Image,
        CodeFile
    }
}
