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
    public class SchoolUploadPaperController : BaseController
    {
        // GET: SchoolUploadQuestion
        public ActionResult Index()
        {
            ViewBag.ClassList = new SelectList(ClassService.SchoolClassList("en-US", true), "ClassId", "ClassName");
            //ViewBag.SubjectList = new SelectList(SubjectService.SchoolSubjectList("en-US", true), "SubjectId", "SubjectName");
            //ViewBag.SectionList = new SelectList(SectionService.SchoolSectionList("en-US", true), "SectionId", "SectionName");
            return View();
        }
        [HttpGet]
        public PartialViewResult SchoolUploadPaperList()
        {
            var model = PaperLogics.SchoolPaperList("en-US", true);

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveSchoolUploadPaper(PaperVM model)
        {

            // Validate & Save
            var sr = new ServiceResponse();
            sr = PaperLogics.ValidatePaper(model);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = PaperLogics.SaveSchoolPaper(model, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Paper has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteSchoolPaper(int PaperId)
        {
            var sr = PaperService.DeleteSchoolPaper(PaperId);

            return GetJsonValidation(sr, "Paper has been successfully deleted.");
        }
        [HttpPost]
        public JsonResult BindSubjects(int classId)
        {
            var subjects = SubjectService.SchoolSubjectList("en-US", true).Where(m => m.ClassId == classId).Select(m => new { SubjectId = m.SubjectId, SubjectName = m.SubjectName }).ToList();

            return GetJsonResult(subjects);
        }
        [HttpPost]
        public JsonResult BindSections(int subjectId)
        {
            var Sections = SectionService.SchoolSectionList("en-US", true).Where(m => m.SubjectId == subjectId).Select(m => new { SectionId = m.SectionId, SectionName = m.SectionName }).ToList();

            return GetJsonResult(Sections);
        }

    }
}