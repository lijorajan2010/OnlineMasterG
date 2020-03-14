using Newtonsoft.Json;
using OnlineMasterG.App_Start;
using OnlineMasterG.Code;
using OnlineMasterG.CommonFramework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace OnlineMasterG
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
			AuthConfig.RegisterAuth();
		}
		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{
			var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

			if (authCookie == null)
				return;

			var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

			if (authTicket == null || authTicket.Expired)
			{
				SessionObject.Logout();
				return;
			}
		
			var serializeModel = JsonConvert.DeserializeObject<SerializablePrincipal>(authTicket.UserData);

			if (string.IsNullOrEmpty(authTicket.UserData))
				return;

			var authUser = new AuthUser(authTicket.Name)
			{
				Login = serializeModel.Login
			};

			HttpContext.Current.User = authUser;
		}
		protected void Application_AcquireRequestState(object sender, EventArgs e)
		{
			// Values are displayed in US Format
			var enCulture = new CultureInfo("en-US");

			var numberFormatInfo = (NumberFormatInfo)enCulture.NumberFormat.Clone();
			var dateTimeInfo = (DateTimeFormatInfo)enCulture.DateTimeFormat.Clone();
			var ci = new CultureInfo(AppInfo.GetCookieCulture) { NumberFormat = numberFormatInfo, DateTimeFormat = dateTimeInfo };

			//Finally setting culture for each request
			Thread.CurrentThread.CurrentUICulture = ci;
			Thread.CurrentThread.CurrentCulture = ci;
		}
	}
}
