using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Models.ViewModels
{
    public class ExcelQuestions
    {
        public string QuestionNumber { get; set; }
        public string Question { get; set; }
        public string QuestionOption1 { get; set; }
        public string QuestionOption2 { get; set; }
        public string QuestionOption3 { get; set; }
        public string QuestionOption4 { get; set; }
        public string QuestionOption5 { get; set; }
        public string AnswerOption1 { get; set; }
        public string AnswerOption2 { get; set; }
        public string AnswerOption3 { get; set; }
        public string AnswerOption4 { get; set; }
        public string AnswerOption5 { get; set; }
        public string QuestionSet { get; set; }
        public string CorrectAnswer { get; set; }
        public string Description { get; set; }
        public string Solution { get; set; }
     
    }
}