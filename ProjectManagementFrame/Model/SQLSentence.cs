using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementFrame.Model
{
    public class SQLSentence
    {
        public string Search2022 { get; set; } = "SELECT COUNT(*) FROM `key_value3` WHERE 年份 = 2022";
        public string Search2023 { get; set; } = "SELECT COUNT(*) FROM `key_value3` WHERE 年份 = 2023";
        public string Search2024 { get; set; } = "SELECT COUNT(*) FROM `key_value3` WHERE 年份 = 2024";
        public string Search2025 { get; set; } = "SELECT COUNT(*) FROM `key_value3` WHERE 年份 = 2025";

        /// <summary>
        /// 项目需求
        /// </summary>
        public string ProjectStage_ProjectRequirements { get; set; } =
            "SELECT COUNT(*) FROM `key_value3` WHERE 年份 = 2025 AND 项目阶段 = '项目需求'";

        /// <summary>
        /// 立项评审
        /// </summary>
        public string ProjectStage_ProjectInitiationReview { get; set; } =
            "SELECT COUNT(*) FROM `key_value3` WHERE 年份 = 2025 AND 项目阶段 = '立项评审'";

        /// <summary>
        /// 方案评审
        /// </summary>
        public string ProjectStage_SchemeReview { get; set; } =
            "SELECT COUNT(*) FROM `key_value3` WHERE 年份 = 2025 AND 项目阶段 = '方案评审'";

        /// <summary>
        /// 设备采购
        /// </summary>
        public string ProjectStage_EquipmentProcurement { get; set; } =
            "SELECT COUNT(*) FROM `key_value3` WHERE 年份 = 2025 AND 项目阶段 = '设备采购'";

        /// <summary>
        /// 预验收/组装调试
        /// </summary>
        public string ProjectStage_PreAcceptanceassemblyAndCommissioning { get; set; } =
            "SELECT COUNT(*) FROM `key_value3` WHERE 年份 = 2025 AND 项目阶段 = '预验收/组装调试'";

        /// <summary>
        /// 设备验收
        /// </summary>
        public string ProjectStage_EquipmentAcceptance { get; set; } =
            "SELECT COUNT(*) FROM `key_value3` WHERE 年份 = 2025 AND 项目阶段 = '设备验收'";

        /// <summary>
        /// 完成
        /// </summary>
        public string ProjectStage_Completed { get; set; } =
            "SELECT COUNT(*) FROM `key_value3` WHERE 年份 = 2025 AND 项目阶段 = '完成'";

        /// <summary>
        /// 朱成绪——2025项目数
        /// </summary>
        public string zhuchengxu_2025 { get; set; } =
            "SELECT COUNT(*) FROM `key_value3` WHERE 年份 = 2025 AND 项目负责人 = '朱成绪'";
    }
}
