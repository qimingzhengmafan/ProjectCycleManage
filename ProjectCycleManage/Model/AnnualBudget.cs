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
    }
}
