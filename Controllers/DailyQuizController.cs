using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMasterG.Controllers
{
    public class DailyQuizController : Controller
    {
        // GET: DailyQuiz
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DailyQuiz()
        {
            return View();
        }
    }
}