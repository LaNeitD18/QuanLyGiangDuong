using QuanLyGiangDuong.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLyGiangDuong.Model
{
    class UsingEventModel: USINGEVENT, IUsing
    {
        public UsingEventModel(USINGEVENT usingEvent)
        {
            UsingEventID = usingEvent.UsingEventID;
            RoomID = usingEvent.RoomID;
            EventID = usingEvent.EventID;
            Date_ = usingEvent.Date_;
            StartPeriod = usingEvent.StartPeriod;
            EndPeriod = usingEvent.EndPeriod;
            Status_ = usingEvent.Status_;
            Description_ = usingEvent.Description_;

            EVENT_ = usingEvent.EVENT_;
            ROOM = usingEvent.ROOM;
            PERIOD_TIMERANGE = usingEvent.PERIOD_TIMERANGE;
            PERIOD_TIMERANGE1 = usingEvent.PERIOD_TIMERANGE1;
        }

        public string StatusString
        {
            get => Enums.GetStringOf((Enums.UsingStatus)Status_);
            set { /* cant set this field */ }
        }

        public string ShortDateString
        {
            get => Date_.ToString("dd/MM/yyyy");
            set { /* cant set this field */ }
        }

        public USINGEVENT USINGEVENT
        {
            get
            {
                USINGEVENT res = (USINGEVENT)this.MemberwiseClone();
                return (USINGEVENT)this.MemberwiseClone();
            }                
        }

        public string GetDisplayString()
        {
            return DataProvider.Ins.DB.EVENT_.Find(EventID).EventName;    
        }

        private string _displayName;
        public string DisplayName
        {
            get
            {
                _displayName = GetDisplayString();
                return _displayName;
            }

            set
            {
                _displayName = value;
            }
        }
    }
}
