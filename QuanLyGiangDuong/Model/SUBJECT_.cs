//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyGiangDuong.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SUBJECT_
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SUBJECT_()
        {
            this.CLASSes = new HashSet<CLASS>();
        }
    
        public string SubjectID { get; set; }
        public string SubjectName { get; set; }
        public string FacaultyID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLASS> CLASSes { get; set; }
        public virtual FACAULTY FACAULTY { get; set; }
    }
}
