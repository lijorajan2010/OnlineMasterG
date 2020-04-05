using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineMasterG.Models.ViewModels
{
    public class CurrentAffairsUploadVM
    {
        public int CurrentAffairsUploadId { get; set; }
        public int? CurrentAffairsCategoryId { get; set; }
        public int? DataFileId { get; set; }
        public string CurrentAffairsCategoryName { get; set; }
        public DateTime? UploadDate { get; set; }
        public string LanguageCode { get; set; }
        public bool Isactive { get; set; }
        public DataFileVM DataFile { get; set; }
    }
}