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
    
    public partial class EXAM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EXAM()
        {
            this.USINGEXAMs = new HashSet<USINGEXAM>();
        }
    
        public string ExamID { get; set; }
        public string LecturerID { get; set; }
        public string ClassID { get; set; }
        public int Population_ { get; set; }
        public string Description_ { get; set; }
    
        public virtual CLASS CLASS { get; set; }
        public virtual LECTURER LECTURER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USINGEXAM> USINGEXAMs { get; set; }
    }
}