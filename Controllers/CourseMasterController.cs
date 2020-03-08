using OnlineMasterG.Base;
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
            return PartialView();
        }

        [HttpPost]
        public JsonResult SaveCourse()
        {
            // Validate & Save
            var sr = new ServiceResponse();
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Airport has been successfully saved.");
        }
    }
}