using OnlineMasterG.Base;
using OnlineMasterG.Code;
using OnlineMasterG.CommonFramework;
using OnlineMasterG.CommonServices;
using OnlineMasterG.DomainLogic;
using OnlineMasterG.Models.DAL;
using OnlineMasterG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMasterG.Controllers
{
    [ActionAuthorization()]
    public class MyAccountController : BaseController
    {
        // GET: MyAccount
        public ActionResult Index()
        {
            MyDashboardVM model = new MyDashboardVM();
            var Login = HttpContext.User.Identity.Name;

            var MockTestAttempts = ExamService.GetAttemptListByLogin(Login);
            if (MockTestAttempts != null && MockTestAttempts.Count() > 0)
            {
                List<GivenMockTests> givenMockTests = new List<GivenMockTests>();
                foreach (var item in MockTestAttempts)
                {

                    decimal? Minutes = (TestService.Fetch(item.TestId).TimeInMinutes - item.TimeLeftInMinutes);
                    double MinutesNotNull = Convert.ToDouble(Minutes.HasValue ? Minutes.Value : 0);
                    TimeSpan spWorkMin = TimeSpan.FromMinutes(MinutesNotNull);

                    givenMockTests.Add(new GivenMockTests()
                    {
                        AttemptId = item.AttemptId,
                        TestName = TestService.Fetch(item.TestId)?.TestName,
                        TimeUsed = string.Format("{0:00} Hours {1:00} Minutes {2:00} Seconds", (int)spWorkMin.TotalHours, spWorkMin.Minutes, spWorkMin.Seconds)
                    });
                }
                model.GivenMockTests = givenMockTests;
            }
            var DailyQuizAttempts = DailyQuizService.GetDailyQuizAttemptListByLogin(Login);
            if (DailyQuizAttempts != null && DailyQuizAttempts.Count() > 0)
            {
                List<GivenQuizes> givenQuizes = new List<GivenQuizes>();
                foreach (var item in DailyQuizAttempts)
                {
                    decimal? Minutes = (DailyQuizService.FetchDailyQuiz(item.DailyQuizId).TimeInMinutes - item.TimeLeftInMinutes);
                    double MinutesNotNull = Convert.ToDouble(Minutes.HasValue ? Minutes.Value : 0);
                    TimeSpan spWorkMin = TimeSpan.FromMinutes(MinutesNotNull);

                    givenQuizes.Add(new GivenQuizes()
                    {
                        QuizName = DailyQuizService.FetchDailyQuiz(item.DailyQuizId)?.DailyQuizName,
                        TimeUsed = string.Format("{0:00} Hours {1:00} Minutes {2:00} Seconds", (int)spWorkMin.TotalHours, spWorkMin.Minutes, spWorkMin.Seconds)
                    });
                }
                model.GivenQuizes = givenQuizes;
            }
            model.Subscriptions = new List<Subscriptions>();
            var UserDetails = UserService.Fetch(Login);
            model.profile = new Profile()
            {

                FirstName = UserDetails.FirstName,
                LastName = UserDetails.LastName,
                ImgDataFileId = UserDetails.LogoDataFileId,
                Login = UserDetails.Login,
                Email = UserDetails.Login,
                FullName = UserDetails.FirstName+" "+ UserDetails.LastName

            };


            return View(model);
        }

        [HttpPost]
        public ActionResult EditProfile(LoginVM model)
        {
            model.Login = HttpContext.User.Identity.Name;
            if (model.postedFile != null && model.postedFile.ContentLength > 0)
            {
                // extract only the fielname
                DataContent dataContent = new DataContent();
                DataFile dataFile = new DataFile();

                using (MemoryStream ms = new MemoryStream())
                {
                   model.postedFile.InputStream.CopyTo(ms);
                    dataContent.RawData = ms.GetBuffer();
                }

                dataFile.FileName = Path.GetFileName(model.postedFile.FileName);
                dataFile.Extension = Path.GetExtension(model.postedFile.FileName);
                dataFile.SourceCode = "PROFILEPIC";
                dataFile.DataContent = dataContent;
                dataFile.CreateBy = HttpContext.User.Identity.Name;
                dataFile.CreateDate = DateTime.Now;

                // Add file
                DataFileService.AddDataFile(dataFile);
                model.ImgDataFileId = dataFile.DataFileId;
            }
            var User = UserService.Fetch(model.Login);
            if (User != null)
            {
                User.FirstName = model.FirstName;
                User.LastName = model.LastName;
                if (model.ImgDataFileId!=null && model.ImgDataFileId!=0)
                {
                    User.LogoDataFileId = model.ImgDataFileId;
                }
               
                UserService.UpdateUser(User);
            }
            return RedirectToAction("Index");
        }
     
    }
}