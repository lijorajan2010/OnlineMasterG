using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Models.ViewModels
{
    public class CurrentAffairsVM
    {
        public int CurrentAffairsCategoryId { get; set; }
        public string AffairsCategoryName { get; set; }
        public string LanguageCode { get; set; }
        public int Sequence { get; set; }
        public bool IsActive { get; set; }
    }
}