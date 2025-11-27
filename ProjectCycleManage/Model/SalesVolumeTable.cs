using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCycleManage.Model
{
    [Comment("年度销售预测")]
    public class SalesVolumeTable
    {
        public int Id { get; set; }


        [Comment("年份")]
        public int Year { get; set; }


        [Comment("销售预测")]
        public double SalesVolume { get; set; }

        [Comment("1月销售预测")]
        public double? JanuarySalesVolume { get; set; }

        [Comment("2月销售预测")]
        public double? FebruarySalesVolume { get; set; }

        [Comment("3月销售预测")]
        public double? MarchSalesVolume { get; set; }

        [Comment("4月销售预测")]
        public double? AprilSalesVolume { get; set; }

        [Comment("5月销售预测")]
        public double? MaySalesVolume { get; set; }

        [Comment("6月销售预测")]
        public double? JuneSalesVolume { get; set; }

        [Comment("7月销售预测")]
        public double? JulySalesVolume { get; set; }

        [Comment("8月销售预测")]
        public double? AugustSalesVolume { get; set; }

        [Comment("9月销售预测")]
        public double? SeptemberSalesVolume { get; set; }

        [Comment("10月销售预测")]
        public double? OctoberSalesVolume { get; set; }

        [Comment("11月销售预测")]
        public double? NovemberSalesVolume { get; set; }

        [Comment("12月销售预测")]
        public double? DecemberSalesVolume { get; set; }

    }
}
