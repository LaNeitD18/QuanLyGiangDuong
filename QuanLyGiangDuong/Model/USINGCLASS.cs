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
    
    public partial class USINGCLASS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USINGCLASS()
        {
            this.ROOMIGNOREs = new HashSet<ROOMIGNORE>();
        }
    
        public string UsingClassID { get; set; }
        public string RoomID { get; set; }
        public string ClassID { get; set; }
        public int StartPeriod { get; set; }
        public int EndPeriod { get; set; }
        public int RepeatCycle { get; set; }
        public int Day_ { get; set; }
        public int Status_ { get; set; }
        public string Description_ { get; set; }
    
        public virtual CLASS CLASS { get; set; }
        public virtual PERIOD_TIMERANGE PERIOD_TIMERANGE { get; set; }
        public virtual PERIOD_TIMERANGE PERIOD_TIMERANGE1 { get; set; }
        public virtual ROOM ROOM { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ROOMIGNORE> ROOMIGNOREs { get; set; }
    }
}
