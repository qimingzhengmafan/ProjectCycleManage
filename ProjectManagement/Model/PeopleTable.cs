using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    /// <summary>
    /// 人员表
    /// </summary>
    public class PeopleTable
    {
        [Key]
        public int PeopleId { get; set; }
        public string PeopleName { get; set; }
    }
}
