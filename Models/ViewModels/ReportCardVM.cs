using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Models.ViewModels
{
    public class ReportCardVM
    {
        public ReportCardVM()
        {
            this.subjectWiseScoreVMs = new List<SubjectWiseScoreVM>();
            this.topPerformersVMs = new List<TopPerformersVM>();
        }
        public int AttemptId { get; set; }
        public string Login { get; set; }
        public decimal TotalMarksScored { get; set; }
        public decimal? TotalOriginalMarks { get; set; }
        public int? TotalTestAttempts { get; set; }
        public int Rank { get; set; }
        public int TotalNumberQuestions { get; set; }
        public int TotalAnswered { get; set; }
        public string TotalTimeTaken { get; set; }
        public decimal TotalTestAccuracy { get; set; }
        public int TestId { get; set; }
        public string TestName { get; set; }
        public int?  CurrentRating { get; set; }
        public decimal  Percentage { get; set; }
        public virtual List<SubjectWiseScoreVM> subjectWiseScoreVMs { get; set; }
        public virtual List<TopPerformersVM> topPerformersVMs { get; set; }
    }
    public class SubjectWiseScoreVM
    {
        public string SubjectName { get; set; }
        public int TotalQuestions { get; set; }
        public decimal? OriginalScore { get; set; }
        public decimal YourScore { get; set; }
        public int NumberAnswered { get; set; }
        public int NumberNotAnswered { get; set; }
        public int NumberReview { get; set; }
        public int NumberNotVisited { get; set; }
        public int TotalCorrectAnswers { get; set; }
        public int TotalWrongAnswers { get; set; }
        public decimal Accuracy { get; set; }
        public string SubjectTimeSpent { get; set; }
    }
    public class TopPerformersVM
    {
        public string Login { get; set; }
        public string FullName { get; set; }
        public int? ImageFieldId { get; set; }
        public decimal? MarksScored { get; set; }
    }
}