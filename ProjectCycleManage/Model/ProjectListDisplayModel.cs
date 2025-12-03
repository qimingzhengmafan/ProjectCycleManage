using System;

namespace ProjectCycleManage.Model
{
    /// <summary>
    /// 项目列表显示模型
    /// </summary>
    public class ProjectListDisplayModel
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public int ProjectsId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Projectname { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string Projectidentifyingnumber { get; set; }

        /// <summary>
        /// 负责人姓名
        /// </summary>
        public string ManagerName { get; set; }

        /// <summary>
        /// 项目年份
        /// </summary>
        public int? Projectyear { get; set; }

        /// <summary>
        /// 年度预算（万元）
        /// </summary>
        public decimal? Annualbudget { get; set; }

        /// <summary>
        /// 实际支出（万元）
        /// </summary>
        public decimal? ActualExpense { get; set; }

        /// <summary>
        /// 项目进度（%）
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// 健康状况
        /// </summary>
        public string HealthStatus { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public string ProjectType { get; set; }

        /// <summary>
        /// 使用部门
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
    }
}
