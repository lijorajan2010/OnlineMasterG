using DotNetOpenAuth.GoogleOAuth2;
using Microsoft.AspNet.Membership.OpenAuth;
using OnlineMasterG.Base;
using OnlineMasterG.CommonFramework;
using OnlineMasterG.CommonServices;
using OnlineMasterG.Models.DAL;
using OnlineMasterG.Models.Shared;
using OnlineMasterG.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
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
        [AllowAnonymous]
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
            //// Validate view model first
            //if (!ModelState.IsValid)
            //    return View(loginModel);

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
                LoginVM Coockieuser = new LoginVM() { 
                Login = userData.Login,
                Password=userData.Password,
                FirstName = userData.FirstName,
                LastName = userData.LastName
                };
              
                SessionObject.SetUserInFormsCookie(Coockieuser);
                string CultLang = userData != null ? (!string.IsNullOrEmpty(userData.DefaultLanguageCode) ? userData.DefaultLanguageCode : "en-US") : "en-US";
                var langCookie = new HttpCookie("lang", userData.DefaultLanguageCode) { HttpOnly = true };
                Response.AppendCookie(langCookie);
                var ci = new CultureInfo(CultLang);
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = ci;
                var languages = LanguageService.FetchList();

                string dir = languages.Where(x => x.LanguageCode == CultLang).FirstOrDefault().LanguageCode;
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
                ViewBag.IsInvalid = "The EmailId and/or Password is not recognized. The password is CASE sensitive. Please ensure that the Caps Lock setting is not enabled. Please re-enter your EmailId and Password or contact your administrator to have your password reset / Click forgot password.";
            }

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Signup(LoginVM loginVM)
        {
            bool isValidEmail = RegexUtilities.IsValidEmail(loginVM.Login);
            if (isValidEmail)
            {
                ServiceResponse sr = new ServiceResponse();
                if (!CheckDuplicateSignup(loginVM.Login))
                {
                    User user = new User();
                    user.Login = loginVM.Login;
                    user.Password = PasswordUtilities.CreateHash(loginVM.Password);
                    user.Isactive = true;
                    user.CreateBy = loginVM.Login;
                    user.CreateOn = DateTime.Now;
                    user.UserTypeCode = UserService.UserTypes.STUDENT.ToString();
                    sr =  UserService.SaveUser(user);
                }
                else
                {
                    ViewBag.Message = "This emailId is already registered";
                    return View();
                }
                if (!sr.Status)
                {
                    ViewBag.Message = "Something went wrong ! Please try after sometime.";
                    return View();
                }
                else
                {
                    // Sen an Email After Account Creation

                    var subject = "New Account created ";
                    var body = "Hi " + loginVM.FirstName + " " + loginVM.LastName + ", <br/> New account has been created. " +

                         " <br/><br/> Username is "+ loginVM .Login+ " <br/><br/>" +
                           " <br/>Password is " + loginVM.Password + " <br/><br/>" +
                         "Now you can login into OnlineMasterJi.<br/><br/> Thank you";

                    //Get and set the AppSettings using configuration manager.  
                    string FromAddress, Password, SMTPPort, Host;
                    EmailManager.AppSettings(out FromAddress, out Password, out SMTPPort, out Host);
                    //Call send email methods.  
                    EmailManager.SendEmail(loginVM.Login, body, subject, FromAddress, Password, SMTPPort, Host);


                    // Create LoginVM
                    LoginVM login = new LoginVM();
                    login.Login = loginVM.Login;
                    login.Password = loginVM.Password;
                    login.IsRememberMe = true;
                    return Redirect(Url.Action("Index", "Login", new { loginModel = loginVM }));
                }

            }
            else
            {
                ViewBag.Message = "Please enter a valid Email Address";
            }

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {
            bool isValidEmail = RegexUtilities.IsValidEmail(Email);
            if (isValidEmail)
            {
                string resetCode = Guid.NewGuid().ToString();
                resetCode = resetCode.Substring(0, 6).ToUpper();
                string enCryptedResetCode = CustomEncrypt.SafeUrlEncrypt(resetCode);
                var verifyUrl = "/Login/ResetPassword?c=" + enCryptedResetCode;
                var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

                var getUser = UserService.Fetch(Email);
                if (getUser != null)
                {
                    getUser.ResetPasswordCode = resetCode;

                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                    UserService.UpdateUser(getUser);
                    var subject = "Password Reset Request";
                    var body = "Hi " + getUser.FirstName + "/" + getUser.Login + ", <br/> You recently requested to reset your password for your account. Click the link below to reset it. " +

                         " <br/><br/><a href='" + link + "'>" + link + "</a> <br/><br/>" +
                         "If you did not request a password reset, please ignore this email.<br/><br/> Thank you";

                    //Get and set the AppSettings using configuration manager.  
                    string FromAddress, Password, SMTPPort, Host;
                    EmailManager.AppSettings(out FromAddress, out Password, out SMTPPort, out Host);
                    //Call send email methods.  
                    EmailManager.SendEmail(Email, body, subject, FromAddress, Password, SMTPPort, Host);

                    ViewBag.Message = "Reset password link has been sent to your email id.";
                }
                else
                {
                    ViewBag.Message = "User doesn't exists.";
                    return View();
                }
            }
            else
            {
                ViewBag.Message = "Please enter a valid Email Address";
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string c)
        {
            ResetPasswordModel model = new ResetPasswordModel();
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(c))
            {
                ViewBag.Message = "There is something went wrong !";
                return View(model);
            }
            else
            {
                string deCryptedResetCode = CustomEncrypt.SafeUrlDecrypt(c);
                var getUser = UserService.FetchUserByResetCode(deCryptedResetCode);
                if (getUser != null)
                {
                    model.ResetCode = c;
                    return View(model);
                }
                else
                {
                    ViewBag.Message = "There is something went wrong !";
                    return View(model);
                }
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                var getUser = UserService.FetchUserByResetCode(model.ResetCode);
                if (getUser!=null)
                {
                    //you can encrypt password here, we are not doing it
                    getUser.Password = PasswordUtilities.CreateHash(model.NewPassword);
                    //make resetpasswordcode empty string now
                    getUser.ResetPasswordCode = null;
                    //to avoid validation issues, disable it
                    UserService.UpdateUser(getUser);
                    message = "New password updated successfully";
                }
            }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message = message;
            return View(model);
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

        [AllowAnonymous]
        public ActionResult RedirectToGoogle()
        {
            string provider = "google";
            string returnUrl = "";
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
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

            if (!authResult.IsSuccessful)
            {
                return Redirect(Url.Action("Index", "Login"));
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
                    user.FirstName = authResult.ExtraData["given_name"];
                    user.LastName = authResult.ExtraData["family_name"];
                    user.Password = PasswordUtilities.CreateHash("googlelogin");
                    user.Isactive = true;
                    user.CreateBy = authResult.ExtraData["email"];
                    user.CreateOn = DateTime.Now;
                    user.UserTypeCode = UserService.UserTypes.STUDENT.ToString();
                    UserService.SaveUser(user);
                }
                // Setting up Cookies
                // Create LoginVM
                LoginVM loginVM = new LoginVM();
                loginVM.Login = authResult.ExtraData["email"].ToString();
                loginVM.Password = "googlelogin";
                loginVM.IsRememberMe = true;

                return Redirect(Url.Action("Index", "Login", new { loginModel = loginVM }));
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
                // Setting up Cookies
                // Create LoginVM
                LoginVM loginVM = new LoginVM();
                loginVM.Login = authResult.ExtraData["email"].ToString();
                loginVM.Password = "googlelogin";
                loginVM.IsRememberMe = true;

                return Redirect(Url.Action("Index", "Login", new { loginModel = loginVM }));
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
                    if (!CheckDuplicateSignup(authResult.ExtraData["email"].ToString()))
                    {
                        User user = new User();
                        user.Login = membershipUserName;
                        user.FirstName = membershipUserName;
                        user.LastName = "  ";
                        user.Password = PasswordUtilities.CreateHash("googlelogin");
                        user.Isactive = true;
                        user.CreateBy = membershipUserName;
                        user.CreateOn = DateTime.Now;
                        user.UserTypeCode = UserService.UserTypes.STUDENT.ToString();
                        UserService.SaveUser(user);
                    }

                    // User created
                    if (OpenAuth.Login(ProviderName, ProviderUserId, createPersistentCookie: false))
                    {
                        // Create LoginVM
                        LoginVM loginVM = new LoginVM();
                        loginVM.Login = membershipUserName;
                        loginVM.Password = "googlelogin";
                        loginVM.IsRememberMe = true;

                        return Redirect(Url.Action("Index", "Login", new { loginModel = loginVM }));
                    }
                }
            }
            return View();
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