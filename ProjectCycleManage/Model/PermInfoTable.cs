using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCycleManage.Model
{
    [Comment("权限信息表")]
    public class PermInfoTable
    {
        [Key]
        public int Id { get; set; }

        [Comment("权限名称-例-跟进人")]
        public string PermName { get; set; }

        [Comment("权限等级")]
        public int Perm { get; set; }

    }
}
