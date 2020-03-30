using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineMasterG.Base;
using OnlineMasterG.CommonServices;
using OnlineMasterG.DomainLogic;
using OnlineMasterG.Models.DAL;
using OnlineMasterG.Models.ViewModels;

namespace OnlineMasterG.Controllers
{
    public class ExaminationController : BaseController
    {
        // GET: Examination
        public ActionResult Index(int TestId)
        {
            var TestDetails = TestService.Fetch(TestId);

            string CurrentLogin = HttpContext.User.Identity.Name;
            MockTestAttemptVM model = new MockTestAttemptVM();
            if (!string.IsNullOrEmpty(CurrentLogin) && TestDetails != null)
            {
                model = ExamLogics.GetMockTestAttemptDetails(CurrentLogin, TestId, CurrentLogin);
            }
            else
            {
                RedirectToAction("Index", "Login");
            }

            ViewBag.ProblemsList = ExamService.GetProblemMasters();
            ViewBag.AnswerStatus = ExamLogics.getAnswerStatuses();
            return View(model);
        }
        [HttpPost]
        public ActionResult StudentFromAnswerSubmit(MockTestAttemptVM model)
        {
            var AttemptSaved = ExamLogics.SaveExamAttempts(model, HttpContext.User.Identity.Name);
            if (AttemptSaved.Status)
            {
                return RedirectToAction("ReportCard", new { AttemptId = AttemptSaved.ReturnId });
            }
            else
            {
                return View();
            }

        }
        public ActionResult ReportCard(int AttemptId)
        {
            ReportCardVM model = new ReportCardVM();

            var AttempDetails = ExamService.Fetch(AttemptId);

            model.TestId = AttempDetails.MockTest.TestId;
            model.TestName = AttempDetails.MockTest.TestName;
            model.Login = AttempDetails.Login;
            model.TotalMarksScored = AttempDetails.MockTestAttemptDetails.Where(m => m.IsAnswerCorrect == true).Sum(m => m.MarksScored);
            model.TotalOriginalMarks = AttempDetails.MockTest.GeneralInstructions.Where(m => m.TestId == AttempDetails.TestId).Sum(m => m.CorrectMarks) * AttempDetails.MockTestAttemptDetails.Count();
            model.TotalTestAttempts = ExamService.GetAttemptListByTestId(AttempDetails?.TestId).Count();
            model.Rank = GetRankOftheStudent(AttempDetails);
            model.Percentage = (model.Rank / model.TotalTestAttempts.Value) * 100;
            model.TotalTestAccuracy = (model.TotalMarksScored / model.TotalOriginalMarks.Value) * 100;

            List<SubjectWiseScoreVM> subjectWiseScoreVMs = new List<SubjectWiseScoreVM>();
            List<TopPerformersVM> topPerformersVMs = new List<TopPerformersVM>();
            List<int?> loopCount = AttempDetails.MockTestAttemptDetails.Select(m => m.SubjectId).Distinct().ToList();
            if (loopCount != null && loopCount.Count() > 0)
            {
                foreach (var item in loopCount)
                {
                    var MockDetails = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item).FirstOrDefault();
                    SubjectWiseScoreVM subjectWiseScoreVM = new SubjectWiseScoreVM();

                    subjectWiseScoreVM.SubjectName = MockDetails.Subject.SubjectName;
                    subjectWiseScoreVM.TotalQuestions = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item).Count();
                    subjectWiseScoreVM.NumberAnswered = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item && m.AnswerStatus == ExamLogics.AnswerStatus.ANSWERED.ToString()).Count();
                    subjectWiseScoreVM.NumberNotAnswered = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item && m.AnswerStatus == ExamLogics.AnswerStatus.NOTANSWERED.ToString()).Count();
                    subjectWiseScoreVM.NumberNotVisited = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item && m.AnswerStatus == ExamLogics.AnswerStatus.NOTATTEMPTED.ToString()).Count();
                    subjectWiseScoreVM.NumberReview = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item && m.AnswerStatus == ExamLogics.AnswerStatus.MARKED.ToString()).Count();
                    subjectWiseScoreVM.OriginalScore = AttempDetails.MockTest.GeneralInstructions.Where(m => m.TestId == AttempDetails.TestId && m.SubjectId == item).Sum(m => m.CorrectMarks) * subjectWiseScoreVM.TotalQuestions;
                    subjectWiseScoreVM.YourScore = AttempDetails.MockTestAttemptDetails.Where(m => m.IsAnswerCorrect == true && m.SubjectId == item).Sum(m => m.MarksScored);
                    subjectWiseScoreVM.SubjectTimeSpent = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item).FirstOrDefault().SubjectTimeUsed;
                    subjectWiseScoreVM.Accuracy = (subjectWiseScoreVM.YourScore / subjectWiseScoreVM.OriginalScore.Value) *100;


                    subjectWiseScoreVMs.Add(subjectWiseScoreVM);
                }

            }
            model.subjectWiseScoreVMs = subjectWiseScoreVMs;
            model.topPerformersVMs = topPerformersVMs;

            return View(model);
        }

        private int GetRankOftheStudent(MockTestAttempt attempDetails)
        {
            int rank = 0;
            // Rank need to be calculated
            //if (attempDetails!=null)
            //{
            //    var AttempstOftheTest = ExamService.GetAttemptListByTestId(attempDetails?.TestId)
            //}


            return rank;
        }

        public ActionResult Solution()
        {
            return View();
        }
    }
}