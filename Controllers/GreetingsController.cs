using OnlineMasterG.Base;
using OnlineMasterG.Code;
using OnlineMasterG.CommonServices;
using OnlineMasterG.DomainLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMasterG.Controllers
{
    [ActionAuthorization("ADMINACTION")]
    public class GreetingsController : BaseController
    {
        // GET: Greetings
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public PartialViewResult GreetingsList()
        {
            var model = GeneralService.GreetingsList("en-US", true);

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult SaveGreetings(HttpPostedFileBase postedFile)
        {
            Stream fs = postedFile.InputStream;
            BinaryReader br = new BinaryReader(fs);
            byte[] bytes = br.ReadBytes((Int32)fs.Length);
            // Validate & Save
            var sr = new ServiceResponse();
            sr = GeneralLogics.ValidateGreetings(postedFile,bytes);
            if (!sr.Status)
                return GetJsonValidation(sr);
            sr = GeneralLogics.SaveGreetings(postedFile, bytes, HttpContext.User.Identity.Name);
            if (!sr.Status)
                return GetJsonValidation(sr);

            return GetJsonValidation(sr, "Greeting has been successfully saved.");
        }
        [HttpPost]
        public JsonResult DeleteGreeting(int GreetingId)
        {
            var sr = GeneralService.DeleteGreetings(GreetingId);

            return GetJsonValidation(sr, "Greeting has been successfully deleted.");
        }
    }
}