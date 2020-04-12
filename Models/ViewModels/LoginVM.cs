using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Models.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ReturnUrl { get; set; }
        public bool IsRememberMe { get; set; }
        public int ImgDataFileId { get; set; }
        public HttpPostedFileBase postedFile { get; set; }
    }
}