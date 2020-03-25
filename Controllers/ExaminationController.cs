using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineMasterG.Base;
using OnlineMasterG.CommonServices;

namespace OnlineMasterG.Controllers
{
    public class ExaminationController : BaseController
    {
        // GET: Examination
        public ActionResult Index(int TestId)
        {
            var TestDetails = TestService.Fetch(TestId);
            if (TestDetails!=null)
            {
                ViewBag.ExamTimeInSeconds = TestDetails.TimeInMinutes * 60;
                ViewBag.ExamTimeInMinutes = TestDetails.TimeInMinutes;
                ViewBag.TestName = TestDetails.TestName;
                ViewBag.TestId = TestDetails.TestId;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           
        }
    }
}