using CommunityToolkit.Mvvm.ComponentModel;
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
    /// ProjectCard.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectCard : UserControl
    {
        public ProjectCard()
        {
            InitializeComponent();
        }

        public string ProjectName
        {
            get { return (string)GetValue(ProjectNameProperty); }
            set { SetValue(ProjectNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProjectName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProjectNameProperty =
            DependencyProperty.Register(nameof(ProjectName), typeof(string), typeof(ProjectCard), new PropertyMetadata(null));



        public string RunningStatus
        {
            get { return (string)GetValue(RunningStatusProperty); }
            set { SetValue(RunningStatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProjectStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RunningStatusProperty =
            DependencyProperty.Register(nameof(RunningStatus), typeof(string), typeof(ProjectCard), new PropertyMetadata(null , OnRunningStatusChanged));

        private static void OnRunningStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var projectctrl = (ProjectCard)d;
            string newdata = (string)e.NewValue;

            switch (newdata)
            {
                case "进行中":
                    projectctrl.BackColor = "#E8F5E9";
                    projectctrl.FontColor = "#10B981";
                    projectctrl.SuspendBtnVb = Visibility.Visible;
                    projectctrl.RestartBtnVb = Visibility.Collapsed;
                    projectctrl.SuspendBtnEn = true;
                    break;

                case "暂停":
                    projectctrl.BackColor = "#FFDBDB";
                    projectctrl.FontColor = "#FA0202";
                    projectctrl.SuspendBtnEn = true;
                    projectctrl.SuspendBtnVb = Visibility.Collapsed;
                    projectctrl.RestartBtnVb = Visibility.Visible;
                    
                    break;

                case "已完成":
                    projectctrl.BackColor = "#E3F2FD";
                    projectctrl.FontColor = "#2196F3";
                    projectctrl.SuspendBtnVb = Visibility.Visible;
                    projectctrl.RestartBtnVb = Visibility.Collapsed;
                    projectctrl.SuspendBtnEn = false;
                    break;
                case "审核中":
                    projectctrl.BackColor = "#FFFBEB";
                    projectctrl.FontColor = "#FAC42F";
                    projectctrl.SuspendBtnEn = true;
                    projectctrl.SuspendBtnVb = Visibility.Visible;
                    projectctrl.RestartBtnVb = Visibility.Collapsed;
                    break;
            }
        }

        public string BackColor
        {
            get { return (string)GetValue(BackColorProperty); }
            set { SetValue(BackColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackColorProperty =
            DependencyProperty.Register(nameof(BackColor), typeof(string), typeof(ProjectCard), new PropertyMetadata(null));

        public string FontColor
        {
            get { return (string)GetValue(FontColorProperty); }
            set { SetValue(FontColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FontColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FontColorProperty =
            DependencyProperty.Register(nameof(FontColor), typeof(string), typeof(ProjectCard), new PropertyMetadata(null));

        public string ProjectLeader
        {
            get { return (string)GetValue(ProjectLeaderProperty); }
            set { SetValue(ProjectLeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProjectLeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProjectLeaderProperty =
            DependencyProperty.Register(nameof(ProjectLeader), typeof(string), typeof(ProjectCard), new PropertyMetadata(null));

        public string StartTime
        {
            get { return (string)GetValue(StartTimeProperty); }
            set { SetValue(StartTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartTimeProperty =
            DependencyProperty.Register(nameof(StartTime), typeof(string), typeof(ProjectCard), new PropertyMetadata(null));



        public string ProjectStatus
        {
            get { return (string)GetValue(ProjectStatusProperty); }
            set { SetValue(ProjectStatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProjectStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProjectStatusProperty =
            DependencyProperty.Register(nameof(ProjectStatus), typeof(string), typeof(ProjectCard), new PropertyMetadata(null));




        public string ProjectStatusProgress
        {
            get { return (string)GetValue(ProjectStatusProgressProperty); }
            set { SetValue(ProjectStatusProgressProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProjectStatusProgress.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProjectStatusProgressProperty =
            DependencyProperty.Register(nameof(ProjectStatusProgress), typeof(string), typeof(ProjectCard), new PropertyMetadata(null));


        public double ProjectStatusProgressdouble
        {
            get { return (double)GetValue(ProjectStatusProgressdoubleProperty); }
            set { SetValue(ProjectStatusProgressdoubleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProjectStatusProgressdouble.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProjectStatusProgressdoubleProperty =
            DependencyProperty.Register(nameof(ProjectStatusProgressdouble), typeof(double), typeof(ProjectCard), new PropertyMetadata(0 , OndoubleChanged));



        private static void OndoubleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var projectctrldouble = (ProjectCard)d;
            double newdata = (double)e.NewValue;

            projectctrldouble.ProjectStatusProgress = newdata.ToString();
        }


        /// <summary>
        /// 查看详情
        /// </summary>
        public ICommand ViewDetails
        {
            get { return (ICommand)GetValue(ViewDetailsProperty); }
            set { SetValue(ViewDetailsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewDetails.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewDetailsProperty =
            DependencyProperty.Register(nameof(ViewDetails), typeof(ICommand), typeof(ProjectCard), new PropertyMetadata(null));


        /// <summary>
        /// 暂停项目
        /// </summary>
        public ICommand SuspendProject
        {
            get { return (ICommand)GetValue(SuspendProjectProperty); }
            set { SetValue(SuspendProjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SuspendProject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SuspendProjectProperty =
            DependencyProperty.Register(nameof(SuspendProject), typeof(ICommand), typeof(ProjectCard), new PropertyMetadata(null , SuspendProjectChanged));
        private static void SuspendProjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var projectctrl = (ProjectCard)d;
            projectctrl.BackColor = "#FFDBDB";
            projectctrl.FontColor = "#FA0202";
            projectctrl.SuspendBtnEn = true;
            projectctrl.SuspendBtnVb = Visibility.Collapsed;
            projectctrl.RestartBtnVb = Visibility.Visible;
        }

        /// <summary>
        /// 项目重启
        /// </summary>
        public ICommand RestartProject
        {
            get { return (ICommand)GetValue(RestartProjectProperty); }
            set { SetValue(RestartProjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RestartProject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RestartProjectProperty =
            DependencyProperty.Register(nameof(RestartProject), typeof(ICommand), typeof(ProjectCard), new PropertyMetadata(null , RestartProjectChanged));

        private static void RestartProjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var projectctrl = (ProjectCard)d;
            projectctrl.BackColor = "#FFDBDB";
            projectctrl.FontColor = "#FA0202";
            projectctrl.SuspendBtnEn = true;
            projectctrl.SuspendBtnVb = Visibility.Visible;
            projectctrl.RestartBtnVb = Visibility.Collapsed;
        }

        /// <summary>
        /// 暂停项目
        /// </summary>
        public Visibility SuspendBtnVb
        {
            get { return (Visibility)GetValue(SuspendBtnVbProperty); }
            set { SetValue(SuspendBtnVbProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SuspendBtnVb.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SuspendBtnVbProperty =
            DependencyProperty.Register(nameof(SuspendBtnVb), typeof(Visibility), typeof(ProjectCard), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// 项目重启
        /// </summary>
        public Visibility RestartBtnVb
        {
            get { return (Visibility)GetValue(RestartBtnVbProperty); }
            set { SetValue(RestartBtnVbProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RestartBtnVb.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RestartBtnVbProperty =
            DependencyProperty.Register(nameof(RestartBtnVb), typeof(Visibility), typeof(ProjectCard), new PropertyMetadata(Visibility.Collapsed));


        public bool SuspendBtnEn
        {
            get { return (bool)GetValue(SuspendBtnEnProperty); }
            set { SetValue(SuspendBtnEnProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SuspendBtnEn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SuspendBtnEnProperty =
            DependencyProperty.Register(nameof(SuspendBtnEn), typeof(bool), typeof(ProjectCard), new PropertyMetadata(true));




    }
}
