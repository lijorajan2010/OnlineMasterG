using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Models.ViewModels
{
    public class GenneralInstructionVM
    {
        public int? TestId { get; set; }
        public int? SubjectId { get; set; }
        public string TestName { get; set; }
        public  string SubjectName { get; set; }
        public  string CourseName { get; set; }
        public  string CategoryName { get; set; }
        public  string SectionName { get; set; }
        public int InstructionId { get; set; }
        public decimal? CorrectMarks { get; set; }
        public decimal? NegativeMarks { get; set; }
        public int QuestionCount { get; set; }
        public string LanguageCode { get; set; }
        public bool IsActive { get; set; }
    }
}