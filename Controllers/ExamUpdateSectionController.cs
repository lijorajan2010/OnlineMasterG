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
    public class ExamUpdateSectionController : BaseController
    {
        // GET: ExamUpdateSection
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public PartialViewResult ExamSectionList()
        {
            var model = ExamUpdateService.ExamAllSectionList("en-US").OrderByDescending(m=>m.CreateOn);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveSection(ExamUpdateSectionVM model)
        {

            // Validate & Save
            var sr = new ServiceResponse();
            sr = ExamUpdateLogics.ValidateSection(model);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = ExamUpdateLogics.SaveExamUpdateSection(model, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Section has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteExamUpdateSection(int SectionId)
        {
            var sr = ExamUpdateService.DeleteExamSection(SectionId);

            return GetJsonValidation(sr, "Section has been successfully deleted.");
        }

    }
}