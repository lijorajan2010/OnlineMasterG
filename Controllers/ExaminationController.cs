﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineMasterG.Base;
using OnlineMasterG.Code;
using OnlineMasterG.CommonFramework;
using OnlineMasterG.CommonServices;
using OnlineMasterG.DomainLogic;
using OnlineMasterG.Models.DAL;
using OnlineMasterG.Models.ViewModels;

namespace OnlineMasterG.Controllers
{
    [ActionAuthorization("PUBLICACTION")]
    public class ExaminationController : BaseController
    {
        // GET: Examination
        public ActionResult Index(string p, string r, string s)
        {

            int TestId = 0;
            int ResumeAttemptId = 0;
            bool isReAttempt = false;
            if (!string.IsNullOrEmpty(p))
            {
                TestId = int.Parse(CustomEncrypt.SafeUrlDecrypt(p));
            }
            if (!string.IsNullOrEmpty(r))
            {
                ResumeAttemptId = int.Parse(CustomEncrypt.SafeUrlDecrypt(r));
            }
            if (!string.IsNullOrEmpty(s))
            {
                isReAttempt = Convert.ToBoolean(CustomEncrypt.SafeUrlDecrypt(s));
            }
        
            if (TestId == 0)
            {
                return PartialView("Error");
            }
            var TestDetails = TestService.Fetch(TestId);

            string CurrentLogin = HttpContext.User.Identity.Name;
            MockTestAttemptVM model = new MockTestAttemptVM();
            if (!string.IsNullOrEmpty(CurrentLogin) && TestDetails != null)
            {
                model = ExamLogics.GetMockTestAttemptDetails(CurrentLogin, CurrentLogin, TestId, ResumeAttemptId, isReAttempt);
            }
            else
            {
                RedirectToAction("Index", "Login");
            }

            ViewBag.ProblemsList = ExamService.GetProblemMasters();
            ViewBag.AnswerStatus = ExamLogics.getAnswerStatuses();

            if (model.MockTestAttemptDetails.Count() == 0)
            {
                return PartialView("Error");
            }
            
            return View(model);

        }
        [HttpPost]
        public ActionResult StudentFromAnswerSubmit(MockTestAttemptVM model)
        {
            var AttemptSaved = ExamLogics.SaveExamAttempts(model, HttpContext.User.Identity.Name);
            if (AttemptSaved.Status)
            {
                if ((Convert.ToBoolean(AttemptSaved.Data) == true))
                {
                    return RedirectToAction("Index", "MockTests");
                }
                return RedirectToAction("ReportCard", new { p = CustomEncrypt.Encrypt(AttemptSaved.ReturnId.ToString()) });
            }
            else
            {
                return View();
            }

        }
        public ActionResult ReportCard(string p)
        {

            int AttemptId = 0;
            if (!string.IsNullOrEmpty(p))
            {
                AttemptId = int.Parse(CustomEncrypt.SafeUrlDecrypt(p));
            }

            ReportCardVM model = new ReportCardVM();


            var AttempDetails = ExamService.Fetch(AttemptId);
            var TestDetails = TestService.Fetch(AttempDetails.TestId);

            model.AttemptId = AttempDetails.AttemptId;
            model.TestId = TestDetails.TestId;
            model.TestName = TestDetails.TestName;
            model.Login = AttempDetails.Login;
            model.CurrentRating = AttempDetails.Rating;
            model.TotalMarksScored = AttempDetails.MockTestAttemptDetails.Where(m => m.IsAnswerCorrect == true).Sum(m => m.MarksScored);

            model.TotalTestAttempts = ExamService.GetAttemptListByTestId(AttempDetails?.TestId).Count();
            model.Rank = GetRankOftheStudent(AttempDetails);


            List<SubjectWiseScoreVM> subjectWiseScoreVMs = new List<SubjectWiseScoreVM>();
            List<TopPerformersVM> topPerformersVMs = new List<TopPerformersVM>();
            List<int?> loopCount = AttempDetails.MockTestAttemptDetails.Select(m => m.SubjectId).Distinct().ToList();
            if (loopCount != null && loopCount.Count() > 0)
            {
                foreach (var item in loopCount)
                {
                    var MockDetails = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item).FirstOrDefault();
                    SubjectWiseScoreVM subjectWiseScoreVM = new SubjectWiseScoreVM();

                    subjectWiseScoreVM.SubjectName = SubjectService.Fetch(item).SubjectName;
                    subjectWiseScoreVM.TotalQuestions = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item).Count();
                    subjectWiseScoreVM.NumberAnswered = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item && m.AnswerStatus == ExamLogics.AnswerStatus.ANSWERED.ToString()).Count();
                    subjectWiseScoreVM.NumberNotAnswered = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item && m.AnswerStatus == ExamLogics.AnswerStatus.NOTANSWERED.ToString()).Count();
                    subjectWiseScoreVM.NumberNotVisited = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item && m.AnswerStatus == ExamLogics.AnswerStatus.NOTATTEMPTED.ToString()).Count();
                    subjectWiseScoreVM.NumberReview = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item && m.AnswerStatus == ExamLogics.AnswerStatus.MARKED.ToString()).Count();
                    subjectWiseScoreVM.OriginalScore = TestDetails.GeneralInstructions.Where(m => m.TestId == AttempDetails.TestId && m.SubjectId == item).Sum(m => m.CorrectMarks) * subjectWiseScoreVM.TotalQuestions;
                    subjectWiseScoreVM.YourScore = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item).Sum(m => m.MarksScored);
                    decimal? Minutes = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item).FirstOrDefault().SubjectTimeUsed;
                    double MinutesNotNull = Convert.ToDouble(Minutes.HasValue ? Minutes.Value : 0);
                    TimeSpan spWorkMin = TimeSpan.FromMinutes(MinutesNotNull);
                    subjectWiseScoreVM.SubjectTimeSpent = string.Format("{0:00} Hours {1:00} Minutes {2:00} Seconds", (int)spWorkMin.TotalHours, spWorkMin.Minutes, spWorkMin.Seconds);
                    if (subjectWiseScoreVM.YourScore != 0 && subjectWiseScoreVM.OriginalScore.Value != 0)
                    {
                        subjectWiseScoreVM.Accuracy = (subjectWiseScoreVM.YourScore / subjectWiseScoreVM.OriginalScore.Value) * 100;
                    }

                    subjectWiseScoreVM.TotalCorrectAnswers = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item && m.IsAnswerCorrect == true).Count();
                    subjectWiseScoreVM.TotalWrongAnswers = (subjectWiseScoreVM.TotalQuestions - AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item && m.IsAnswerCorrect == true).Count());


                    subjectWiseScoreVMs.Add(subjectWiseScoreVM);
                }

            }
            model.subjectWiseScoreVMs = subjectWiseScoreVMs;
            model.topPerformersVMs = GetTopPerformers(AttempDetails);
            model.TotalOriginalMarks = subjectWiseScoreVMs.Sum(m => m.OriginalScore);
            model.TotalCorrectAnswers = subjectWiseScoreVMs.Sum(m => m.TotalCorrectAnswers);
            model.TotalQuestions = subjectWiseScoreVMs.Sum(m => m.TotalQuestions);
            
            if (model.TotalCorrectAnswers != 0 && model.TotalQuestions != 0)
            {
                model.TotalTestAccuracy = (model.TotalCorrectAnswers / model.TotalQuestions) * 100;
            }
            if (model.TotalMarksScored != 0 && model.TotalOriginalMarks.Value != 0)
            {
                model.Percentage = (model.TotalMarksScored / model.TotalOriginalMarks.Value) * 100;
            }


            return View(model);
        }

        private List<TopPerformersVM> GetTopPerformers(MockTestAttempt attempDetails)
        {
            List<TopPerformersVM> topPerformersVMs = new List<TopPerformersVM>();
            var AttempstOftheTest = ExamService.GetAttemptListByTestId(attempDetails?.TestId);
            var List = AttempstOftheTest.OrderByDescending(m => m.FinalMarksScoredForRank)
                                    .Select((grp, i) => new
                                    {
                                        Login = grp.Login,
                                        FullName = UserService.Fetch(grp.Login).FirstName + " " + UserService.Fetch(grp.Login).LastName,
                                        MarkScored = grp.FinalMarksScoredForRank,
                                        Rank = i + 1
                                    })
                                    .ToList();
            if (List != null && List.Count() > 0)
            {
                foreach (var item in List)
                {
                    topPerformersVMs.Add(new TopPerformersVM()
                    {
                        FullName = item.FullName,
                        MarksScored = item.MarkScored,
                    });
                }
            }

            return topPerformersVMs;
        }

        private int GetRankOftheStudent(MockTestAttempt attempDetails)
        {
            int rank = 0;
            // Rank need to be calculated
            if (attempDetails != null)
            {
                var AttempstOftheTest = ExamService.GetAttemptListByTestId(attempDetails?.TestId);
                rank = AttempstOftheTest.OrderByDescending(m => m.FinalMarksScoredForRank)
                                        .Select((grp, i) => new
                                        {
                                            Login = grp.Login,
                                            Rank = i + 1
                                        })
                                                    .ToList().Where(m => m.Login == attempDetails.Login).Select(m => m.Rank).FirstOrDefault();

            }


            return rank;
        }

        [HttpGet]
        public ActionResult Solution(string p)
        {
            int AttemptId = 0;
            if (!string.IsNullOrEmpty(p))
            {
                AttemptId = int.Parse(CustomEncrypt.SafeUrlDecrypt(p));
            }

            MockTestAttemptVM model = new MockTestAttemptVM();
            var AttempDetails = ExamService.Fetch(AttemptId);
            var TestDetails = TestService.Fetch(AttempDetails.TestId);
            model = ExamLogics.GetMockTestAttemptDetailsByAttemptId(AttemptId);

            model.Rank = GetRankOftheStudent(AttempDetails);
            model.TotalMarksScored = AttempDetails.MockTestAttemptDetails.Where(m => m.IsAnswerCorrect == true).Sum(m => m.MarksScored);
            model.TotalTestAttempts = ExamService.GetAttemptListByTestId(AttempDetails?.TestId).Count();
            if (model.Rank != 0 && model.TotalTestAttempts.Value != 0)
            {
                model.Percentage = (model.Rank / model.TotalTestAttempts.Value) * 100;
            }

            List<SubjectWiseScoreVM> subjectWiseScoreVMs = new List<SubjectWiseScoreVM>();
            List<int?> loopCount = AttempDetails.MockTestAttemptDetails.Select(m => m.SubjectId).Distinct().ToList();
            if (loopCount != null && loopCount.Count() > 0)
            {
                foreach (var item in loopCount)
                {
                    var MockDetails = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item).FirstOrDefault();
                    SubjectWiseScoreVM subjectWiseScoreVM = new SubjectWiseScoreVM();

                    subjectWiseScoreVM.SubjectName = SubjectService.Fetch(item).SubjectName;
                    subjectWiseScoreVM.TotalQuestions = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item).Count();
                    subjectWiseScoreVM.NumberAnswered = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item && m.AnswerStatus == ExamLogics.AnswerStatus.ANSWERED.ToString()).Count();
                    subjectWiseScoreVM.NumberNotAnswered = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item && m.AnswerStatus == ExamLogics.AnswerStatus.NOTANSWERED.ToString()).Count();
                    subjectWiseScoreVM.NumberNotVisited = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item && m.AnswerStatus == ExamLogics.AnswerStatus.NOTATTEMPTED.ToString()).Count();
                    subjectWiseScoreVM.NumberReview = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item && m.AnswerStatus == ExamLogics.AnswerStatus.MARKED.ToString()).Count();
                    subjectWiseScoreVM.OriginalScore = TestDetails.GeneralInstructions.Where(m => m.TestId == AttempDetails.TestId && m.SubjectId == item).Sum(m => m.CorrectMarks) * subjectWiseScoreVM.TotalQuestions;
                    subjectWiseScoreVM.YourScore = AttempDetails.MockTestAttemptDetails.Where(m => m.IsAnswerCorrect == true && m.SubjectId == item).Sum(m => m.MarksScored);
                    decimal? Minutes = AttempDetails.MockTestAttemptDetails.Where(m => m.SubjectId == item).FirstOrDefault().SubjectTimeUsed;
                    double MinutesNotNull = Convert.ToDouble(Minutes.HasValue ? Minutes.Value : 0);
                    TimeSpan spWorkMin = TimeSpan.FromMinutes(MinutesNotNull);
                    subjectWiseScoreVM.SubjectTimeSpent = string.Format("{0:00} Hours {1:00} Minutes {2:00} Seconds", (int)spWorkMin.TotalHours, spWorkMin.Minutes, spWorkMin.Seconds);

                    if (subjectWiseScoreVM.YourScore != 0 && subjectWiseScoreVM.OriginalScore.Value != 0)
                    {
                        subjectWiseScoreVM.Accuracy = (subjectWiseScoreVM.YourScore / subjectWiseScoreVM.OriginalScore.Value) * 100;
                    }
                    subjectWiseScoreVMs.Add(subjectWiseScoreVM);
                }

            }

            model.TotalOriginalMarks = subjectWiseScoreVMs.Sum(m => m.OriginalScore);
            if (model.TotalMarksScored != 0 && model.TotalOriginalMarks.Value != 0)
            {
                model.TotalTestAccuracy = (model.TotalMarksScored / model.TotalOriginalMarks.Value) * 100;

            }
            if (model.MockTestAttemptDetails.Any(m => m.QuestionsMockTests == null))
            {
                return PartialView("Error");
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult AddRating(int AttemptId, int? Rating, string Review)
        {
            ExamService.AddRatingOfTest(AttemptId, Rating, Review);
            return Json("");
        }
    }
}