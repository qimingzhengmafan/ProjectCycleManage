using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Model
{
    public class TreeModel:INotifyPropertyChanged
    {
        private string? _leveveone;
        /// <summary>
        /// 一级
        /// </summary>
        public string? LevelOne
        {
            get => _leveveone;
            set
            {
                _leveveone = value;
                OnPropChanged("LevelOne");
            }
        }
        
        private int _level = 1;
        /// <summary>
        /// 层级
        /// </summary>
        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                OnPropChanged("Level");
            }
        }
        
        private ObservableCollection<TreeModel> _leveltwo = new ObservableCollection<TreeModel>();
        /// <summary>
        /// 二级
        /// </summary>
        public ObservableCollection<TreeModel> LevelTwo
        {
            get => _leveltwo;
            set
            {
                _leveltwo = value;
                OnPropChanged("LevelTwo");
            }
        }




        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropChanged(String Prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Prop));
        }
    }
}
