using OnlineMasterG.Base;
using OnlineMasterG.CommonFramework;
using OnlineMasterG.CommonServices;
using OnlineMasterG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineMasterG.Controllers
{
    public class LoginController : BaseController
    {
        // GET: Login
        public ActionResult Index()
        {
            HttpCookie cookie = Request.Cookies.Get("OnlineMasterG_Login");
            LoginVM loginModel = new LoginVM();

            // Load cookie
            if (cookie != null)
            {
                loginModel.Login = CustomEncrypt.Decrypt(cookie.Values["UserName"]);
                loginModel.Password = CustomEncrypt.Decrypt(cookie.Values["Password"]);
                loginModel.IsRememberMe = true;
            }

            loginModel.ReturnUrl = Request?.UrlReferrer?.AbsoluteUri;
          
            return View(loginModel);
        }
        [HttpPost]
        public ActionResult Index(LoginVM loginModel)
        {
            // Validate view model first
            if (!ModelState.IsValid)
                return View(loginModel);

            var userData = SecurityService.FastLogin(loginModel.Login, loginModel.Password);

            if (userData != null)
            {
                if (loginModel.IsRememberMe)
                {
                    // Save cookie
                    HttpCookie cookie = new HttpCookie("OnlineMasterG_Login");
                    cookie.Values["UserName"] = CustomEncrypt.Encrypt(loginModel.Login);
                    cookie.Values["Password"] = CustomEncrypt.Encrypt(loginModel.Password);
                    cookie.Expires = DateTime.Now.AddYears(1);
                    cookie.Secure = true;  // For HTTPS
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    // Remove login cookie
                    HttpCookie cookie = new HttpCookie("OnlineMasterG_Login");
                    cookie.Expires = DateTime.Now.AddYears(-1);
                    Response.Cookies.Add(cookie);
                }

                SessionObject.SetUserInFormsCookie(userData);

                var langCookie = new HttpCookie("lang", userData.DefaultLanguageCode) { HttpOnly = true };
                Response.AppendCookie(langCookie);
                var ci = new CultureInfo(userData.DefaultLanguageCode);
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = ci;
                var languages = LanguageService.FetchList();

                string dir = languages.Where(x => x.LanguageCode == userData.DefaultLanguageCode).FirstOrDefault().LanguageCode;
                var dirCookie = new HttpCookie("direction", dir) { HttpOnly = true };
                Response.AppendCookie(dirCookie);

                if (userData.UserTypeCode == UserService.UserTypes.ADMIN.ToString())
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                else 
                {
                    if (!string.IsNullOrEmpty(loginModel.ReturnUrl))
                    {
                        var routeValues = SessionObject._getControllerAndActionName(loginModel.ReturnUrl);
                        return RedirectToAction($"{routeValues.Item2}", $"{routeValues.Item1}");
                    }
                  return RedirectToAction("Index", "Home");
                }
  
            }
            else
            {
                ViewBag.IsInvalid = "The User ID and/or Password are not recognized. The password is CASE sensitive. Please ensure that the Caps Lock setting is not enabled. Please re-enter your User Name and Password or contact your administrator to have your password reset.";
            }

            return View();
        }

        public ActionResult Logout()
        {
            // Clear authentication cookie
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            authCookie.Expires = DateTime.Now.AddYears(-1);
            System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
            SessionObject.Logout();
            return RedirectToAction("Index", "Login");
        }
    }
}