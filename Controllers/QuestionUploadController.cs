using OnlineMasterG.Base;
using OnlineMasterG.CommonServices;
using OnlineMasterG.DomainLogic;
using OnlineMasterG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMasterG.Controllers
{
    public class QuestionUploadController : BaseController
    {
        // GET: UploadMaster
        public ActionResult Index()
        {
            ViewBag.CourseList = new SelectList(CourseService.CourseList("en-US", true), "CourseId", "CourseName");
            ViewBag.CategoryList = new SelectList(CategoryService.CategoryList("en-US", true), "CategoryId", "CategoryName");
            ViewBag.SectionList = new SelectList(SectionService.SectionList("en-US", true), "SectionId", "SectionName");
            ViewBag.TestList = new SelectList(TestService.TestList("en-US", true), "TestId", "TestName");
            ViewBag.SubjectList = new SelectList(SubjectService.SubjectList("en-US", true), "SubjectId", "SubjectName");
            return View();
        }
        [HttpGet]
        public PartialViewResult QuestionUploadList()
        {
            var model = QuestionUploadLogics.QuestionUploadList("en-US", true);

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult SaveQuestionUpload(QuestionUploadVM model)
        {
            System.IO.Stream fs = model.postedFile.InputStream;
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
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
                questionReviewVM.CourseName = QuestionUpload.Course?.CourseName;
                questionReviewVM.CategoryName = QuestionUpload.Category?.CategoryName;
                questionReviewVM.SectionName = QuestionUpload.Section?.SectionName;
                questionReviewVM.TestName = QuestionUpload.MockTest?.TestName;
                questionReviewVM.SubjectName = QuestionUpload.Subject?.SubjectName;

                if (QuestionUpload.QuestionsMockTests !=null && QuestionUpload.QuestionsMockTests.Count()>0)
                {
                    foreach (var item in questionReviewVM.QuestionsMockTests)
                    {
                        List<QuestionAnswerChoiceVM> questionAnswerChoiceVMs = new List<QuestionAnswerChoiceVM>();

                        foreach (var qc in item.QuestionAnswerChoices)
                        {
                            questionAnswerChoiceVMs.Add(new QuestionAnswerChoiceVM()
                            {
                                QuestionAnswerChoiceId = qc.QuestionAnswerChoiceId,
                                QuestionAnswer = qc.QuestionAnswer,
                                QuestionsMockTestId = qc.QuestionsMockTestId,
                                IsCorrect = qc.IsCorrect
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
                            QuestionUploadId = item.QuestionUploadId
                        });
                    }
                }

            }

            return View(questionReviewVM);
        }


    }
}