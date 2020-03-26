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
    
    public partial class Subject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Subject()
        {
            this.QuestionUploads = new HashSet<QuestionUpload>();
            this.GeneralInstructions = new HashSet<GeneralInstruction>();
            this.MockTestAttemptDetails = new HashSet<MockTestAttemptDetail>();
        }
    
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string LanguageCode { get; set; }
        public bool Isactive { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateOn { get; set; }
        public string EditBy { get; set; }
        public Nullable<System.DateTime> EditOn { get; set; }
        public Nullable<int> CourseId { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public Nullable<int> TestId { get; set; }
        public int Sequence { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Course Course { get; set; }
        public virtual MockTest MockTest { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionUpload> QuestionUploads { get; set; }
        public virtual Section Section { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GeneralInstruction> GeneralInstructions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MockTestAttemptDetail> MockTestAttemptDetails { get; set; }
    }
}
