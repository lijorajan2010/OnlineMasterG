using OnlineMasterG.Base;
using OnlineMasterG.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMasterG.Controllers
{
    [ActionAuthorization()]
    public class MyAccountController : BaseController
    {
        // GET: MyAccount
        public ActionResult Index()
        {
            return View();
        }
    }
}