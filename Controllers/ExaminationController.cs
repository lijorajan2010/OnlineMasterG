using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineMasterG.Base;

namespace OnlineMasterG.Controllers
{
    public class ExaminationController : BaseController
    {
        // GET: Examination
        public ActionResult Index()
        {
            return View();
        }
    }
}