// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using MiniExcelLibs;

var regs = MiniExcel.Query<Person>("");

public class Person
{
    public int 姓名 { get; set; }
    public string 年纪 { get; set; }
    public string 性别 { get; set; }
}
