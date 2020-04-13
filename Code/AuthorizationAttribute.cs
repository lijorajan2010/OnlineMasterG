using OnlineMasterG.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineMasterG.Code
{
    public class ActionAuthorizationAttribute : AuthorizeAttribute
    {
        public string action { get; set; }

        public ActionAuthorizationAttribute(string action)
        {
            this.action = action;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("No filter context.");

            var user = HttpContext.Current.User;

            // No logged-in user trying to access a private page
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                filterContext.Result = CreateSessionExpiredResult(filterContext);
                return;
            }

            if (!ActionAuthorization.Authorize(this.action))
            {
                filterContext.Result = CreateUnauthorizedResult(filterContext);
                return;
            }
        }

        protected ActionResult CreateUnauthorizedResult(AuthorizationContext filterContext)
        {
            UnauthorizedModel unauthorizedModel = new UnauthorizedModel();
            unauthorizedModel.ControllerName = (string)filterContext.RouteData.Values["controller"];
            unauthorizedModel.ActionName = (string)filterContext.RouteData.Values["action"];
            unauthorizedModel.Message = "You do not have sufficient privileges for this operation.";

            // custom logic to determine proper view here - i'm just hardcoding it
            string viewName = "~/Views/Shared/Unauthorized.cshtml";

            return new ViewResult
            {
                ViewName = viewName,
                ViewData = new ViewDataDictionary<UnauthorizedModel>(unauthorizedModel)
            };
        }

        protected ActionResult CreateSessionExpiredResult(AuthorizationContext filterContext)
        {
            HttpContext httpContext = HttpContext.Current;

            SessionExpiredModel sessionExpiredModel = new SessionExpiredModel();
            sessionExpiredModel.LoginPage = "http://" + httpContext.Request.Url.Host + httpContext.Request.ApplicationPath + "Login";

            // Add header so that we can detec this on ajaxSuccess
            httpContext.Response.AddHeader("X-LOGIN-PAGE", sessionExpiredModel.LoginPage);

            // Custom logic to determine proper view here - i'm just hardcoding it
            string viewName = "~/Views/Shared/SessionExpired.cshtml";

            return new ViewResult
            {
                ViewName = viewName,
                ViewData = new ViewDataDictionary<SessionExpiredModel>(sessionExpiredModel)
            };
        }
    }
}