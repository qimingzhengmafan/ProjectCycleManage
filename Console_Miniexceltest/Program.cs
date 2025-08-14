// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using MiniExcelLibs;
using Newtonsoft.Json;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

//var regs = MiniExcel.Query<Person>("项目汇总表.xlsx");
//Console.WriteLine(JsonConvert.SerializeObject(regs, Newtonsoft.Json.Formatting.Indented));

var path = @"D:\weilisync\works\HuaDaLaser\ProjectManagementSoftware\Document\项目汇总表.xlsx";
//var rows = MiniExcel.Query(path).ToList();

var rows = MiniExcel.Query(path, useHeaderRow: true, startCell: "B3");
//if (firrows != null)
//{
//    var columnNames = ((IDictionary<string, object>)firrows).Keys;
//    Console.WriteLine("列名列表: " + string.Join(", ", columnNames));
//}

string yui = "CREATE TABLE key_value2(_key VARCHAR(255)," +
    " _value VARCHAR(255)" +
    ")COMMENT = '这是一个测试comment';";
try
{
    var datas = rows.ToList();
    var inui = datas[1];
    var outui = ((IDictionary<string, object>)inui).Keys;
    datas.RemoveRange(0 , 2);
    
    var names = datas.Select(row => row.项目名称).ToList();
    foreach (var name in names)
    {
        if (name != null)
        {
            Console.WriteLine(name);
        }
        
    }
    Console.WriteLine(names[5]);
}
catch (Exception)
{

	//throw;
}

foreach (var item in rows)
{
	try
	{
        Console.WriteLine($"项目名称: {item.项目名称}, 项目编号: {item.项目编号}");
    }
	catch (Exception)
	{

		//throw;
	}
    //Console.WriteLine(item);
    
}

//var rows = MiniExcel.Query(path, useHeaderRow: true).ToList();


//Console.WriteLine(rows.Count);
//Console.WriteLine(rows[1]);
Console.ReadLine();


public class Person
{
    public string 姓名 { get; set; }
    public string 年纪 { get; set; }
    public string 性别 { get; set; }
}



