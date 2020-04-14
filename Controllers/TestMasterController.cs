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
    public class TestMasterController : BaseController
    {
        // GET: TestMaster
        public ActionResult Index()
        {
            ViewBag.CourseList = new SelectList(CourseService.CourseList("en-US", true), "CourseId", "CourseName");
            //ViewBag.CategoryList = new SelectList(CategoryService.CategoryList("en-US", true), "CategoryId", "CategoryName");
            //ViewBag.SectionList = new SelectList(SectionService.SectionList("en-US", true), "SectionId", "SectionName");
            return View();
        }
        [HttpGet]
        public PartialViewResult TestList()
        {
            var model = TestLogics.TestList("en-US").OrderByDescending(m=>m.CreateOn);

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveTest(TestVM model)
        {

            // Validate & Save
            var sr = new ServiceResponse();
            sr = TestLogics.ValidateTest(model);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = TestLogics.SaveTest(model, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Test has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteTest(int TestId)
        {
            var sr = TestService.DeleteTest(TestId);

            return GetJsonValidation(sr, "Test has been successfully deleted.");
        }
        [HttpPost]
        public JsonResult BindCategory(int courseId)
        {
            var Categories = CategoryService.CategoryList("en-US", true).Where(m => m.CourseId == courseId).Select(m => new { CategoryId = m.CategoryId, CategoryName = m.CategoryName }).ToList();

            return GetJsonResult(Categories);
        }
        [HttpPost]
        public JsonResult BindSections(int categoryId)
        {
            var Sections = SectionService.SectionList("en-US", true).Where(m => m.CategoryId == categoryId).Select(m => new { SectionId = m.SectionId, SectionName = m.SectionName }).ToList();

            return GetJsonResult(Sections);
        }
    }
}