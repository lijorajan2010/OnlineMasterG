using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Models.ViewModels
{
    public class ReportedQuestionsVM
    {
        public string CandidateName { get; set; }
        public string ReportedQuestion { get; set; }
        public string TestName { get; set; }
        public string CategoryName { get; set; }
        public string SectionName { get; set; }
        public string SubjectName { get; set; }
    }
}