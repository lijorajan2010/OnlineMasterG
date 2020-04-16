using OnlineMasterG.Base;
using OnlineMasterG.CommonFramework;
using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.CommonServices
{
    public  class SecurityService : ServiceBase
    {
        public static User FastLogin(string login, string password)
        {
            User user = UserService.Fetch(login);

            if (user != null && user.Isactive)
            {
                if (PasswordUtilities.CompareHash(password, user.Password))
                {
                    return user;
                }
            }

            return null;
        }
    }
}