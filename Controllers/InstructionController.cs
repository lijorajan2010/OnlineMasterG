using OnlineMasterG.Base;
using OnlineMasterG.Code;
using OnlineMasterG.CommonFramework;
using OnlineMasterG.CommonServices;
using OnlineMasterG.DomainLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMasterG.Controllers
{
    [ActionAuthorization("PUBLICACTION")]
    public class InstructionController : BaseController
    {
        // GET: Instruction
        public ActionResult Index(string p, string r, string s)
        {
            int TestId = 0;
            bool isReattempt = false;
            int ResumeAttemptId = 0;
            if (!string.IsNullOrEmpty(p))
            {
                TestId = int.Parse(CustomEncrypt.SafeUrlDecrypt(p));
            }
            if (!string.IsNullOrEmpty(r))
            {
                isReattempt = Convert.ToBoolean(CustomEncrypt.SafeUrlDecrypt(r));
            }
            if (!string.IsNullOrEmpty(s))
            {
                ResumeAttemptId = int.Parse(CustomEncrypt.SafeUrlDecrypt(s));
            }

            ViewBag.TestId = TestId;
            ViewBag.IsReattempt = isReattempt;
            ViewBag.ResumeAttemptId = ResumeAttemptId;
            return View();
        }
        [HttpPost]
        public PartialViewResult LoadInstructionList(int testId)
        {
            var model = GeneralLogics.LoadDBGeneralInstruction("en-US",true,testId).ToList();

            return PartialView(model);
        }

        public ActionResult DailyQuiz(string p, string r, string s)
        {
            int DailyQuizId = 0;
            bool isReattempt = false;
            int ResumeAttemptId = 0;
            if (!string.IsNullOrEmpty(p))
            {
                DailyQuizId = int.Parse(CustomEncrypt.SafeUrlDecrypt(p));
            }
            if (!string.IsNullOrEmpty(r))
            {
                isReattempt = Convert.ToBoolean(CustomEncrypt.SafeUrlDecrypt(r));
            }
            if (!string.IsNullOrEmpty(s))
            {
                ResumeAttemptId = int.Parse(CustomEncrypt.SafeUrlDecrypt(s));
            }
            ViewBag.DailyQuizId = DailyQuizId;
            ViewBag.IsReattempt = isReattempt;
            ViewBag.ResumeAttemptId = ResumeAttemptId;

            var model = DailyQuizService.FetchDailyQuiz(DailyQuizId);
            return View(model);
        }
    }
}