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

        [Comment("备注")]
        public string? Reamrks { get; set; }

        [Comment("信息类型-例-信息-时间")]
        public int? InforTypesDataId { get; set; }
        public FileTypesTable? InforTypesData { get; set; }
    }
}
