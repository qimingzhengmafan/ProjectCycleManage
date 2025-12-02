using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCycleManage.Model
{
    [Comment("年度预算")]
    public class AnnualBudget
    {
        public int Id { get; set; }
        public int Year { get; set; }
        [Comment("预算")]
        public int Budget { get; set; }
        
        [Comment("1月预算")]
        public double? JanuaryBudget { get; set; }
        
        [Comment("2月预算")]
        public double? FebruaryBudget { get; set; }
        
        [Comment("3月预算")]
        public double? MarchBudget { get; set; }
        
        [Comment("4月预算")]
        public double? AprilBudget { get; set; }
        
        [Comment("5月预算")]
        public double? MayBudget { get; set; }
        
        [Comment("6月预算")]
        public double? JuneBudget { get; set; }
        
        [Comment("7月预算")]
        public double? JulyBudget { get; set; }
        
        [Comment("8月预算")]
        public double? AugustBudget { get; set; }
        
        [Comment("9月预算")]
        public double? SeptemberBudget { get; set; }
        
        [Comment("10月预算")]
        public double? OctoberBudget { get; set; }
        
        [Comment("11月预算")]
        public double? NovemberBudget { get; set; }
        
        [Comment("12月预算")]
        public double? DecemberBudget { get; set; }
    }
}
