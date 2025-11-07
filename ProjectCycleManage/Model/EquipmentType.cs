using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    /// <summary>
    /// 设备类型表
    /// </summary>
    /// 
    [Comment("设备类型-例：非标外购")]
    public class EquipmentType
    {
        public int EquipmentTypeId { get; set; }
        public string EquipmentName { get; set; }
    }
}
