using OnlineMasterG.Base;
using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.CommonServices
{
  
    public static class UserService
    {
        public enum UserTypes
        {
            SUPERADMIN,
            ADMIN,
            STUDENT
        }

        private static OnlinemasterjiEntities DB = new OnlinemasterjiEntities();

        public static User Fetch(string login)
        {
            return DB
                .Users
                 .AsNoTracking()
                .FirstOrDefault(x => x.Login.Equals(login, StringComparison.OrdinalIgnoreCase));
        }
        public static List<User> FetchUserList(string login)
        {
            return DB
                .Users
                 .AsNoTracking()
                .Where(x => x.Login.Equals(login, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        public static int? GetUserCompanyLogoDataFileId(string login, int? businessUnitId = null)
        {
            // Use login
            if (!string.IsNullOrEmpty(login))
            {
                var user = Fetch(login);

                // Use user logo if exists
                if (user.LogoDataFileId.HasValue)
                    return user.LogoDataFileId;
            }
            return null;
        }

        public static ServiceResponse SaveUser(User user)
        {
            ServiceResponse sr = new ServiceResponse();
            try
            {
                DB.Users.Add(user);
                DB.SaveChanges();
                sr.ReturnCode = user.Login;
            }
            catch (Exception ex)
            {
                sr.AddError(ex.Message);
            }
            return sr;
        }

    }
}