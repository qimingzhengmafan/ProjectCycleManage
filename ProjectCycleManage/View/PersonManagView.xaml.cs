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
    /// PersonManagView.xaml 的交互逻辑
    /// </summary>
    public partial class PersonManagView : UserControl
    {
        public PersonManagView()
        {
            InitializeComponent();
        }
    }

    public class TeamMemberData
    {
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string EmployeeId { get; set; }
        public string Department { get; set; }
        public string Role { get; set; }
        public Brush RoleBackground { get; set; }
        public Brush RoleForeground { get; set; }
        public string JoinDate { get; set; }
        public string Status { get; set; }
        public Brush StatusColor { get; set; }
    }
}
