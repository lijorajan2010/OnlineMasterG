using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Models.ViewModels
{
    public class DailyQuizUploadVM
    {
        public int DailyQuizUploadId { get; set; }
        public int? DailyQuizId { get; set; }
        public int? DailyQuizCourseId { get; set; }
        public int? DataFileId { get; set; }
        public int? DailyQuizSubjectId { get; set; }
        public int NoOfQuestions { get; set; }
        public string DailyQuizName { get; set; }
        public string DailyQuizSubjectName { get; set; }
        public string DailyQuizCourseName { get; set; }
        public string Description { get; set; }
        public string LanguageCode { get; set; }
        public string QuestionStatus { get; set; }
        public DataFileVM DataFile { get; set; }
        public bool Isactive { get; set; }
        public HttpPostedFileBase postedFile { get; set; }
    }
}