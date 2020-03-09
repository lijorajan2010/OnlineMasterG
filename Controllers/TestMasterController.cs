using OnlineMasterG.Base;
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
    public class TestMasterController : BaseController
    {
        // GET: TestMaster
        public ActionResult Index()
        {
            ViewBag.CourseList = new SelectList(CourseService.CourseList("en-US", true), "CourseId", "CourseName");
            ViewBag.CategoryList = new SelectList(CategoryService.CategoryList("en-US", true), "CategoryId", "CategoryName");
            ViewBag.SectionList = new SelectList(SectionService.SectionList("en-US", true), "SectionId", "SectionName");
            return View();
        }
        [HttpGet]
        public PartialViewResult TestList()
        {
            var model = TestLogics.TestList("en-US", true);

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
    }
}