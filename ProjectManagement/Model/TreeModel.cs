using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Model
{
    public class TreeModel
    {
        /// <summary>
        /// 一级
        /// </summary>
        public string? LevelOne { get; set; }
        /// <summary>
        /// 二级
        /// </summary>
        public ObservableCollection<TreeModel> LevelTwo { get; set; } = new ObservableCollection<TreeModel>();
    }
}
