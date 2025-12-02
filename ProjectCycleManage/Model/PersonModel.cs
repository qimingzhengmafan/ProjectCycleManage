using CommunityToolkit.Mvvm.ComponentModel;
using ProjectCycleManage.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCycleManage.Model
{
    public partial class PersonModel : ObservableObject
    {
        [ObservableProperty]
        private int peopleId;

        [ObservableProperty]
        private string _surname;

        [ObservableProperty]
        private string peopleName = string.Empty;

        [ObservableProperty]
        private string status = "在职";

        [ObservableProperty]
        private string statusColor = "#4CAF50";

        public PersonModel() { }

        public PersonModel(int peopleId, string peopleName,string status)
        {
            PeopleId = peopleId;
            PeopleName = peopleName;
            Status = status == "True" ? "在职" : "离职" ;
            StatusColor = status == "True" ? "#4CAF50" : "#F44336";
            Surname = StringHelper.GetFirstCharOrN(peopleName);
        }
    }
}