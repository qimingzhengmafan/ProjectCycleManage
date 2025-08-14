using MiniExcelLibs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FramewarkTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var path = @"D:\weilisync\works\HuaDaLaser\ProjectManagementSoftware\Document\项目汇总表.xlsx";
            //D:\NASFile\weilisync\Works\
            string adddata = "";
            string xlsxadddata = "";
            string mysqlcon = "server=rm-uf694p49ucaz7035xvo.mysql.rds.aliyuncs.com;database=rdxcftghuj;port=3306;user=MyPublicAccount;password=MyPublic789W";

            MySqlConnection con = new MySqlConnection(mysqlcon);

            var rows = MiniExcel.Query(path, useHeaderRow: true, startCell: "B3");
            var datas = rows.ToList();

            var inui = datas[1];
            var outui = ((IDictionary<string, object>)inui).Keys;
            int datacount = 0;
            foreach (var key in outui)
            {
                datacount++;
                //string cleanedText = key.Replace("\r", "").Replace("\n", "").Replace("/", "").Replace(" ", "");
                string cleanedText = key;
                //columns.Add(cleanedText);
                adddata = adddata + Adddatafun(cleanedText, outui.Count() - datacount);
                Console.WriteLine(cleanedText);

            }

            var adddatacopy = datas;
            adddatacopy.RemoveRange(0, 2);
            con.Open();
            //int BigFor = 0;
            foreach (var item in adddatacopy)
            {
                var xlsxinui = item;
                var xlsxoutui = ((IDictionary<string, object>)xlsxinui).Values;
                int xlsxdatacount = 0;
                foreach (var key in xlsxoutui)
                {
                    xlsxdatacount++;
                    //string cleanedText = key.Replace("\r", "").Replace("\n", "").Replace("/", "").Replace(" ", "");
                    string cleanedText;
                    if (key == null)
                    {
                        cleanedText = "";
                    }
                    else
                    {
                        cleanedText = key.ToString();
                    }

                    //columns.Add(cleanedText);
                    xlsxadddata = xlsxadddata + Adddatafun1(cleanedText, xlsxoutui.Count() - xlsxdatacount);
                    Console.WriteLine(cleanedText);

                }

                string xlsxdata = $"insert into key_value3 values ({xlsxadddata});";
                //string ylsxdata = $"'{adddata}' , '{adddata}' , '{adddata}' , '{adddata}' , '{adddata}'";

                Console.WriteLine("连接成功");
                string sql = xlsxdata;
                Console.WriteLine(sql);
                MySqlCommand cmd = new MySqlCommand(sql, con);
                int res = cmd.ExecuteNonQuery();
                xlsxadddata = "";
                //BigFor++;
            }



            //Console.WriteLine(outui);


            //string yui = $"CREATE TABLE key_value8({adddata})COMMENT = '这是一个测试comment';";
            //string xlsxdata = $"insert into key_value1 values ({xlsxadddata});";
            ////string ylsxdata = $"'{adddata}' , '{adddata}' , '{adddata}' , '{adddata}' , '{adddata}'";
            //con.Open();
            //Console.WriteLine("连接成功");
            //string sql = yui;
            //Console.WriteLine(sql);
            //MySqlCommand cmd = new MySqlCommand(sql, con);
            //int res = cmd.ExecuteNonQuery();
            //MySqlDataReader mdr = command(sql)



            //while (mdr.Read())
            //{
            //    Console.WriteLine(mdr[0].ToString());
            //    Console.WriteLine(mdr[1].ToString());
            //}



            //Console.ReadLine();
            //第六步：关闭连接
            con.Clone();




            //Console.WriteLine(yui);
            Console.ReadLine();



            string Adddatafun(string data, int lastdata)
            {
                string data1 = "";
                if (lastdata == 0)
                {
                    data1 = data1 + data + " " + "VARCHAR(50)";
                }
                else
                {
                    data1 = data1 + data + " " + "VARCHAR(50)" + " " + ",";
                }
                return data1;
            }

            string Adddatafun1(string data, int lastdata)
            {
                string data1 = "";
                if (lastdata == 0)
                {
                    data1 = data1 + $"'{data}'";
                }
                else
                {
                    data1 = data1 + $"'{data}'" + " , ";
                }
                return data1;
            }

            //MySqlCommand command(string sql)
            //{
            //    MySqlCommand cmd = new MySqlCommand(sql, con);
            //    return cmd;
            //}
        }
    }
}
