using DotNetOpenAuth.GoogleOAuth2;
using Microsoft.AspNet.Membership.OpenAuth;
using OnlineMasterG.Base;
using OnlineMasterG.CommonFramework;
using OnlineMasterG.CommonServices;
using OnlineMasterG.Models.DAL;
using OnlineMasterG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        public ActionResult RedirectToGoogle()
        {
            string provider = "google";
            string returnUrl = "";
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }
        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            string ProviderName = OpenAuth.GetProviderNameFromCurrentRequest();

            if (ProviderName == null || ProviderName == "")
            {
                NameValueCollection nvs = Request.QueryString;
                if (nvs.Count > 0)
                {
                    if (nvs["state"] != null)
                    {
                        NameValueCollection provideritem = HttpUtility.ParseQueryString(nvs["state"]);
                        if (provideritem["__provider__"] != null)
                        {
                            ProviderName = provideritem["__provider__"];
                        }
                    }
                }
            }

            GoogleOAuth2Client.RewriteRequest();

            var redirectUrl = Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl });
            var retUrl = returnUrl;
            var authResult = OpenAuth.VerifyAuthentication(redirectUrl);

            //string ProviderDisplayName = OpenAuth.GetProviderDisplayName(ProviderName);

            if (!authResult.IsSuccessful)
            {
                return Redirect(Url.Action("login", "Login"));
            }

            // User has logged in with provider successfully
            // Check if user is already registered locally
            //You can call you user data access method to check and create users based on your model
            if (OpenAuth.Login(authResult.Provider, authResult.ProviderUserId, createPersistentCookie: false))
            {
                if (!CheckDuplicateSignup(authResult.ExtraData["email"].ToString()))
                {
                    User user = new User();
                    user.Login = authResult.ExtraData["email"];
                    user.Password = CustomEncrypt.Encrypt("googlelogin");
                    user.Isactive = true;
                    user.CreateBy = authResult.ExtraData["email"];
                    user.CreateOn = DateTime.Now;
                    user.UserTypeCode = UserService.UserTypes.STUDENT.ToString();
                    UserService.SaveUser(user);
                }
                // Setting up Cookies



                Session["email"] = authResult.ExtraData["email"].ToString();
                return Redirect(Url.Action("Index", "Home"));
            }

            //Get provider user details
            string ProviderUserId = authResult.ProviderUserId;
            string ProviderUserName = authResult.UserName;

            string Email = null;
            if (Email == null && authResult.ExtraData.ContainsKey("email"))
            {
                Email = authResult.ExtraData["email"];
            }

            if (User.Identity.IsAuthenticated)
            {
                // User is already authenticated, add the external login and redirect to return url
                OpenAuth.AddAccountToExistingUser(ProviderName, ProviderUserId, ProviderUserName, User.Identity.Name);
                return Redirect(Url.Action("Index", "Home"));
            }
            else
            {
                // User is new, save email as username
                string membershipUserName = Email ?? ProviderUserId;
                var createResult = OpenAuth.CreateUser(ProviderName, ProviderUserId, ProviderUserName, membershipUserName);

                if (!createResult.IsSuccessful)
                {
                    ViewBag.Message = "User cannot be created";
                    return View();
                }
                else
                {
                    // User created
                    if (OpenAuth.Login(ProviderName, ProviderUserId, createPersistentCookie: false))
                    {
                        return Redirect(Url.Action("Index", "Home"));
                    }
                }
            }
            return View();
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OpenAuth.RequestAuthentication(Provider, ReturnUrl);
            }
        }
        private bool CheckDuplicateSignup(string login)
        {
            int count = UserService.FetchUserList(login).Count();
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}