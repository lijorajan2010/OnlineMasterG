using OnlineMasterG.Base;
using OnlineMasterG.Code;
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
    [ActionAuthorization("ADMINACTION")]
    public class LatestUpdatesController : BaseController
    {
        // GET: LatestUpdates
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public PartialViewResult LatestUpdatesList()
        {
            var model = GeneralService.LatestAllUpdatesList("en-US").OrderByDescending(m=>m.CreateOn);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveLatestUpdate(LatestUpdatesVM model)
        {

            // Validate & Save
            var sr = new ServiceResponse();
            sr = GeneralLogics.ValidateLatestUpdate(model);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = GeneralLogics.SaveLatestUpdate(model, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Latest Update has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteLatestUpdate(int updateId)
        {
            var sr = GeneralService.DeleteLatestUpdate(updateId);

            return GetJsonValidation(sr, "Latest update has been successfully deleted.");
        }
    }
}