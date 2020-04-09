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
    
    public partial class MockTestAttempt
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MockTestAttempt()
        {
            this.MockTestAttemptDetails = new HashSet<MockTestAttemptDetail>();
        }
    
        public int AttemptId { get; set; }
        public string Login { get; set; }
        public Nullable<int> TestId { get; set; }
        public bool IsPaused { get; set; }
        public Nullable<decimal> TimeLeftInMinutes { get; set; }
        public bool IsCompleted { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string EditBy { get; set; }
        public Nullable<System.DateTime> EditDate { get; set; }
        public Nullable<decimal> FinalMarksScoredForRank { get; set; }
        public Nullable<int> Rating { get; set; }
        public string Review { get; set; }
    
        public virtual MockTest MockTest { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MockTestAttemptDetail> MockTestAttemptDetails { get; set; }
        public virtual User User { get; set; }
    }
}
