using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Models.ViewModels
{
    public class QuestionUploadVM
    {
        public int QuestionUploadId { get; set; }
        public int? SubjectId { get; set; }
        public int? TestId { get; set; }
        public int? CategoryId { get; set; }
        public int? CourseId { get; set; }
        public int? SectionId { get; set; }
        public string Description { get; set; }
        public string Direction { get; set; }
        public string SubjectName { get; set; }
        public string TestName { get; set; }
        public string CourseName { get; set; }
        public string CategoryName { get; set; }
        public string SectionName { get; set; }
        public string QuestionStatus { get; set; }
        public string LanguageCode { get; set; }
        public bool IsActive { get; set; }
        public string Question { get; set; }
        public string QuestionSet { get; set; }
        public HttpPostedFileBase postedFile { get; set; }
        public System.DateTime CreateOn { get; set; }
    }
}