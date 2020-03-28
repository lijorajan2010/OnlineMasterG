using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Models.ViewModels
{
    public class MockTestAttemptVM
    {
        public MockTestAttemptVM()
        {
            this.MockTestAttemptDetails = new List<MockTestAttemptDetailVM>();
        }
        public int AttemptId { get; set; }
        public string Login { get; set; }
        public Nullable<int> TestId { get; set; }
        public bool IsPaused { get; set; }
        public int TimeLeftInMinutes { get; set; }
        public bool IsCompleted { get; set; }
        public string TestName { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string EditBy { get; set; }
        public Nullable<System.DateTime> EditDate { get; set; }
        public virtual TestVM Tests { get; set; }
        public virtual List<MockTestAttemptDetailVM> MockTestAttemptDetails { get; set; }
    }
    public  class MockTestAttemptDetailVM
    {
        public MockTestAttemptDetailVM()
        {
            this.ProblemsReporteds = new List<ProblemsReportedVM>();
        }
        public int AttemptDetailId { get; set; }
        public Nullable<int> AttemptId { get; set; }
        public Nullable<int> CourseId { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public Nullable<int> SubjectId { get; set; }
        public Nullable<int> QuestionsMockTestId { get; set; }
        public int QuestionNumber { get; set; }
        public Nullable<int> ChoosenAnswerChoiceId { get; set; }
        public Nullable<int> AnswerChoiceId { get; set; }
        public string AnswerStatus { get; set; }
        public decimal MarksScored { get; set; }
        public virtual CategoryVM Category { get; set; }
        public virtual CourseVM Course { get; set; }
        public virtual MockTestAttemptVM MockTestAttempt { get; set; }
        public virtual QuestionAnswerChoiceVM QuestionAnswerChoice { get; set; }
        public virtual QuestionsMockTestVM QuestionsMockTests { get; set; }
        public virtual SectionVM Section { get; set; }
        public virtual SubjectVM Subject { get; set; }
      
        public virtual List<ProblemsReportedVM> ProblemsReporteds { get; set; }
    }

    public class ProblemsReportedVM
    {
        public int ProblemReportId { get; set; }
        public Nullable<int> AttemptDetailId { get; set; }
        public int ProblemId { get; set; }
        public bool IsReported { get; set; }
        public virtual MockTestAttemptDetailVM MockTestAttemptDetail { get; set; }
        public virtual ProblemMasterVM ProblemMaster { get; set; }
    }
    public  class ProblemMasterVM
    {
        public ProblemMasterVM()
        {
            this.ProblemsReporteds = new List<ProblemsReportedVM>();
        }
        public int ProblemId { get; set; }
        public string Problem { get; set; }
        public bool IsActive { get; set; }
        public virtual List<ProblemsReportedVM> ProblemsReporteds { get; set; }
    }
}