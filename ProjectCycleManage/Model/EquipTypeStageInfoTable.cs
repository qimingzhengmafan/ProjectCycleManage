using Microsoft.EntityFrameworkCore;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCycleManage.Model
{
    [Comment("设备类型-阶段-信息对应表")]
    public class EquipTypeStageInfoTable
    {
        [Key]
        public int Id { get; set; }


        [Comment("设备类型")]
        public int? equipmenttypeId { get; set; }
        public EquipmentType? equipmenttype { get; set; }

        [Comment("阶段-例-项目评审")]
        public int ProjectStageId { get; set; }
        public ProjectStage ProjectStage { get; set; }

        [Comment("信息-例-设备名称")]
        public int InformationId {  get; set; }
        public InformationTable Information { get; set; }


    }
}
