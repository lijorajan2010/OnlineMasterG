using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace OnlineMasterG.CommonFramework
{
    public static class AppInfo
    {
        public static string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string GetCookieCulture
        {
            get
            {
                var langCookie = HttpContext.Current.Request.Cookies["lang"];

                if (langCookie == null)
                {
                    var cookie = new HttpCookie("lang", "en-US") { HttpOnly = true };
                    HttpContext.Current.Response.AppendCookie(cookie);
                }

                return langCookie == null ? "en-US" : langCookie.Value;
            }
        }

    }
}