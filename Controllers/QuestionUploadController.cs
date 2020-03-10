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
        //[HttpPost]
        //public JsonResult DeleteQuestionUpload(int TestId)
        //{
        //    var sr = QuestionUploadService.DeleteQuestionUpload(TestId);

        //    return GetJsonValidation(sr, "Test has been successfully deleted.");
        //}
    }
}