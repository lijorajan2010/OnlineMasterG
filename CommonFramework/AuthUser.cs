using Newtonsoft.Json;
using OnlineMasterG.Code;
using OnlineMasterG.Models.DAL;
using OnlineMasterG.Models.ViewModels;
using System;
using System.IO;
using System.Security.Principal;
using System.Web;
using System.Web.Routing;
using System.Web.Security;

namespace OnlineMasterG.CommonFramework
{
    public class AuthUser : IPrincipal
    {
        public bool IsInRole(string userTypeCode)
        {
            return true;
        }

        public IIdentity Identity { get; private set; }
        public string Login { get; set; }
        public AuthUser(string login)
        {
            Identity = new GenericIdentity(login);
        }
    }
    public static class SessionObject
    {
        public static void SetUserInFormsCookie(LoginVM loginResponse)
        {
            string userDataInJson = JsonConvert.SerializeObject(loginResponse);
            var authTicket = new FormsAuthenticationTicket(1, loginResponse.Login, DateTime.Now, DateTime.Now.AddMinutes(150), false, userDataInJson);
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            System.Web.HttpContext.Current.Response.Cookies.Add(faCookie);

            HttpContext.Current.User = new AuthUser(loginResponse.Login);
        }
        public static SerializablePrincipal GetUserFromFormsCookie()
        {
            HttpContext ctx = System.Web.HttpContext.Current;
            var authCookie = ctx.Request.Cookies[FormsAuthentication.FormsCookieName];

            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            var userData = JsonConvert.DeserializeObject<SerializablePrincipal>(authTicket.UserData);

            return userData;
        }

        public static void Logout()
        {
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            authCookie.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }
        public static Tuple<string, string> _getControllerAndActionName(string fullUrl)
        {
            Tuple<string, string> routeValues;
            var questionMarkIndex = fullUrl.IndexOf('?');
            string queryString = null;
            string url = fullUrl;
            if (questionMarkIndex != -1) // There is a QueryString
            {
                url = fullUrl.Substring(0, questionMarkIndex);
                queryString = fullUrl.Substring(questionMarkIndex + 1);
            }
            // Arranges
            var request = new HttpRequest(null, url, queryString);
            var response = new HttpResponse(new StringWriter());
            var httpContext = new HttpContext(request, response);

            var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            // Extract the data    
            var values = routeData.Values;
            var controllerName = values["controller"].ToString();
            var actionName = values["action"].ToString();
            routeValues = Tuple.Create<string, string>(controllerName, actionName);
            return routeValues;
        }
    }
}