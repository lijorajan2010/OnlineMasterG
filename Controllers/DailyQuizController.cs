using OnlineMasterG.Code;
using OnlineMasterG.CommonFramework;
using OnlineMasterG.CommonServices;
using OnlineMasterG.DomainLogic;
using OnlineMasterG.Models.DAL;
using OnlineMasterG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMasterG.Controllers
{
    [ActionAuthorization()]
    public class DailyQuizController : Controller
    {
        // GET: DailyQuiz
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DailyQuiz(string p)
        {
            int DailyQuizId = 0;
            if (!string.IsNullOrEmpty(p))
            {
                DailyQuizId = int.Parse(CustomEncrypt.SafeUrlDecrypt(p));
            }
            var DailyQuizDetails = DailyQuizService.FetchDailyQuiz(DailyQuizId);

            string CurrentLogin = HttpContext.User.Identity.Name;
            DailyQuizAttemptVM model = new DailyQuizAttemptVM();
            if (!string.IsNullOrEmpty(CurrentLogin) && DailyQuizDetails != null)
            {
                model = DailyQuizLogics.GetDailyQuizAttemptDetails(CurrentLogin, DailyQuizId, CurrentLogin);
            }
            else
            {
                RedirectToAction("Index", "Login");
            }

            ViewBag.AnswerStatus = ExamLogics.getAnswerStatuses();
            return View(model);
        }
        [HttpPost]
        public ActionResult DailyQuizAnswerSubmit(DailyQuizAttemptVM model)
        {
            var AttemptSaved = DailyQuizLogics.SaveDailyQuizAttempts(model, HttpContext.User.Identity.Name);
            if (AttemptSaved.Status)
            {
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


            var AttempDetails = DailyQuizService.GetDailyQuizAttempt(AttemptId);
            var TestDetails = DailyQuizService.FetchDailyQuiz(AttempDetails.DailyQuizId);

            model.AttemptId = AttempDetails.AttemptId;
            model.TestId = TestDetails.DailyQuizId;
            model.TestName = TestDetails.DailyQuizName;
            model.Login = AttempDetails.Login;
            model.CurrentRating = AttempDetails.Rating;
            model.TotalMarksScored = AttempDetails.DailyQuizAttemptDetails.Where(m => m.IsAnswerCorrect == true).Sum(m => m.MarksScored);

            model.TotalTestAttempts = DailyQuizService.GetDailyQuizAttemptList(AttempDetails?.DailyQuizId).Count();
            model.Rank = GetRankOftheStudent(AttempDetails);
            //model.Percentage = (model.Rank / model.TotalTestAttempts.Value) * 100;


            List<SubjectWiseScoreVM> subjectWiseScoreVMs = new List<SubjectWiseScoreVM>();
            List<TopPerformersVM> topPerformersVMs = new List<TopPerformersVM>();
            List<int?> loopCount = AttempDetails.DailyQuizAttemptDetails.Select(m => m.DailyQuizSubjectId).Distinct().ToList();
            if (loopCount != null && loopCount.Count() > 0)
            {
                foreach (var item in loopCount)
                {
                    var MockDetails = AttempDetails.DailyQuizAttemptDetails.Where(m => m.DailyQuizSubjectId == item).FirstOrDefault();
                    SubjectWiseScoreVM subjectWiseScoreVM = new SubjectWiseScoreVM();

                    subjectWiseScoreVM.SubjectName = DailyQuizService.FetchDailyQuizSubject(item).DailyQuizSubjectName;
                    subjectWiseScoreVM.TotalQuestions = AttempDetails.DailyQuizAttemptDetails.Where(m => m.DailyQuizSubjectId == item).Count();
                    subjectWiseScoreVM.NumberAnswered = AttempDetails.DailyQuizAttemptDetails.Where(m => m.DailyQuizSubjectId == item && m.AnswerStatus == ExamLogics.AnswerStatus.ANSWERED.ToString()).Count();
                    subjectWiseScoreVM.NumberNotAnswered = AttempDetails.DailyQuizAttemptDetails.Where(m => m.DailyQuizSubjectId == item && m.AnswerStatus == ExamLogics.AnswerStatus.NOTANSWERED.ToString()).Count();
                    subjectWiseScoreVM.NumberNotVisited = AttempDetails.DailyQuizAttemptDetails.Where(m => m.DailyQuizSubjectId == item && m.AnswerStatus == ExamLogics.AnswerStatus.NOTATTEMPTED.ToString()).Count();
                    subjectWiseScoreVM.NumberReview = AttempDetails.DailyQuizAttemptDetails.Where(m => m.DailyQuizSubjectId == item && m.AnswerStatus == ExamLogics.AnswerStatus.MARKED.ToString()).Count();
                    subjectWiseScoreVM.OriginalScore = (1 * subjectWiseScoreVM.TotalQuestions);
                    subjectWiseScoreVM.YourScore = AttempDetails.DailyQuizAttemptDetails.Where(m => m.IsAnswerCorrect == true && m.DailyQuizSubjectId == item).Sum(m => m.MarksScored);
                    //decimal? Minutes = AttempDetails.DailyQuizAttemptDetails.Where(m => m.DailyQuizSubjectId == item).FirstOrDefault().SubjectTimeUsed;
                    //double MinutesNotNull = Convert.ToDouble(Minutes.HasValue ? Minutes.Value : 0);
                    //TimeSpan spWorkMin = TimeSpan.FromMinutes(MinutesNotNull);
                    //subjectWiseScoreVM.SubjectTimeSpent = string.Format("{0:00} Hours {1:00} Minutes {2:00} Seconds", (int)spWorkMin.TotalHours, spWorkMin.Minutes, spWorkMin.Seconds);
                    subjectWiseScoreVM.Accuracy = (subjectWiseScoreVM.YourScore / subjectWiseScoreVM.OriginalScore.Value) * 100;
                    decimal? Mnutes = (TestDetails.TimeInMinutes.HasValue ? TestDetails.TimeInMinutes.Value : 0) - AttempDetails.TimeLeftInMinutes;
                    double Mnutesnotnl = Convert.ToDouble(Mnutes.HasValue ? Mnutes.Value : 0);
                    TimeSpan WorkMin = TimeSpan.FromMinutes(Mnutesnotnl);
                    subjectWiseScoreVM.SubjectTimeSpent = string.Format("{0:00} Hours {1:00} Minutes {2:00} Seconds", (int)WorkMin.TotalHours, WorkMin.Minutes, WorkMin.Seconds);

                    subjectWiseScoreVMs.Add(subjectWiseScoreVM);
                }

            }

            model.subjectWiseScoreVMs = subjectWiseScoreVMs;
            model.TotalNumberQuestions = subjectWiseScoreVMs.Sum(m => m.TotalQuestions);
            model.TotalAnswered = subjectWiseScoreVMs.Sum(m => m.NumberAnswered);

            decimal? Minutes = (TestDetails.TimeInMinutes.HasValue ? TestDetails.TimeInMinutes.Value : 0) - AttempDetails.TimeLeftInMinutes;
            double MinutesNotNull = Convert.ToDouble(Minutes.HasValue ? Minutes.Value : 0);
            TimeSpan spWorkMin = TimeSpan.FromMinutes(MinutesNotNull);
            model.TotalTimeTaken = string.Format("{0:00} Hours {1:00} Minutes {2:00} Seconds", (int)spWorkMin.TotalHours, spWorkMin.Minutes, spWorkMin.Seconds);
            model.topPerformersVMs = GetTopPerformers(AttempDetails);
            model.TotalOriginalMarks = subjectWiseScoreVMs.Sum(m => m.OriginalScore);
            model.TotalTestAccuracy = (model.TotalMarksScored / model.TotalOriginalMarks.Value) * 100;


            return View(model);
        }

        private List<TopPerformersVM> GetTopPerformers(DailyQuizAttempt attempDetails)
        {
            List<TopPerformersVM> topPerformersVMs = new List<TopPerformersVM>();
            var AttempstOftheTest = DailyQuizService.GetDailyQuizAttemptList(attempDetails?.DailyQuizId);
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

        private int GetRankOftheStudent(DailyQuizAttempt attempDetails)
        {
            int rank = 0;
            // Rank need to be calculated
            if (attempDetails != null)
            {
                var AttempstOftheTest = DailyQuizService.GetDailyQuizAttemptList(attempDetails?.DailyQuizId);
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

        [HttpPost]
        public ActionResult Solution()
        {
            return View();
        }
    }
}