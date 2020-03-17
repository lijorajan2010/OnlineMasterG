using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Models.ViewModels
{
    public class ClassVM
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int Sequence { get; set; }
        public string LanguageCode { get; set; }
        public bool IsActive { get; set; }
    }
}