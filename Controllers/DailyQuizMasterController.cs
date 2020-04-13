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
    public class DailyQuizMasterController : BaseController
    {
        // GET: DailyQuiz
        public ActionResult Index()
        {
            ViewBag.CourseList = new SelectList(DailyQuizService.DailyQuizCourseList("en-US", true), "DailyQuizCourseId", "DailyQuizCourseName");
            return View();
        }
        [HttpGet]
        public PartialViewResult DailyQuizList()
        {
            var model = DailyQuizLogics.DailyQuizList("en-US", true);

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveDailyQuiz(DailyQuizVM model)
        {

            // Validate & Save
            var sr = new ServiceResponse();
            sr = DailyQuizLogics.ValidateDailyQuiz(model);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = DailyQuizLogics.SaveDailyQuiz(model, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Daily quiz has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteDailyQuiz(int DailyQuizId)
        {
            var sr = DailyQuizService.DeleteDailyQuiz(DailyQuizId);

            return GetJsonValidation(sr, "Section has been successfully deleted.");
        }
        [HttpPost]
        public JsonResult BindSubjects(int CourelistId)
        {
            var Categories = DailyQuizService.DailyQuizSubjectList("en-US", true).Where(m => m.DailyQuizCourseId == CourelistId).Select(m => new { SubjectId = m.DailyQuizSubjectId, SubjectName = m.DailyQuizSubjectName }).ToList();

            return GetJsonResult(Categories);
        }
    }
}