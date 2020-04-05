using OnlineMasterG.Base;
using OnlineMasterG.Code;
using OnlineMasterG.CommonServices;
using OnlineMasterG.DomainLogic;
using OnlineMasterG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMasterG.Controllers
{
    [ActionAuthorization()]
    public class DailyQuizUploadController : BaseController
    {
        // GET: DailyQuizUpload
        public ActionResult Index()
        {
            ViewBag.CourseList = new SelectList(DailyQuizService.DailyQuizCourseList("en-US", true), "DailyQuizCourseId", "DailyQuizCourseName");
            return View();
        }
        [HttpGet]
        public PartialViewResult QuizUploadList()
        {
            var model = DailyQuizLogics.DailyQuizUploadList("en-US", true);

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult SaveQuizUpload(DailyQuizUploadVM model)
        {
            Stream fs = model.postedFile.InputStream;
            BinaryReader br = new BinaryReader(fs);
            byte[] bytes = br.ReadBytes((Int32)fs.Length);
            // Validate & Save
            var sr = new ServiceResponse();
            sr = DailyQuizLogics.ValidateQuizUpload(model);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = DailyQuizLogics.SaveQuizUpload(model, bytes, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Quiz Upload has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteQuizUpload(int QuizUploadId)
        {
            var sr = DailyQuizService.DeleteQuizUpload(QuizUploadId);

            return GetJsonValidation(sr, "Quiz Upload has been successfully deleted.");
        }
        public FileResult DownloadSample()
        {

            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(@"~/Content/Sample/QuestionFormat_Sample.xlsx"));
            string fileName = "SampleQuizUpload.xlsx";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        [HttpPost]
        public JsonResult BindSubjects(int CourelistId)
        {
            var Categories = DailyQuizService.DailyQuizSubjectList("en-US", true).Where(m => m.DailyQuizCourseId == CourelistId).Select(m => new { SubjectId = m.DailyQuizSubjectId, SubjectName = m.DailyQuizSubjectName }).ToList();

            return GetJsonResult(Categories);
        }
        [HttpPost]
        public JsonResult BindQuiz(int SubjectListId)
        {
            var Categories = DailyQuizService.DailyQuizList("en-US", true).Where(m => m.DailyQuizSubjectId == SubjectListId).Select(m => new { QuizId = m.DailyQuizId, QuizName = m.DailyQuizName }).ToList();

            return GetJsonResult(Categories);
        }
        [HttpPost]
        public JsonResult ApproveQuizUpload(int dailyQuizUploadId)
        {
            var sr = DailyQuizService.ApproveQuizUpload(dailyQuizUploadId);

            return GetJsonValidation(sr, "Quiz Upload has been successfully approved.");
        }
        [HttpPost]
        public JsonResult DenyQuizUpload(int dailyQuizUploadId)
        {
            var sr = DailyQuizService.DenyQuizUpload(dailyQuizUploadId);

            return GetJsonValidation(sr, "Quiz Upload has been successfully denied.");
        }

        public ActionResult EditQuizQuestions(int id)
        {
            QuestionReviewVM questionReviewVM = new QuestionReviewVM();
            var QuestionUpload = DailyQuizService.FetchDailyQuizUpload(id);
            if (QuestionUpload != null)
            {
                questionReviewVM.CourseName = QuestionUpload.DailyQuizCourse?.DailyQuizCourseName;
                questionReviewVM.TestName = QuestionUpload.DailyQuiz?.DailyQuizName;
                questionReviewVM.SubjectName = QuestionUpload.DailyQuizSubject?.DailyQuizSubjectName;

                if (QuestionUpload.QuizTests != null && QuestionUpload.QuizTests.Count() > 0)
                {
                    foreach (var item in QuestionUpload.QuizTests)
                    {
                        List<QuestionAnswerChoiceVM> questionAnswerChoiceVMs = new List<QuestionAnswerChoiceVM>();

                        foreach (var qc in item.QuizQuestionAnswerChoices)
                        {
                            questionAnswerChoiceVMs.Add(new QuestionAnswerChoiceVM()
                            {
                                QuestionAnswerChoiceId = qc.QuizQuestionAnswerChoiceId,
                                QuestionAnswer = qc.QuizQuestionAnswer,
                                QuestionsMockTestId = qc.QuizTestId,
                                IsCorrect = qc.IsCorrect,
                                ChoiceId = qc.ChoiceId
                            });
                        }

                        List<QuestionPointVM> questionPointVMs = new List<QuestionPointVM>();

                        foreach (var qp in item.QuizQuestionPoints)
                        {
                            questionPointVMs.Add(new QuestionPointVM()
                            {
                                QPoint = qp.QuizQPoint,
                                QuestionPointId = qp.QuizQuestionPointId,
                                QuestionsMockTestId = qp.QuizTestId

                            });
                        }
                        var CorrectAnswer = questionAnswerChoiceVMs.Where(m => m.IsCorrect == true).FirstOrDefault();
                        var OptionList = questionAnswerChoiceVMs.Select(m => m.ChoiceId).ToList();
                        int? CorrectAnswerId = null;
                        if (CorrectAnswer != null)
                        {
                            CorrectAnswerId = CorrectAnswer.ChoiceId;
                        }
                        questionReviewVM.QuestionsMockTests.Add(new QuestionsMockTestVM
                        {
                            QuestionsMockTestId = item.QuizTestId,
                            Question = item.Question,
                            QuestionNumber = item.QuestionNumber,
                            Description = item.Description,
                            QuestionSet = item.QuestionSet,
                            QuestionImageFileId = item.QuizImageFileId,
                            QuestionAnswerChoices = questionAnswerChoiceVMs,
                            QuestionPoints = questionPointVMs,
                            Solution = item.Solution,
                            QuestionUploadId = item.DailyQuizUploadId,
                            CorrectAnswer = CorrectAnswerId != null ? CorrectAnswerId.ToString() : "1",
                            OptionList = OptionList
                        });
                    }
                }

            }

            return View(questionReviewVM);
        }

        [HttpPost]
        public ActionResult SaveReviewedQuestions(QuestionReviewEdit model)
        {
            var sr = DailyQuizLogics.SaveEditQuestions(model, HttpContext.User.Identity.Name);

            return GetJsonValidation(sr, "Quiz has been edited successfully.");
        }
    }
}