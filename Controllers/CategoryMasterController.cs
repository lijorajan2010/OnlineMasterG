﻿using OnlineMasterG.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMasterG.Controllers
{
    public class CategoryMasterController : BaseController
    {
        // GET: CategoryMaster
        public ActionResult Index()
        {
            return View();
        }
    }
}