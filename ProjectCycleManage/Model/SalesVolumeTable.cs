using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCycleManage.Model
{
    [Comment("年度销售额")]
    public class SalesVolumeTable
    {
        public int Id { get; set; }


        [Comment("年份")]
        public int Year { get; set; }


        [Comment("销售额")]
        public double SalesVolume { get; set; }



    }
}
