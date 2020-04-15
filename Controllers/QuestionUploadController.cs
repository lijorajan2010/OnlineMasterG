using OnlineMasterG.Base;
using OnlineMasterG.Code;
using OnlineMasterG.CommonServices;
using OnlineMasterG.DomainLogic;
using OnlineMasterG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace OnlineMasterG.Controllers
{
    [ActionAuthorization("ADMINACTION")]
    public class QuestionUploadController : BaseController
    {
        // GET: UploadMaster
        public ActionResult Index()
        {
            ViewBag.CourseList = new SelectList(CourseService.CourseList("en-US", true), "CourseId", "CourseName");
            //ViewBag.CategoryList = new SelectList(CategoryService.CategoryList("en-US", true), "CategoryId", "CategoryName");
            //ViewBag.SectionList = new SelectList(SectionService.SectionList("en-US", true), "SectionId", "SectionName");
            //ViewBag.TestList = new SelectList(TestService.TestList("en-US", true), "TestId", "TestName");
            //ViewBag.SubjectList = new SelectList(SubjectService.SubjectList("en-US", true), "SubjectId", "SubjectName");
            return View();
        }
        [HttpGet]
        public PartialViewResult QuestionUploadList()
        {
            var model = QuestionUploadLogics.QuestionUploadList("en-US", true).OrderByDescending(m=>m.CreateOn).ToList();

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult SaveQuestionUpload(QuestionUploadVM model)
        {
            Stream fs = model.postedFile.InputStream;
            BinaryReader br = new BinaryReader(fs);
            byte [] bytes = br.ReadBytes((Int32)fs.Length);
            // Validate & Save
            var sr = new ServiceResponse();
            sr = QuestionUploadLogics.ValidateQuestionUpload(model);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = QuestionUploadLogics.SaveQuestionUpload(model, bytes, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Question Upload has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteQuestionUpload(int QuestionUploadId)
        {
            var sr = QuestionUploadService.DeleteQuestionUpload(QuestionUploadId);

            return GetJsonValidation(sr, "Question Upload has been successfully deleted.");
        }
        [HttpPost]
        public JsonResult ApproveQuestionUpload(int QuestionUploadId)
        {
            var sr = QuestionUploadService.ApproveQuestionUpload(QuestionUploadId);

            return GetJsonValidation(sr, "Question Upload has been successfully approved.");
        }
        [HttpPost]
        public JsonResult DenyQuestionUpload(int QuestionUploadId)
        {
            var sr = QuestionUploadService.DenyQuestionUpload(QuestionUploadId);

            return GetJsonValidation(sr, "Question Upload has been successfully denied.");
        }

        public ActionResult EditQuestions(int id)
        {
            QuestionReviewVM questionReviewVM = new QuestionReviewVM();
            var QuestionUpload = QuestionUploadService.Fetch(id);
            if (QuestionUpload!=null)
            {
                questionReviewVM.CourseName = CourseService.Fetch(QuestionUpload.CourseId)?.CourseName;
                questionReviewVM.CategoryName = CategoryService.Fetch(QuestionUpload.CategoryId)?.CategoryName;
                questionReviewVM.SectionName = SectionService.Fetch(QuestionUpload.SectionId)?.SectionName;
                questionReviewVM.TestName = TestService.Fetch(QuestionUpload.TestId)?.TestName;
                questionReviewVM.SubjectName = SubjectService.Fetch(QuestionUpload.SubjectId)?.SubjectName;

                if (QuestionUpload.QuestionsMockTests !=null && QuestionUpload.QuestionsMockTests.Count()>0)
                {
                    foreach (var item in QuestionUpload.QuestionsMockTests)
                    {
                        List<QuestionAnswerChoiceVM> questionAnswerChoiceVMs = new List<QuestionAnswerChoiceVM>();

                        foreach (var qc in item.QuestionAnswerChoices)
                        {
                            questionAnswerChoiceVMs.Add(new QuestionAnswerChoiceVM()
                            {
                                QuestionAnswerChoiceId = qc.QuestionAnswerChoiceId,
                                QuestionAnswer = qc.QuestionAnswer,
                                QuestionsMockTestId = qc.QuestionsMockTestId,
                                IsCorrect = qc.IsCorrect,
                                ChoiceId = qc.ChoiceId
                            });
                        }

                        List<QuestionPointVM> questionPointVMs = new List<QuestionPointVM>();

                        foreach (var qp in item.QuestionPoints)
                        {
                            questionPointVMs.Add(new QuestionPointVM()
                            {
                                QPoint = qp.QPoint,
                                QuestionPointId = qp.QuestionPointId,
                                QuestionsMockTestId = qp.QuestionsMockTestId

                            });
                        }
                        var CorrectAnswer = questionAnswerChoiceVMs.Where(m => m.IsCorrect == true).FirstOrDefault();
                        var OptionList = questionAnswerChoiceVMs.Select(m => m.ChoiceId).ToList();
                        int? CorrectAnswerId = null;
                        if (CorrectAnswer!=null)
                        {
                            CorrectAnswerId = CorrectAnswer.ChoiceId;
                        }
                        questionReviewVM.QuestionsMockTests.Add(new QuestionsMockTestVM
                        {
                            QuestionsMockTestId = item.QuestionsMockTestId,
                            Question = item.Question,
                            QuestionNumber = item.QuestionNumber,
                            Description = item.Description,
                            QuestionSet = item.QuestionSet,
                            QuestionImageFileId = item.QuestionImageFileId,
                            QuestionAnswerChoices = questionAnswerChoiceVMs,
                            QuestionPoints = questionPointVMs,
                            Solution = item.Solution,
                            QuestionUploadId = item.QuestionUploadId,
                            CorrectAnswer = CorrectAnswerId!=null ? CorrectAnswerId.ToString() : "1",
                            OptionList = OptionList,
                            Direction = item.Direction
                        }); 
                    }
                }

            }

            return View(questionReviewVM);
        }

        [HttpPost]
        public ActionResult SaveReviewedQuestions(QuestionReviewEdit model)
        {
            var sr = QuestionUploadLogics.SaveEditQuestions(model,HttpContext.User.Identity.Name);

            return GetJsonValidation(sr, "Questions has been edited successfully.");
        }

        [HttpPost]
        public JsonResult BindCategory(int courseId)
        {
            var Categories = CategoryService.CategoryList("en-US", true).Where(m => m.CourseId == courseId).Select(m => new { CategoryId = m.CategoryId, CategoryName = m.CategoryName }).ToList();

            return GetJsonResult(Categories);
        }
        [HttpPost]
        public JsonResult BindSections(int categoryId)
        {
            var Sections = SectionService.SectionList("en-US", true).Where(m => m.CategoryId == categoryId).Select(m => new { SectionId = m.SectionId, SectionName = m.SectionName }).ToList();

            return GetJsonResult(Sections);
        }
        [HttpPost]
        public JsonResult BindTests(int sectionId)
        {
            var Tests = TestService.TestList("en-US", true).Where(m => m.SectionId == sectionId).Select(m => new { TestId = m.TestId, TestName = m.TestName }).ToList();

            return GetJsonResult(Tests);
        }
        [HttpPost]
        public JsonResult BindSubjects(int testId)
        {
            var Subjects = SubjectService.SubjectList("en-US", true).Where(m => m.TestId == testId).Select(m => new { SubjectId = m.SubjectId, SubjectName = m.SubjectName }).ToList();

            return GetJsonResult(Subjects);
        }

        public FileResult DownloadSample()
        {
           
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(@"~/Content/Sample/QuestionFormat_Sample.xlsx"));
            string fileName = "SampleQuestionUpload.xlsx";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}