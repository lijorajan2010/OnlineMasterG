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
    
    public partial class ProblemsReported
    {
        public int ProblemReportId { get; set; }
        public Nullable<int> AttemptDetailId { get; set; }
        public int ProblemId { get; set; }
        public bool IsReported { get; set; }
        public string IssueText { get; set; }
    
        public virtual MockTestAttemptDetail MockTestAttemptDetail { get; set; }
        public virtual ProblemMaster ProblemMaster { get; set; }
    }
}
