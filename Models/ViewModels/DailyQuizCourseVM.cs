using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Models.ViewModels
{
    public class DailyQuizCourseVM
    {
        public int DailyQuizCourseId { get; set; }
        public string DailyQuizCourseName { get; set; }
        public int? Sequence { get; set; }
        public string LanguageCode { get; set; }
        public bool IsActive { get; set; }
    }
}