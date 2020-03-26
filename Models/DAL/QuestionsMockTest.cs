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
    
    public partial class QuestionsMockTest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public QuestionsMockTest()
        {
            this.QuestionAnswerChoices = new HashSet<QuestionAnswerChoice>();
            this.QuestionPoints = new HashSet<QuestionPoint>();
            this.MockTestAttemptDetails = new HashSet<MockTestAttemptDetail>();
        }
    
        public int QuestionsMockTestId { get; set; }
        public string Question { get; set; }
        public string Description { get; set; }
        public string LanguageCode { get; set; }
        public bool Isactive { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> QuestionUploadId { get; set; }
        public Nullable<int> QuestionImageFileId { get; set; }
        public int QuestionNumber { get; set; }
        public string QuestionSet { get; set; }
        public string Solution { get; set; }
    
        public virtual DataFile DataFile { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionAnswerChoice> QuestionAnswerChoices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionPoint> QuestionPoints { get; set; }
        public virtual QuestionUpload QuestionUpload { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MockTestAttemptDetail> MockTestAttemptDetails { get; set; }
    }
}
