using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    /// <summary>
    /// 项目类型-项目文档关联表
    /// </summary>
    public class ProjectTypeDocumentAssociationTable
    {
        [Key]
        public int ProjectTypeDocumentAssociationId { get; set; }
        public int ProjectsId { get; set; }
        public Projects Projects { get; set; }
        public int DocumentTypeId { get; set; }
        public DocumentType DocumentType { get; set; }
        public bool? IsNecessary { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int? DisplaySequence { get; set; }
    }
}
