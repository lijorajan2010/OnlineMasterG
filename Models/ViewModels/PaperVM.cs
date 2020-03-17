using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Models.ViewModels
{
    public class PaperVM
    {
        public int? SubjectId { get; set; }
        public int? CourseId { get; set; }
        public int? ClassId { get; set; }
        public int? SectionId { get; set; }
        public int PaperId { get; set; }
        public string SubjectName { get; set; }
        public string CourseName { get; set; }
        public string PaperName { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string Description { get; set; }
        public string LanguageCode { get; set; }
        public int? DataFileId { get; set; }
        public bool IsActive { get; set; }
        public virtual DataFileVM DataFile { get; set; }
    }
}