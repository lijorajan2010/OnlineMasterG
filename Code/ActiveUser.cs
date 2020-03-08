using OnlineMasterG.CommonServices;
using OnlineMasterG.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Code
{
    public class ActiveUser
    {
        #region Properties

        public string Login { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserTypeCode { get; set; }
        public int? LogoDataFileId { get; set; }
        public string DefaultLanguageCode { get; set; }
        public bool AllowWebsiteDefaultCulture { get; set; }

        public bool IsSuperAdmin
        {
            get { return UserTypeCode == "SUPERADMIN"; }
        }
        public bool IsAdmin
        {
            get { return UserTypeCode == "ADMIN"; }
        }

        public bool IsStudent
        {
            get { return UserTypeCode == "STUDENT"; }
        }

        #endregion

        #region Constructors

        public ActiveUser(string login)
        {
            User user = UserService.Fetch(login);
            var logoId = UserService.GetUserCompanyLogoDataFileId(user.Login);
            Login = user.Login;
            UserName = user.FirstName + " " + user.LastName;
            Email = user.Login;
            UserTypeCode = user.UserTypeCode;
            LogoDataFileId = logoId;
            DefaultLanguageCode = user.DefaultLanguageCode;
        }
       

        #endregion
    }
}