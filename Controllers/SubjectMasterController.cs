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
    public class SubjectMasterController : BaseController
    {
        // GET: SubjectMaster
        public ActionResult Index()
        {
            ViewBag.CourseList = new SelectList(CourseService.CourseList("en-US", true), "CourseId", "CourseName");
            ViewBag.CategoryList = new SelectList(CategoryService.CategoryList("en-US", true), "CategoryId", "CategoryName");
            ViewBag.SectionList = new SelectList(SectionService.SectionList("en-US", true), "SectionId", "SectionName");
            ViewBag.TestList = new SelectList(TestService.TestList("en-US", true), "TestId", "TestName");
            return View();
        }
        [HttpGet]
        public PartialViewResult SubjectList()
        {
            var model = SubjectLogics.SubjectList("en-US").OrderByDescending(m=>m.CreateOn).ToList();

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveSubject(SubjectVM model)
        {

            // Validate & Save
            var sr = new ServiceResponse();
            sr = SubjectLogics.ValidateSubject(model);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = SubjectLogics.SaveSubject(model, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Subject has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteSubject(int SubjectId)
        {
            var sr = SubjectService.DeleteSubject(SubjectId);

            return GetJsonValidation(sr, "Subject has been successfully deleted.");
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
        [HttpPost]
        public JsonResult BindTests(int sectionId)
        {
            var Tests = TestService.TestList("en-US", true).Where(m => m.SectionId == sectionId).Select(m => new { TestId = m.TestId, TestName = m.TestName }).ToList();

            return GetJsonResult(Tests);
        }
    }
}