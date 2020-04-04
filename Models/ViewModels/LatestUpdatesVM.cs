using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Models.ViewModels
{
    public class LatestUpdatesVM
    {
        public int UpdateId { get; set; }
        public string UpdateDescription { get; set; }
        public int Sequence { get; set; }
        public string LanguageCode { get; set; }
        public bool IsActive { get; set; }
    }
}