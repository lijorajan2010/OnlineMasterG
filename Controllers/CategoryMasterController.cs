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
    public class CategoryMasterController : BaseController
    {
        // GET: CategoryMaster
        public ActionResult Index()
        {
            ViewBag.CourseList = new SelectList(CourseService.CourseList("en-US", true), "CourseId", "CourseName");
            return View();
        }
        [HttpGet]
        public PartialViewResult CategoryList()
        {
           var model = CategoryLogics.CategoryList("en-US");

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveCategory(CategoryVM model)
        {

            // Validate & Save
            var sr = new ServiceResponse();
            sr = CategoryLogics.ValidateCategory(model);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = CategoryLogics.SaveCategory(model, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Category has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteCategory(int CategoryId)
        {
            var sr = CategoryService.DeleteCategory(CategoryId);

            return GetJsonValidation(sr, "Category has been successfully deleted.");
        }
    }
}