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
    [ActionAuthorization()]
    public class CurrentAffairsCategoryController : BaseController
    {
        // GET: CurrentAffairsCategory
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public PartialViewResult CurrentAffairsCategoryList()
        {
            var model = CurrentAffairsService.CurrentAffairsAllCategoryList("en-US");
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveCurrentAffairsCategory(CurrentAffairsVM model)
        {
            // Validate & Save
            ServiceResponse sr = new ServiceResponse();
            sr = CurrentAffairsLogics.ValidateCurrentAffairsCategory(model);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = CurrentAffairsLogics.SaveCurrentAffairsCategory(model, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Category has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteCurrentAffairsCategory(int categoryId)
        {
            var sr = CurrentAffairsService.DeleteCurrentAffairsCategory(categoryId);

            return GetJsonValidation(sr, "Category has been successfully deleted.");
        }

    }
}