using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using FolderExplorer;

namespace FolderExplorer
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<FolderItem> _rootFolders = new();

        public MainWindow()
        {
            InitializeComponent();
            
            // 初始化示例数据
            var root = new FolderItem("Root") { Type = NodeType.CustomRoot };
            root.Children.Add(new FolderItem("Category 1") { Type = NodeType.Category });
            root.Children.Add(new FolderItem("Item 1") { Type = NodeType.Item });
            _rootFolders.Add(root);

            DataContext = this;
        }

        public ObservableCollection<FolderItem> RootFolders => _rootFolders;
    }

    public class IconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                NodeType.CustomRoot => "\uE8B7",
                NodeType.Category => "\uE8F1",
                NodeType.Item => "\uE8A5",
                _ => "\uE8B7"
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}