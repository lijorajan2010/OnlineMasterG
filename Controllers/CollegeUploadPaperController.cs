using OnlineMasterG.Base;
using OnlineMasterG.Code;
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
    [ActionAuthorization()]
    public class CollegeUploadPaperController : BaseController
    {
        // GET: CollegeUploadPaper
        public ActionResult Index()
        {
            ViewBag.CourseList = new SelectList(CourseService.CollegeCourseList("en-US", true), "CourseId", "CourseName");
          //  ViewBag.SubjectList = new SelectList(SubjectService.CollegeSubjectList("en-US", true), "SubjectId", "SubjectName");

            return View();
        }
        [HttpGet]
        public PartialViewResult CollegeUploadPaperList()
        {
            var model = PaperLogics.CollegePaperList("en-US", true);

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveCollegeUploadPaper(PaperVM model)
        {

            // Validate & Save
            var sr = new ServiceResponse();
            sr = PaperLogics.ValidatePaper(model);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = PaperLogics.SaveCollegePaper(model, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Paper has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteCollegeUploadPaper(int PaperId)
        {
            var sr =PaperService.DeleteCollegePaper(PaperId);

            return GetJsonValidation(sr, "Paper has been successfully deleted.");
        }

        [HttpPost]
        public JsonResult BindSubjects(int courseId)
        {
            var Sections = SubjectService.CollegeSubjectList("en-US", true).Where(m => m.CourseId == courseId).Select(m => new { SubjectId = m.SubjectId, SubjectName = m.SubjectName }).ToList();

            return GetJsonResult(Sections);
        }
    }
}