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
    public class DailyQuizSubjectMasterController : BaseController
    {
        // GET: DailyQuizSubjectMaster
        public ActionResult Index()
        {
            ViewBag.CourseList = new SelectList(DailyQuizService.DailyQuizCourseList("en-US", true), "DailyQuizCourseId", "DailyQuizCourseName");
            return View();
        }
        [HttpGet]
        public PartialViewResult SubjectList()
        {
            var model = DailyQuizLogics.SubjectList("en-US").OrderByDescending(m=>m.CreateOn).ToList();

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveSubject(DailyQuizSubjectVM model)
        {

            // Validate & Save
            var sr = new ServiceResponse();
            sr = DailyQuizLogics.ValidateDailyQuizSubject(model);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = DailyQuizLogics.SaveDailyQuizSubject(model, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Subject has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteSubject(int SubjectId)
        {
            var sr = DailyQuizService.DeleteDailyQuizSubject(SubjectId);

            return GetJsonValidation(sr, "Subject has been successfully deleted.");
        }
    }
}