using MySql.Data.MySqlClient;
using Page_Navigation_App.Utilities;
using System;
using System.Collections.Generic;
using System.Windows;
using ProjectManagementSoftwareFrame.Model;

namespace ProjectManagementFrame.Model
{
    public class MySQLOperationClass
    {
        private string _mysqladdress = "server=rm-uf694p49ucaz7035xvo.mysql.rds.aliyuncs.com;" +
                                       "database=rdxcftghuj;" +
                                       "port=3306;" +
                                       "user=MyPublicAccount;" +
                                       "password=MyPublic789W";
        public string Mysqladdress 
        {
            get => _mysqladdress;
            set
            {
                _mysqladdress = value;
                //OnPropertyChanged();
            }
        }

        private MySqlConnection con;

        public MySQLOperationClass()
        {
            con = new MySqlConnection(Mysqladdress);
        }


        public void Connect()
        {
            try
            {
                con.Open();
                MessageBox.Show("连接成功");
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                //throw;
                
                MessageBox.Show(e.Message + "\n 数据库连接失败");
            }
        }

        public void Disconnect()
        {
            try
            {
                con.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw;
            }
        }

        public List<ProjectInfor> Search(string sqldata)
        {
            List<ProjectInfor> projectClass = new List<ProjectInfor>();
            //ProjectClass projectClass = new ProjectClass();
            //string data = searchdata;
            //string sql = $"select * from key_value3 where 年份 like '%2022%';";
            string sql = sqldata;
            MySqlDataReader reader = null;
            MySqlCommand cmd = new MySqlCommand(sql, con);
            try
            {
                //int res = cmd.ExecuteNonQuery();
                reader = cmd.ExecuteReader();
                var readtest = reader;
                while (reader.Read())
                {
                    projectClass.Add(new ProjectInfor() { ProjectSorted = reader[0].ToString(), 
                                                          ProjectYear = reader[1].ToString(), 
                                                          ProjectName = reader[2].ToString() });
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
            return projectClass;
        }

        public int Search(string sqldata , int x)
        {
            string sql = sqldata;
            int count = 0;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    // 执行查询并获取结果
                    // ExecuteScalar() 返回第一行第一列的值
                    object result = cmd.ExecuteScalar();

                    // 将结果转换为整数
                    if (result != null && result != DBNull.Value)
                    {
                        count = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error");
            }

            return count;
        }

        public void UPData()
        {

        }

        public void Delete()
        {

        }
        
        
    }
}