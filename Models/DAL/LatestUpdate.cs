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
    
    public partial class LatestUpdate
    {
        public int UpdateId { get; set; }
        public string UpdateDescription { get; set; }
        public Nullable<int> Sequence { get; set; }
        public string LanguageCode { get; set; }
        public bool Isactive { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateOn { get; set; }
        public string EditBy { get; set; }
        public Nullable<System.DateTime> EditOn { get; set; }
    }
}