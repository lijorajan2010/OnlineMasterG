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
    public class DailyQuizCourseMasterController : BaseController
    {
        // GET: DailyQuizCourseMaster
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public PartialViewResult DailyQuizCourseList()
        {
            var model = DailyQuizService.DailyQuizCourseList("en-US", true);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveDailyQuizCourse(DailyQuizCourseVM model)
        {
            // Validate & Save
            ServiceResponse sr = new ServiceResponse();
            sr = DailyQuizLogics.ValidateDailyQuizCourse(model);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = DailyQuizLogics.SaveDailyQuizCourse(model, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Course has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteDailyQuizCourse(int courseId)
        {
            var sr = DailyQuizService.DeleteDailyQuizCourse(courseId);

            return GetJsonValidation(sr, "Course has been successfully deleted.");
        }

    }
}