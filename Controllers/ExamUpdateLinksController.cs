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
    public class ExamUpdateLinksController : BaseController
    {
        // GET: ExamUpdateLinks
        public ActionResult Index()
        {
            ViewBag.SectionList = new SelectList(ExamUpdateService.ExamSectionList("en-US", true), "SectionId", "SectionName");
            return View();
        }
        [HttpGet]
        public PartialViewResult ExamLinkList()
        {
            var model = ExamUpdateLogics.ExamSectionLinkList("en-US", true);

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveExamLink(ExamUpdateSectionVM model)
        {

            // Validate & Save
            var sr = new ServiceResponse();
            sr = ExamUpdateLogics.ValidateSectionLinks(model);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = ExamUpdateLogics.SaveExamUpdateSectionLink(model, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Exam Link has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteExamLink(int LinkId)
        {
            var sr = ExamUpdateService.DeleteExamSectionLink(LinkId);

            return GetJsonValidation(sr, "Exam Link has been successfully deleted.");
        }
    }
}