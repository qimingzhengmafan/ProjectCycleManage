﻿using ProjectCycleManage.Model;
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

namespace ProjectCycleManage.View
{
    /// <summary>
    /// AllProjectsView.xaml 的交互逻辑
    /// </summary>
    public partial class AllProjectsView : UserControl
    {
        public AllProjectsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// DataGrid双击事件 - 打开项目详情弹窗
        /// </summary>
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is ProjectListDisplayModel selectedProject)
            {
                // TODO: 打开项目详情弹窗
                MessageBox.Show(
                    $"项目名称: {selectedProject.Projectname}\n" +
                    $"项目编号: {selectedProject.Projectidentifyingnumber}\n" +
                    $"负责人: {selectedProject.ManagerName}\n" +
                    $"进度: {selectedProject.Progress}%\n" +
                    $"健康状况: {selectedProject.HealthStatus}",
                    "项目详情",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }
    }
}
