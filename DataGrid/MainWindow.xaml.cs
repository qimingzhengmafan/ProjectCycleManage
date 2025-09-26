using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DataGrid
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<DynamicData> DataItems { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            // 初始化示例数据
            DataItems = new ObservableCollection<DynamicData>();
            for (int i = 1; i <= 5; i++)
            {
                DataItems.Add(new DynamicData { RowNumber = i });
            }

            // 添加示例列
            AddColumn("项目名称", "ProjectName");
            AddColumn("进度", "Progress");
        }

        public void AddColumn(string header, string bindingPath)
        {
            var newColumn = new System.Windows.Controls.DataGridTextColumn
            {
                Header = header,
                Binding = new System.Windows.Data.Binding(bindingPath)
            };
            SmartDataGrid.Columns.Add(newColumn);
        }

        private void DataGrid_CellEditEnding(object sender,
            System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == System.Windows.Controls.DataGridEditAction.Commit)
            {
                var editedCell = e.Column.GetCellContent(e.Row);
                var bindingPath = ((System.Windows.Data.Binding)((System.Windows.Controls.DataGridTextColumn)e.Column).Binding).Path.Path;
                var newValue = ((System.Windows.Controls.TextBox)editedCell).Text;

                // 触发数据更新事件
                System.Windows.MessageBox.Show($"已更新 {bindingPath}: {newValue}", "数据变更",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        public class DynamicData : INotifyPropertyChanged
        {
            private string _rowName = "Row ";
            private int _rowNumber;
            private string _projectName = "未命名项目";
            private double _progress = 0.0;

            public string RowName
            {
                get => _rowName + _rowNumber;
                set { _rowName = value; OnPropertyChanged(nameof(RowName)); }
            }

            public int RowNumber
            {
                get => _rowNumber;
                set { _rowNumber = value; OnPropertyChanged(nameof(RowNumber)); }
            }

            public string ProjectName
            {
                get => _projectName;
                set { _projectName = value; OnPropertyChanged(nameof(ProjectName)); }
            }

            public double Progress
            {
                get => _progress;
                set { _progress = value; OnPropertyChanged(nameof(Progress)); }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        // 添加行名称修改方法
        public void UpdateRowName(int rowIndex, string newName)
        {
            if (rowIndex >= 0 && rowIndex < DataItems.Count)
            {
                DataItems[rowIndex].RowName = newName;
            }
        }
        
        // 修改行号列绑定
        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var dataItem = e.Row.Item as DynamicData;
            if (dataItem != null)
            {
                // 显示可编辑的行名称
                e.Row.Header = dataItem.RowName;
                
                // 保持自动编号逻辑
                dataItem.RowNumber = e.Row.GetIndex() + 1;
            }
        }
    }
}
