using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using ProjectCycleManage.View;
using ProjectManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace ProjectCycleManage.ViewModel
{
    public partial class LoginVM : ObservableObject
    {
        [ObservableProperty]
        private string _userpassword;

        [ObservableProperty]
        private string _username;

        private int _loginpersonnamegrade;
        public LoginVM()
        {
            Username = "请输入用户名";
        }

        [RelayCommand]
        private async void Login()
        {
            //MessageBox.Show(Userpassword + " +" + Username);
            using var context = new ProjectContext();
            var user = await context.PeopleTable
                .FirstOrDefaultAsync(u => u.PeopleName == Username && u.Password == Userpassword);

            if (user != null)
            {
                if (user.Permission != null)
                {
                    _loginpersonnamegrade = Convert.ToInt32(user.Permission);
                }
                else
                {
                    _loginpersonnamegrade = 0;
                }

                // 登录成功，打开主窗口
                var mainWindow = new MainWindow();
                MainVM vm = new MainVM(Username , _loginpersonnamegrade);
                
                mainWindow.DataContext = vm;
                mainWindow.Show();

                // 关闭登录窗口
                Application.Current.Windows.OfType<LogInWindow>().FirstOrDefault()?.Close();
            }
            else
            {
                MessageBox.Show("登录失败");
            }
        }
    }
}
