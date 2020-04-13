using OnlineMasterG.Base;
using OnlineMasterG.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMasterG.Controllers
{
    [AllowAnonymous]
    public class CurrentAffairsController : BaseController
    {
        // GET: CurrentAffairs
        public ActionResult Index()
        {
            return View();
        }
    }
}