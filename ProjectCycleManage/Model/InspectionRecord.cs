using Microsoft.EntityFrameworkCore;
using ProjectCycleManage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    /// <summary>
    /// 检验记录
    /// </summary>
    public class InspectionRecord
    {
        public int InspectionRecordId { get; set; }
        public int ProjectsId { get; set; }
        public Projects Projects { get; set; }
        public int? CheckPeopleId { get; set; }
        public PeopleTable? CheckPeople {  get; set; }
        public DateTime? CheckTime { get; set; }
        public string? CheckResult { get; set; }
        public string? CheckOpinion { get; set; }

        [Comment("流程ID记录")]
        public int projId {  get; set; }
        public ProjFlowTable proj { get; set; }

        [Comment("顺序")]
        public int Sequence { get; set; }

    }
}
