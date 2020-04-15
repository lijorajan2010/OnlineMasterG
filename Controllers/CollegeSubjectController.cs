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
    [ActionAuthorization("ADMINACTION")]
    public class CollegeSubjectController : BaseController
    {
        // GET: CollegeSubject
        public ActionResult Index()
        {
            ViewBag.CourseList = new SelectList(CourseService.CollegeCourseList("en-US", true), "CourseId", "CourseName");
            return View();
        }
        [HttpGet]
        public PartialViewResult CollegeSubjectList()
        {
            var model = SubjectLogics.CollegeSubjectList("en-US").OrderByDescending(m=>m.CreateOn).ToList();

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveCollegeSubject(SubjectVM model)
        {

            // Validate & Save
            var sr = new ServiceResponse();
            sr = SubjectLogics.ValidateSubject(model);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = SubjectLogics.SaveCollegeSubject(model, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Subject has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteCollegeSubject(int SubjectId)
        {
            var sr = SubjectService.DeleteCollegeSubject(SubjectId);

            return GetJsonValidation(sr, "Subject has been successfully deleted.");
        }
    }
}