using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Models.ViewModels
{
    public class ExamUpdateSectionVM
    {
        public int SectionId { get; set; }
        public int? SectionLinkId { get; set; }
        public string SectionName { get; set; }
        public int LinkId { get; set; }
        public string Link { get; set; }
        public string LinkDescription { get; set; }
        public int? Sequence { get; set; }
        public string LanguageCode { get; set; }
        public bool IsActive { get; set; }
    }
}