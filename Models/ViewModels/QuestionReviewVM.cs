using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Models.ViewModels
{
    public class QuestionReviewVM
    {
        public int QuestionUploadId { get; set; }
        public string QuestionStatus { get; set; }
        public string LanguageCode { get; set; }
        public bool Isactive { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public int TestId { get; set; }
        public string TestName { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public virtual List<QuestionsMockTestVM> QuestionsMockTests { get; set; }
        public QuestionReviewVM()
        {
            QuestionsMockTests = new List<QuestionsMockTestVM>();
        }
    }

    public class QuestionsMockTestVM
    {
        public QuestionsMockTestVM()
        {
            this.QuestionAnswerChoices = new HashSet<QuestionAnswerChoiceVM>();
            this.QuestionPoints = new HashSet<QuestionPointVM>();
        }

        public int QuestionsMockTestId { get; set; }
        public string Question { get; set; }
        public string Description { get; set; }
        public string LanguageCode { get; set; }
        public bool Isactive { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> QuestionUploadId { get; set; }
        public Nullable<int> QuestionImageFileId { get; set; }
        public int QuestionNumber { get; set; }
        public string QuestionSet { get; set; }
        public string Solution { get; set; }
        public string CorrectAnswer { get; set; }
        public List<int> OptionList { get; set; }
        public virtual ICollection<QuestionAnswerChoiceVM> QuestionAnswerChoices { get; set; }
        public virtual ICollection<QuestionPointVM> QuestionPoints { get; set; }
    }
    public class QuestionAnswerChoiceVM
    {
        public int QuestionAnswerChoiceId { get; set; }
        public string QuestionAnswer { get; set; }
        public int ChoiceId { get; set; }
        public bool IsCorrect { get; set; }
        public Nullable<int> QuestionsMockTestId { get; set; }

    }
    public class QuestionPointVM
    {
        public int QuestionPointId { get; set; }
        public string QPoint { get; set; }
        public Nullable<int> QuestionsMockTestId { get; set; }

    }

}