using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomeloEntityFrameworkCoreTest.Models
{
    /// <summary>
    /// 项目表
    /// </summary>
    public class Projects
    {
        /// <summary>
        /// 项目序号
        /// </summary>
        [Key]
        public int ProjectsId { get; set; }
        public int Year { get; set; }
        public DateTime ProcurementMonth { get; set; }
        public string ProjectName { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectIdentifyingNumber { get; set; }

        public int equipmenttypeId { get; set; }
        public  EquipmentType equipmenttype { get; set; }
        public int typeId { get; set; }
        public TypeTable type { get; set; }
        public int ProjectStageId { get; set; }
        public ProjectStage ProjectStage { get; set; }
        public DateTime FinishTime { get; set; }
        /// <summary>
        /// 预算
        /// </summary>
        public string Budget { get; set; }
        /// <summary>
        /// 实际花费
        /// </summary>
        public string ActualExpenditure { get; set; }
        /// <summary>
        /// 项目进度（%）
        /// </summary>
        public int ProjectProgress { get; set; }
        /// <summary>
        /// 项目阶段状态
        /// </summary>
        public int ProjectPhaseStatusId { get; set; }
        public ProjectPhaseStatus ProjectPhaseStatus { get; set; }
        /// <summary>
        /// 项目负责人
        /// </summary>
        public int ProjectLeaderId { get; set; }
        public PeopleTable ProjectLeader { get; set; }
        /// <summary>
        /// 项目跟进人
        /// </summary>
        public int projectfollowuppersonId { get; set; }
        public PeopleTable projectfollowupperson { get; set; }
        /// <summary>
        /// 资产编号
        /// </summary>
        public string AssetNumber { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remarks {  get; set; }



    }
}
