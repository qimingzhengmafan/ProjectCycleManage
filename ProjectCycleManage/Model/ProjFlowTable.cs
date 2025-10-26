using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCycleManage.Model
{
    [Comment("项目流程表")]
    public class ProjFlowTable
    {
        [Key]
        public int Id { get; set; }


        [Comment("流程信息-例-项目评审")]
        public string ProjFlowInfor { get; set; }
    }
}
