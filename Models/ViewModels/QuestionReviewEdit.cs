using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Models.ViewModels
{
    public class QuestionReviewEdit
    {
        public virtual List<QuestionSet> EditQuestionSet { get; set; }
        public QuestionReviewEdit()
        {
            EditQuestionSet = new List<QuestionSet>();
        }
    }

    public class QuestionSet
    {
        public int QuestionUploadId { get; set; }
        public int? QuestionDescriptionImageFileId { get; set; }
        public string Description { get; set; }
        public string Direction { get; set; }
        public virtual List<QuestionsMockTestReview> EditQuestions { get; set; }
        public QuestionSet()
        {
            EditQuestions = new List<QuestionsMockTestReview>();
        }
    }

    public class QuestionsMockTestReview
    {
        public int QuestionsMockTestId { get; set; }
        public string Question { get; set; }
        public Nullable<int> QuestionImageFileId { get; set; }
        public Nullable<int> QuestionDescriptionImageFileId { get; set; }
        public int QuestionNumber { get; set; }
        public string QuestionSet { get; set; }
        public string Solution { get; set; }
        public string CorrectAnswer { get; set; }
        public string Description { get; set; }
        public string Direction { get; set; }
        public int QuestionUploadId { get; set; }
        public virtual List<QuestionPointReviewEdit> EditQuestionPoints { get; set; }
        public virtual List<QuestionAnswerChoiceReviewEdit> EditAnswerChoice { get; set; }
        public QuestionsMockTestReview()
        {
            EditQuestionPoints = new List<QuestionPointReviewEdit>();
            EditAnswerChoice = new List<QuestionAnswerChoiceReviewEdit>();
        }
    }
    public class QuestionAnswerChoiceReviewEdit
    {
        public int QuestionAnswerChoiceId { get; set; }
        public string ChoiceId { get; set; }
        public string QuestionAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public Nullable<int> QuestionsMockTestId { get; set; }

    }
    public class QuestionPointReviewEdit
    {
        public int QuestionPointId { get; set; }
        public string QPoint { get; set; }
        public Nullable<int> QuestionsMockTestId { get; set; }

    }

}