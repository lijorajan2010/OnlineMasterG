using OnlineMasterG.CommonFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Code
{
    public class ActionAuthorization
    {
        public static bool Authorize(string action)
        {
            if (AppInfo.ActiveUser.IsAdmin)
            {
                return true;
            }
            if (AppInfo.ActiveUser.UserActions.Select(m=>m.Action).Contains(action))
            {
                return true;
            }
            return false;
        }
    }
}