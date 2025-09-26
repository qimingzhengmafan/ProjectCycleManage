using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FolderExplorer
{
    public enum NodeType { CustomRoot, Category, Item }

    public class FolderItem : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private string _fullPath;
        private bool _isExpanded;
        private ObservableCollection<FolderItem> _children = new();
        private bool _hasChildren;
        private bool _areChildrenLoaded;
        private NodeType _type;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public NodeType Type
        {
            get => _type;
            set { _type = value; OnPropertyChanged(); }
        }

        public string FullPath
        {
            get => _fullPath;
            set { _fullPath = value; OnPropertyChanged(); }
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                OnPropertyChanged();

                if (value && !_areChildrenLoaded && HasChildren)
                {
                    LoadChildren();
                    _areChildrenLoaded = true;
                }
            }
        }

        public ObservableCollection<FolderItem> Children
        {
            get => _children;
            set
            {
                _children = value;
                OnPropertyChanged();
                UpdateHasChildren();
            }
        }

        public bool HasChildren
        {
            get => _hasChildren;
            set
            {
                _hasChildren = value;
                OnPropertyChanged();
            }
        }

        public FolderItem(string path)
        {
            FullPath = path;
            Name = Path.GetFileName(path);
            if (string.IsNullOrEmpty(Name))
                Name = path;

            CheckIfHasChildren();
        }

        private void CheckIfHasChildren()
        {
            try
            {
                HasChildren = Directory.EnumerateDirectories(FullPath).Any();
            }
            catch { HasChildren = false; }
        }

        private void UpdateHasChildren()
        {
            HasChildren = _children.Count > 0;
        }

        public void LoadChildren()
        {
            try
            {
                foreach (var dirPath in Directory.GetDirectories(FullPath))
                {
                    Children.Add(new FolderItem(dirPath));
                }
                UpdateHasChildren();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载失败: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}