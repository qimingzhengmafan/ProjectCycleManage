using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSoftwareFrame.Model
{
    public class statustest
    {
    }

    public class Employee : INotifyPropertyChanged
    {
        private string _testdata;
        public string TestData
        {
            get => _testdata;
            set
            {
                _testdata = value;
                OnPropChanged("TestData");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropChanged(String Prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Prop));
        }
    }
}
