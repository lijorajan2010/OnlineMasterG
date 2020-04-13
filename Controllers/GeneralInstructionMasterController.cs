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
    [ActionAuthorization("PUBLICACTION")]
    public class GeneralInstructionMasterController : BaseController
    {
        // GET: GeneralInstructionMaster
        public ActionResult Index()
        {
            ViewBag.TestList = new SelectList(TestService.TestList("en-US", true), "TestId", "TestName");
            return View();
        }
        [HttpGet]
        public PartialViewResult InstructionList()
        {
            var model = GeneralLogics.GeneralInstructionList("en-US", true);
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult SaveInstruction(List<GenneralInstructionVM> setupMarks)
        {

            // Validate & Save
            var sr = new ServiceResponse();
            sr = GeneralLogics.ValidateGeneralInstruction(setupMarks);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = GeneralLogics.SaveGeneralInstructions(setupMarks, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Instruction has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteInstruction(int instructionId)
        {
            var sr = GeneralService.DeleteGeneralInstruction(instructionId);

            return GetJsonValidation(sr, "Instruction has been successfully deleted.");
        }

        [HttpPost]
        public PartialViewResult LoadInstructionList(int testId)
        {
            var model = GeneralLogics.LoadGeneralInstructionVM(testId);

          return PartialView(model);
        }
    }
}