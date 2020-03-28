using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineMasterG.Base;
using OnlineMasterG.CommonServices;
using OnlineMasterG.DomainLogic;
using OnlineMasterG.Models.ViewModels;

namespace OnlineMasterG.Controllers
{
    public class ExaminationController : BaseController
    {
        // GET: Examination
        public ActionResult Index(int TestId)
        {
            var TestDetails = TestService.Fetch(TestId);

            string CurrentLogin = HttpContext.User.Identity.Name;
            MockTestAttemptVM model = new MockTestAttemptVM();
            if (!string.IsNullOrEmpty(CurrentLogin) && TestDetails != null)
            {
                model = ExamLogics.GetMockTestAttemptDetails(CurrentLogin, TestId, CurrentLogin);
              //  ViewBag.CurrentAttempt = model;
            }
            else
            {
                RedirectToAction("Index", "Login");
            }

            //if (TestDetails!=null)
            //{
            //    //ViewBag.ExamTimeInSeconds = model.TimeLeftInMinutes * 60;
            //    //ViewBag.ExamTimeInMinutes = model.TimeLeftInMinutes;
            //    //ViewBag.TestName = TestDetails.TestName;
            //    //ViewBag.TestId = TestDetails.TestId;
            //    //ViewBag.ProblemsList = model?.MockTestAttemptDetails; 
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            ViewBag.ProblemsList = ExamService.GetProblemMasters();
            ViewBag.AnswerStatus = ExamLogics.getAnswerStatuses();
            return View(model);
        }
    }
}