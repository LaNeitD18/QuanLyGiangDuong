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
    
    public partial class ROOMIGNORE
    {
        public string RoomIgnoreID { get; set; }
        public string UsingClassID { get; set; }
        public System.DateTime IgnoreDate { get; set; }
    
        public virtual USINGCLASS USINGCLASS { get; set; }
    }
}
