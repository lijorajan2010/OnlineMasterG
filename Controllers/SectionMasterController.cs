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
    public class SectionMasterController : BaseController
    {
        // GET: SectionMaster
        public ActionResult Index()
        {
            ViewBag.CourseList = new SelectList(CourseService.CourseList("en-US", true), "CourseId", "CourseName");
          //  ViewBag.CategoryList = new SelectList(CategoryService.CategoryList("en-US", true), "CategoryId", "CategoryName");
            return View();
        }
        [HttpGet]
        public PartialViewResult SectionList()
        {
            var model = SectionLogics.SectionList("en-US").OrderByDescending(m=>m.CreateOn);

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveSection(SectionVM model)
        {

            // Validate & Save
            var sr = new ServiceResponse();
            sr = SectionLogics.ValidateSection(model);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = SectionLogics.SaveSection(model, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Section has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteSection(int SectionId)
        {
            var sr = SectionService.DeleteSection(SectionId);

            return GetJsonValidation(sr, "Section has been successfully deleted.");
        }
        [HttpPost]
        public JsonResult BindCategory(int courseId)
        {
            var Categories = CategoryService.CategoryList("en-US", true).Where(m => m.CourseId == courseId).Select(m=>new { CategoryId = m.CategoryId, CategoryName = m.CategoryName}).ToList();
            
            return GetJsonResult(Categories);
        }
    }
}