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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.MockTestAttempts = new HashSet<MockTestAttempt>();
        }
    
        public string Login { get; set; }
        public string Password { get; set; }
        public string DefaultLanguageCode { get; set; }
        public bool Isactive { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateOn { get; set; }
        public string EditBy { get; set; }
        public Nullable<System.DateTime> EditOn { get; set; }
        public string UserTypeCode { get; set; }
        public Nullable<int> LogoDataFileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ResetPasswordCode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MockTestAttempt> MockTestAttempts { get; set; }
        public virtual UserType UserType { get; set; }
    }
}
