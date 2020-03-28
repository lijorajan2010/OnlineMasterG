using OnlineMasterG.Base;
using OnlineMasterG.Code;
using OnlineMasterG.CommonFramework;
using OnlineMasterG.DomainLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMasterG.Controllers
{
    [ActionAuthorization()]
    public class InstructionController : BaseController
    {
        // GET: Instruction
        public ActionResult Index(string p)
        {
            int TestId = 0;
            if (!string.IsNullOrEmpty(p))
            {
                TestId = int.Parse(CustomEncrypt.SafeUrlDecrypt(p));
            }
            ViewBag.TestId = TestId;
            return View();
        }
        [HttpPost]
        public PartialViewResult LoadInstructionList(int testId)
        {
            var model = GeneralLogics.LoadDBGeneralInstruction("en-US",true,testId);

            return PartialView(model);
        }
    }
}