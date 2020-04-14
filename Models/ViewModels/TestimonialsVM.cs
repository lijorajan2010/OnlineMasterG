using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Models.ViewModels
{
    public class TestimonialsVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? ImageDataFileId { get; set; }
        public int? Rating { get; set; }
        public string Review { get; set; }
        public string TestName { get; set; }
        public bool IsApproved { get; set; }
        public int AttemptId { get; set; }
    }
}