using OnlineMasterG.Code;
using OnlineMasterG.CommonFramework;
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
    public class DailyQuizController : Controller
    {
        // GET: DailyQuiz
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DailyQuiz(string p)
        {
            int DailyQuizId = 0;
            if (!string.IsNullOrEmpty(p))
            {
                DailyQuizId = int.Parse(CustomEncrypt.SafeUrlDecrypt(p));
            }
            var DailyQuizDetails = DailyQuizService.FetchDailyQuiz(DailyQuizId);

            string CurrentLogin = HttpContext.User.Identity.Name;
            DailyQuizAttemptVM model = new DailyQuizAttemptVM();
            if (!string.IsNullOrEmpty(CurrentLogin) && DailyQuizDetails != null)
            {
                model = DailyQuizLogics.GetDailyQuizAttemptDetails(CurrentLogin, DailyQuizId, CurrentLogin);
            }
            else
            {
                RedirectToAction("Index", "Login");
            }

            ViewBag.AnswerStatus = ExamLogics.getAnswerStatuses();
            return View(model);
        }
    }
}