﻿using OnlineMasterG.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMasterG.Controllers
{
    [AllowAnonymous]
    public class CollegesController : Controller
    {
        // GET: Colleges
        public ActionResult Index()
        {
            return View();
        }
    }
}