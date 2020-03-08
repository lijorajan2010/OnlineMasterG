using OnlineMasterG.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMasterG.Controllers
{
    public class QuestionMasterController : BaseController
    {
        // GET: QuestionMaster
        public ActionResult Index()
        {
            return View();
        }
    }
}