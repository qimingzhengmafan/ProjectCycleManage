using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    /// <summary>
    /// 项目阶段状态表
    /// </summary>
    /// 
    [Comment("项目阶段状态表，例-进行中/已完结")]
    public class ProjectPhaseStatus
    {
        public int ProjectPhaseStatusId { get; set; }
        public string ProjectPhaseStatusName { get; set; }
    }
}
