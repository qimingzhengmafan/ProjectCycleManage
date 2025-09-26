using PomeloEntityFrameworkCoreTest.Data;
using PomeloEntityFrameworkCoreTest.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PomeloEntityFrameworkCoreTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //using var db = new MyBlogContext();
        //MyBlogContext db = new MyBlogContext();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine("插入一条新的博客记录");
            //db.Add(new Blogs { Url = "http://blogs.packtpub.com/dotnet" });
            //db.SaveChanges(); // 必须调用 SaveChanges 才会真正执行数据库操作
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine("查询所有博客：");
            //var blogs = db.Blogs.OrderBy(b => b.BlogsId);
            //foreach (var blog in blogs)
            //{
            //    Console.WriteLine($"博客ID: {blog.BlogsId}, 网址: {blog.Url}");
            //    MessageBox.Show($"博客ID: {blog.BlogsId}, 网址: {blog.Url}");
            //}
        }
    }
}