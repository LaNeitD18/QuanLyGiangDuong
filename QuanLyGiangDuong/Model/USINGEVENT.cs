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
    
    public partial class USINGEVENT
    {
        public string UsingEventID { get; set; }
        public string RoomID { get; set; }
        public string EventID { get; set; }
        public System.DateTime Date_ { get; set; }
        public int StartPeriod { get; set; }
        public System.TimeSpan Duration { get; set; }
        public int Status_ { get; set; }
        public string Description_ { get; set; }
    
        public virtual EVENT_ EVENT_ { get; set; }
        public virtual PERIOD_TIMERANGE PERIOD_TIMERANGE { get; set; }
        public virtual ROOM ROOM { get; set; }
    }
}
