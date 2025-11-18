using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    /// <summary>
    /// 项目阶段表
    /// </summary>
    /// [Comment("项目阶段表，例-项目评审/设备采购")]
    /// 
    [Comment("项目阶段表，例-项目评审/设备采购")]
    public class ProjectStage
    {
        public int ProjectStageId { get; set; }
        public string ProjectStageName { get; set; }
        [Comment("每个阶段所对应的进度")]
        public int ProjectProgress { get; set; }
    }
}
