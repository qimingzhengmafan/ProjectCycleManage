using ClientSide.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientSide.ViewModel
{
    public partial class LogInVM : ObservableObject
    {
        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _password;


        public LogInVM() 
        {

        }

        [RelayCommand]
        private async void Login()
        {
            using var context = new ProjectContext();
            var user = await context.PeopleTable
                .FirstOrDefaultAsync(u => u.PeopleName == Name && u.Password == Password);

            if (user != null)
            {
                // 登录成功，打开主窗口
                var mainWindow = new MainWindow();
                MainVM vm = new MainVM(Name);
                mainWindow.DataContext = vm;
                mainWindow.Show();

                // 关闭登录窗口
                Application.Current.Windows.OfType<LogInView>().FirstOrDefault()?.Close();
            }
            else
            {
                MessageBox.Show("登录失败");
            }
        }


    }
}
