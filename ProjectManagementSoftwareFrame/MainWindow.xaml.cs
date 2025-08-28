using MySql.Data.MySqlClient;
using ProjectManagementSoftwareFrame.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static ProjectManagementSoftwareFrame.MainWindow;

namespace ProjectManagementSoftwareFrame
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        string mysqlcon = "server=rm-uf694p49ucaz7035xvo.mysql.rds.aliyuncs.com;" +
                          "database=rdxcftghuj;port=3306;" +
                          "user=MyPublicAccount;" +
                          "password=MyPublic789W";

        MySqlConnection con;
        private ObservableCollection<ProjectInfor> _projectinfor = new ObservableCollection<ProjectInfor>();
        public ObservableCollection<ProjectInfor> Projectdata
        {
            get { return _projectinfor; }
            set { _projectinfor = value; }
        }

        private ProjectInfor _test = new ProjectInfor();
        public ProjectInfor Test
        {
            get { return _test; }
            set { _test = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
            con = new MySqlConnection(mysqlcon);

        }



        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            

        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                con.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("连接失败");
                //throw;
            }
            
        }

        private void DisConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                con.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("断开故障");
                //throw;
            }
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            string xlsxdata = $"SELECT * from key_value3;";
            Projectdata.Clear();
            //string ylsxdata = $"'{adddata}' , '{adddata}' , '{adddata}' , '{adddata}' , '{adddata}'";
            string sql = xlsxdata;
            MySqlDataReader reader = null;
            MySqlCommand cmd = new MySqlCommand(sql, con);
            try
            {
                //int res = cmd.ExecuteNonQuery();
                reader = cmd.ExecuteReader();
                var readtest = reader;
                List<ProjectInfor> readlist = new List<ProjectInfor>();
                while (reader.Read())
                {
                    Projectdata.Add(new ProjectInfor() { ProjectSorted = reader[0].ToString(), 
                                                         ProjectYear = reader[1].ToString(), 
                                                         ProjectName = reader[3].ToString() });
                }


            }
            catch (Exception)
            {
                MessageBox.Show("Error");
                //throw;
            }
            finally
            {
                reader.Close();
            }

        }

        private void test(object sender, RoutedEventArgs e)
        {
            Projectdata.Add(new ProjectInfor() { ProjectSorted = "1", ProjectYear = "2", ProjectName = "3" });
            Application.Current.Dispatcher.Invoke(() =>
            {
                dataGrid.ItemsSource = Projectdata;
            });
            

        }

        private void SelectDong(object sender, RoutedEventArgs e)
        {
            string xlsxdata = $"select 序号,年份,项目名称 from key_value3 where 项目负责人 like '董鑫';";
            Projectdata.Clear();
            //string ylsxdata = $"'{adddata}' , '{adddata}' , '{adddata}' , '{adddata}' , '{adddata}'";
            string sql = xlsxdata;
            MySqlDataReader reader = null;
            MySqlCommand cmd = new MySqlCommand(sql, con);
            try
            {
                //int res = cmd.ExecuteNonQuery();
                reader = cmd.ExecuteReader();
                var readtest = reader;
                List<ProjectInfor> readlist = new List<ProjectInfor>();
                while (reader.Read())
                {
                    Projectdata.Add(new ProjectInfor() { ProjectSorted = reader[0].ToString(), ProjectYear = reader[1].ToString(), ProjectName = reader[2].ToString() });
                }


            }
            catch (Exception)
            {
                MessageBox.Show("Error");
                //throw;
            }
            finally
            {
                reader.Close();
            }
        }

        private void Select2022(object sender, RoutedEventArgs e)
        {
            string xlsxdata = $"select 序号,年份,项目名称 from key_value3 where 年份 like '%2022%';";
            Projectdata.Clear();
            //string ylsxdata = $"'{adddata}' , '{adddata}' , '{adddata}' , '{adddata}' , '{adddata}'";
            string sql = xlsxdata;
            MySqlDataReader reader = null;
            MySqlCommand cmd = new MySqlCommand(sql, con);
            try
            {
                //int res = cmd.ExecuteNonQuery();
                reader = cmd.ExecuteReader();
                var readtest = reader;
                List<ProjectInfor> readlist = new List<ProjectInfor>();
                while (reader.Read())
                {

                    Projectdata.Add(new ProjectInfor() { ProjectSorted = reader[0].ToString(), ProjectYear = reader[1].ToString(), ProjectName = reader[2].ToString() });
                }


            }
            catch (Exception)
            {
                MessageBox.Show("Error");
                //throw;
            }
            finally
            {
                reader.Close();
            }
        }
    }
}
