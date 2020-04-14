using OnlineMasterG.Base;
using OnlineMasterG.Code;
using OnlineMasterG.CommonServices;
using OnlineMasterG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMasterG.Controllers
{
    [ActionAuthorization("ADMINACTION")]
    public class DashboardController : BaseController
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult getTestimonailasList()
        {
            List<TestimonialsVM> _getTestimonials = new List<TestimonialsVM>();
            _getTestimonials = GeneralService.TestimonialsVMList().Distinct().ToList();

            return PartialView(_getTestimonials);
        }

        [HttpPost]
        public JsonResult ApproveReview(int AttemptId)
        {
            var sr = ExamService.ApproveRating(AttemptId);

            return GetJsonValidation(sr, "Review has been successfully approved.");
        }
        [HttpPost]
        public JsonResult DenyReview(int AttemptId)
        {
            var sr = ExamService.DenyRating(AttemptId);

            return GetJsonValidation(sr, "Review has been successfully denied.");
        }

    }
}