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
    public class CourseMasterController : BaseController
    {
        // GET: CourseMaster
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public PartialViewResult CourseList()
        {
            var model = CourseService.CourseList("en-US",true);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveCourse(CourseVM model)
        {

            // Validate & Save
            var sr = new ServiceResponse();
            sr = CourseLogics.ValidateCourse(model);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = CourseLogics.SaveCourse(model,HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Course has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteCourse(int coursetId)
        {
            var sr = CourseService.DeleteCourse(coursetId);

            return GetJsonValidation(sr, "Course has been successfully deleted.");
        }

    }
}