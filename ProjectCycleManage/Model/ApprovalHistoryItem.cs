using System;

namespace ProjectCycleManage.Model
{
    /// <summary>
    /// 审批历史项数据模型
    /// </summary>
    public class ApprovalHistoryItem
    {
        /// <summary>
        /// 审批人姓名
        /// </summary>
        public string ApproverName { get; set; }

        /// <summary>
        /// 审批人职位
        /// </summary>
        public string ApproverPosition { get; set; }

        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime? ApprovalTime { get; set; }

        /// <summary>
        /// 审批状态：已批准、待审批、已驳回
        /// </summary>
        public string ApprovalStatus { get; set; }

        /// <summary>
        /// 审批意见/评论
        /// </summary>
        public string ApprovalComment { get; set; }

        /// <summary>
        /// 审批顺序
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 是否当前审批人
        /// </summary>
        public bool IsCurrentApprover { get; set; }

        /// <summary>
        /// 是否已完成审批
        /// </summary>
        public bool IsCompleted => ApprovalStatus == "已批准" || ApprovalStatus == "已驳回";

        /// <summary>
        /// 是否待审批
        /// </summary>
        public bool IsPending => ApprovalStatus == "待审批";

        /// <summary>
        /// 状态颜色
        /// </summary>
        public string StatusColor
        {
            get
            {
                return ApprovalStatus switch
                {
                    "已批准" => "#4CAF50", // 绿色
                    "待审批" => "#2196F3", // 蓝色
                    "已驳回" => "#F44336", // 红色
                    _ => "#9E9E9E" // 灰色
                };
            }
        }

        /// <summary>
        /// 状态图标
        /// </summary>
        public string StatusIcon
        {
            get
            {
                return ApprovalStatus switch
                {
                    "已批准" => "✓",
                    "待审批" => "👤", 
                    "已驳回" => "✗",
                    _ => "⏰"
                };
            }
        }

        /// <summary>
        /// 状态背景色
        /// </summary>
        public string StatusBackground
        {
            get
            {
                return ApprovalStatus switch
                {
                    "已批准" => "#E8F5E8", // 浅绿色
                    "待审批" => "#E3F2FD", // 浅蓝色
                    "已驳回" => "#FFEBEE", // 浅红色
                    _ => "#F5F5F5" // 浅灰色
                };
            }
        }

        /// <summary>
        /// 状态文本
        /// </summary>
        public string StatusText
        {
            get
            {
                return ApprovalStatus switch
                {
                    "已批准" => "已批准",
                    "待审批" => "待审批", 
                    "已驳回" => "已驳回",
                    "待处理" => "待处理",
                    "未开始" => "未开始",
                    _ => ApprovalStatus
                };
            }
        }

        /// <summary>
        /// 状态文本颜色
        /// </summary>
        public string StatusForeground
        {
            get
            {
                return ApprovalStatus switch
                {
                    "已批准" => "#2E7D32", // 深绿色
                    "待审批" => "#1565C0", // 深蓝色
                    "已驳回" => "#C62828", // 深红色
                    _ => "#616161" // 深灰色
                };
            }
        }

        /// <summary>
        /// 审批意见/评论（兼容XAML中的Comments绑定）
        /// </summary>
        public string Comments => ApprovalComment;
    }
}