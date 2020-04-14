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
    public class CurrentAffairsUploadController : BaseController
    {
        // GET: CurrentAffairs
        public ActionResult Index()
        {
            ViewBag.CategoryList = new SelectList(CurrentAffairsService.CurrentAffairsCategoryList("en-US", true), "CurrentAffairsCategoryId", "CurrentAffairsCategoryName");
            return View();
        }
        [HttpGet]
        public PartialViewResult CurrentAffairsUploadList()
        {
            var model = CurrentAffairsLogics.CurrentAffairsUploadList("en-US", true).OrderByDescending(m=>m.CreateOn);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveCurrentAffairsUpload(CurrentAffairsUploadVM model)
        {
            // Validate & Save
            ServiceResponse sr = new ServiceResponse();
            sr = CurrentAffairsLogics.ValidateCurrentAffairsUpload(model);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = CurrentAffairsLogics.SaveCurrentAffairsUpload(model, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Current Affairs has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteCurrentAffairsUpload(int uploadId)
        {
            var sr = CurrentAffairsService.DeleteCurrentAffairsUpload(uploadId);

            return GetJsonValidation(sr, "Current Affairs has been successfully deleted.");
        }
    }
}