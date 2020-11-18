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
    
    public partial class CLASS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CLASS()
        {
            this.EXAMs = new HashSet<EXAM>();
            this.USINGCLASSes = new HashSet<USINGCLASS>();
        }
    
        public string ClassID { get; set; }
        public string SubjectID { get; set; }
        public string TrainingTypeID { get; set; }
        public string ClassName { get; set; }
        public int Semester { get; set; }
        public int Year_ { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public int Population_ { get; set; }
        public string LecturerID { get; set; }
        public string Description_ { get; set; }
    
        public virtual LECTURER LECTURER { get; set; }
        public virtual SUBJECT_ SUBJECT_ { get; set; }
        public virtual TRAININGTYPE TRAININGTYPE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EXAM> EXAMs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USINGCLASS> USINGCLASSes { get; set; }
    }
}
