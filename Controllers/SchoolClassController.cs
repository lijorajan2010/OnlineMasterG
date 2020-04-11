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
    public class SchoolClassController : BaseController
    {
        // GET: SchoolClass
        public ActionResult Index()
        {
            return View();
        }
          [HttpGet]
        public PartialViewResult SchoolClassList()
        {
            var model = ClassService.SchoolAllClassList("en-US");
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveSchoolClass(ClassVM model)
        {

            // Validate & Save
            var sr = new ServiceResponse();
            sr = ClassLogics.ValidateClass(model);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = ClassLogics.SaveSchoolClass(model, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Class has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteSchoolClass(int classId)
        {
            var sr = ClassService.DeleteSchoolClass(classId);

            return GetJsonValidation(sr, "Class has been successfully deleted.");
        }
    }
}