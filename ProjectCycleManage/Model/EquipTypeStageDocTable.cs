using Microsoft.EntityFrameworkCore;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCycleManage.Model
{
    [Comment("设备类型-阶段-文档对应表")]
    public class EquipTypeStageDocTable
    {
        [Key]
        public int Id { get; set; }


        [Comment("设备类型")]
        public int? equipmenttypeId { get; set; }
        public EquipmentType? equipmenttype { get; set; }

        [Comment("阶段-例-项目评审")]
        public int ProjectStageId { get; set; }
        public ProjectStage ProjectStage { get; set; }

        [Comment("文档-例-设备名称")]
        [ForeignKey("documenttype")]
        public int documenttypeId { get; set; }
        public DocumentType documenttype { get; set; }

        [Comment("文档状态，Nece-必要，Abolish-废除")]
        public string Status { get;set;  }
    }
}
