//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineMasterG.Models.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class SchoolPaper
    {
        public int PaperId { get; set; }
        public string PaperName { get; set; }
        public string Description { get; set; }
        public string LanguageCode { get; set; }
        public bool Isactive { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateOn { get; set; }
        public string EditBy { get; set; }
        public Nullable<System.DateTime> EditOn { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<int> SubjectId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public Nullable<int> DataFileId { get; set; }
    
        public virtual DataFile DataFile { get; set; }
        public virtual SchoolClass SchoolClass { get; set; }
        public virtual SchoolSection SchoolSection { get; set; }
        public virtual SchoolSubject SchoolSubject { get; set; }
    }
}