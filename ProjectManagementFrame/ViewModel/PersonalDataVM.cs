using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementFrame.ViewModel
{
    public class PersonalDataVM
    {
        public AllProjects _personalprojectsinformation = new AllProjects();
        public AllProjects PersonalProjectsInformation
        {
            get => _personalprojectsinformation;
            set => _personalprojectsinformation = value;
        }



        public PersonalDataVM()
        {

        }



    }
}
