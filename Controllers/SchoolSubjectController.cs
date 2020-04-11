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
    public class SchoolSubjectController : BaseController
    {
        // GET: SchoolSubject
        public ActionResult Index()
        {
            ViewBag.ClassList = new SelectList(ClassService.SchoolClassList("en-US", true), "ClassId", "ClassName");
            return View();
        }
        [HttpGet]
        public PartialViewResult SchoolSubjectList()
        {
            var model = SubjectLogics.SchoolSubjectList("en-US");

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveSchoolSubject(SubjectVM model)
        {

            // Validate & Save
            var sr = new ServiceResponse();
            sr = SubjectLogics.ValidateSubject(model);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = SubjectLogics.SaveSchoolSubject(model, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Subject has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteSchoolSubject(int SubjectId)
        {
            var sr = SubjectService.DeleteSchoolSubject(SubjectId);

            return GetJsonValidation(sr, "Subject has been successfully deleted.");
        }
    }
}