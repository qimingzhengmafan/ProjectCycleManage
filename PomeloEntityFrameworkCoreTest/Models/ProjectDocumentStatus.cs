using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomeloEntityFrameworkCoreTest.Models
{
    /// <summary>
    /// 项目文档状态表
    /// </summary>
    public class ProjectDocumentStatus
    {
        public int ProjectDocumentStatusId { get; set; }

        /// <summary>
        /// 项目序号Id
        /// </summary>
        public int ProjectsId { get; set; }
        public Projects Projects { get; set; }
        public int DocumentTypeId { get; set; }
        public DocumentType DocumentType { get; set; } 
        public bool IsHasDocument { get; set; }
        public DateTime TheLastUpDateTime { get; set; }
        public int UpdatePeopleId { get; set; }
        public PeopleTable UpdatePeople { get; set; }
        public string Remarks { get; set; }

    }
}
