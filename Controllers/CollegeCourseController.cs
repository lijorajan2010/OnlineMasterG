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
    public class CollegeCourseController : BaseController
    {
        // GET: CollegeCourse
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public PartialViewResult CollegeCourseList()
        {
            var model = CourseService.CollegeAllCourseList("en-US");
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveCollegeCourse(CourseVM model)
        {

            // Validate & Save
            var sr = new ServiceResponse();
            sr = CourseLogics.ValidateCourse(model);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = CourseLogics.SaveCollegeCourse(model, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Course has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteCollegeCourse(int coursetId)
        {
            var sr = CourseService.DeleteCollegeCourse(coursetId);

            return GetJsonValidation(sr, "Course has been successfully deleted.");
        }
    }
}