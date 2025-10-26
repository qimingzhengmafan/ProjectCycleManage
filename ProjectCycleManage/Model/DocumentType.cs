using Microsoft.EntityFrameworkCore;
using ProjectCycleManage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    /// <summary>
    /// 文档类型表
    /// </summary>
    public class DocumentType
    {
        public int DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; }

        [Comment("权限分级")]
        public string? Permission { get; set; }

        [Comment("文档类型-例-文档-OA")]
        public int? FileTypesDataId { get; set; }
        public FileTypesTable? FileTypesData { get; set; }
    }
}
