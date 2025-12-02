using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCycleManage.Model
{
    [Comment("资产金额修正")]
    public class AssetAmountCorrection
    {
        public int Id { get; set; }
        public int Year { get; set; }
        
        [Comment("1月修正金额")]
        public double? JanuaryCorrection { get; set; }
        
        [Comment("2月修正金额")]
        public double? FebruaryCorrection { get; set; }
        
        [Comment("3月修正金额")]
        public double? MarchCorrection { get; set; }
        
        [Comment("4月修正金额")]
        public double? AprilCorrection { get; set; }
        
        [Comment("5月修正金额")]
        public double? MayCorrection { get; set; }
        
        [Comment("6月修正金额")]
        public double? JuneCorrection { get; set; }
        
        [Comment("7月修正金额")]
        public double? JulyCorrection { get; set; }
        
        [Comment("8月修正金额")]
        public double? AugustCorrection { get; set; }
        
        [Comment("9月修正金额")]
        public double? SeptemberCorrection { get; set; }
        
        [Comment("10月修正金额")]
        public double? OctoberCorrection { get; set; }
        
        [Comment("11月修正金额")]
        public double? NovemberCorrection { get; set; }
        
        [Comment("12月修正金额")]
        public double? DecemberCorrection { get; set; }

    }
}