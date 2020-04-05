using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Models.ViewModels
{
    public class DailyQuizAttemptVM
    {
        public DailyQuizAttemptVM()
        {
            this.dailyQuizAttemptVMDetail = new List<DailyQuizAttemptVMDetailVM>();
        }
        public int AttemptId { get; set; }
        public string Login { get; set; }
        public Nullable<int> TestId { get; set; }
        public Nullable<int> DailyQuizId { get; set; }
        public bool IsPaused { get; set; }
        public decimal? TimeLeftInMinutes { get; set; }
        public bool IsCompleted { get; set; }
        public string TestName { get; set; }
        public string QuizName { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string EditBy { get; set; }
        public Nullable<System.DateTime> EditDate { get; set; }
        public virtual TestVM Tests { get; set; }
        public virtual List<DailyQuizAttemptVMDetailVM> dailyQuizAttemptVMDetail { get; set; }
    }
    public class DailyQuizAttemptVMDetailVM
    {
        public int AttemptDetailId { get; set; }
        public Nullable<int> AttemptId { get; set; }
        public Nullable<int> DailyQuizCourseId { get; set; }
        public Nullable<int> DailyQuizSubjectId { get; set; }
        public Nullable<int> DailyQuizId { get; set; }
        public Nullable<int> QuizTestId { get; set; }
        public int QuestionNumber { get; set; }
        public Nullable<int> QuizQuestionAnswerChoiceId { get; set; }
        public Nullable<int> AnswerChoiceId { get; set; }
        public string AnswerStatus { get; set; }
        public decimal MarksScored { get; set; }
        public virtual DailyQuizVM DailyQuiz { get; set; }
        public virtual DailyQuizAttemptVM DailyQuizAttempt { get; set; }
        public virtual DailyQuizCourseVM DailyQuizCourse { get; set; }
        public virtual DailyQuizSubjectVM DailyQuizSubject { get; set; }
        public virtual QuestionAnswerChoiceVM QuizQuestionAnswerChoice { get; set; }
        public virtual QuizTestVM QuizTest { get; set; }
    }
    public class QuizTestVM
    {
        public int QuizTestId { get; set; }
        public string Question { get; set; }
        public string Description { get; set; }
        public string LanguageCode { get; set; }
        public bool Isactive { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> DailyQuizUploadId { get; set; }
        public Nullable<int> QuizImageFileId { get; set; }
        public int QuestionNumber { get; set; }
        public string QuestionSet { get; set; }
        public string Solution { get; set; }
        public List<QuestionAnswerChoiceVM> QuestionAnswerChoice { get; set; }
        public List<QuestionPointVM> QuestionPoint { get; set; }
    }
}