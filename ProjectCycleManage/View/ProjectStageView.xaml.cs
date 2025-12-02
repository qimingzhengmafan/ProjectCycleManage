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
using ProjectCycleManage.ViewModel;
using ProjectCycleManage.Model;
using ProjectManagement.Models;

namespace ProjectCycleManage.View
{
    /// <summary>
    /// ProjectStageView.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectStageView : UserControl
    {
        public ProjectStageView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 可用文档列表双击事件 - 添加到阶段
        /// </summary>
        private void AvailableDocumentsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is DocumentType document)
            {
                if (DataContext is ProjectStageVM viewModel)
                {
                    viewModel.AddDocumentToStageCommand.Execute(document);
                }
            }
        }

        /// <summary>
        /// 可用信息列表双击事件 - 添加到阶段
        /// </summary>
        private void AvailableInformationList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is InformationTable information)
            {
                if (DataContext is ProjectStageVM viewModel)
                {
                    viewModel.AddInformationToStageCommand.Execute(information);
                }
            }
        }

        /// <summary>
        /// 阶段文档列表双击事件 - 从阶段移除
        /// </summary>
        private void StageDocumentsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is DocumentType document)
            {
                if (DataContext is ProjectStageVM viewModel)
                {
                    viewModel.RemoveDocumentFromStageCommand.Execute(document);
                }
            }
        }

        /// <summary>
        /// 阶段信息列表双击事件 - 从阶段移除
        /// </summary>
        private void StageInformationList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is InformationTable information)
            {
                if (DataContext is ProjectStageVM viewModel)
                {
                    viewModel.RemoveInformationFromStageCommand.Execute(information);
                }
            }
        }
    }
}
