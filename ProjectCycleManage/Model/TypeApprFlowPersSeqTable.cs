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
    
    [Comment("类型审批流程人员顺序表")]
    public class TypeApprFlowPersSeqTable
    {
        [Key]
        public int Id { get; set; }

        [Comment("设备类型")]
        public int? equipmenttypeId { get; set; }
        public EquipmentType? equipmenttype { get; set; }

        [Comment("流程信息")]
        public int? projectflowId { get; set; }
        public ProjFlowTable projectflow {  get; set; }

        [Comment("审核人员")]
        public int? ReviewerPeopleId { get; set; }
        public PeopleTable? Reviewer { get; set; }

        [Comment("顺序")]
        public int? Sequence { get; set; }

        [Comment("标记-Dele表示删除")]
        public string? Mark { get; set; }
    }
}
