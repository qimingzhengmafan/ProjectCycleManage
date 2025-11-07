using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    /// <summary>
    /// 类型表
    /// </summary>
    /// 
    [Comment("项目类型表，例：复制/新增/改善")]
    public class TypeTable
    {
        [Key]
        public int TypeId { get; set; }
        public string TypeName { get; set; }
    }
}
