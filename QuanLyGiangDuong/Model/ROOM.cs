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
    
    public partial class ROOM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ROOM()
        {
            this.USINGCLASSes = new HashSet<USINGCLASS>();
            this.USINGEVENTs = new HashSet<USINGEVENT>();
            this.USINGEXAMs = new HashSet<USINGEXAM>();
        }
    
        public string RoomID { get; set; }
        public int Capacity { get; set; }
        public string Description_ { get; set; }
        public string Status_ { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USINGCLASS> USINGCLASSes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USINGEVENT> USINGEVENTs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USINGEXAM> USINGEXAMs { get; set; }
    }
}
