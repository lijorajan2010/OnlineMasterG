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
    public class SchoolSectionController : BaseController
    {
        // GET: SchoolSection
        public ActionResult Index()
        {
            ViewBag.ClassList = new SelectList(ClassService.SchoolClassList("en-US", true), "ClassId", "ClassName");
            ViewBag.SubjectList = new SelectList(SubjectService.SchoolSubjectList("en-US", true), "SubjectId", "SubjectName");
            return View();
        }
        [HttpGet]
        public PartialViewResult SectionList()
        {
            var model = SectionLogics.SectionSchoolList("en-US", true);

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
            sr = SectionLogics.SaveSchoolSection(model, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Section has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteSection(int SectionId)
        {
            var sr = SectionService.DeleteSchoolSection(SectionId);

            return GetJsonValidation(sr, "Section has been successfully deleted.");
        }
    }
}