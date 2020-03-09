using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Models.ViewModels
{
    public class SectionVM
    {
        public int? CategoryId { get; set; }
        public int? CourseId { get; set; }
        public int SectionId { get; set; }
        public string CategoryName { get; set; }
        public string CourseName { get; set; }
        public string SectionName { get; set; }
        public string Description { get; set; }
        public string LanguageCode { get; set; }
        public bool IsActive { get; set; }
    }
}