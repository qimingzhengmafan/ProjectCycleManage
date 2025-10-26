using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCycleManage.Model
{
    [Comment("信息表")]
    public class InformationTable
    {
        [Key]
        public int Id {  get; set; }

        public string Infor { get; set;  }

        [Comment("权限分级")]
        public string? Permission { get; set; }
    }
}
