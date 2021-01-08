using QuanLyGiangDuong.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLyGiangDuong.Model
{
    public partial class USINGEVENT : IUsing
    {
        public USINGEVENT()
        {

        }

        public USINGEVENT(USINGEVENT usingEvent)
        {
            UsingEventID = usingEvent.UsingEventID;
            RoomID = usingEvent.RoomID;
            EventID = usingEvent.EventID;
            Date_ = usingEvent.Date_;
            StartPeriod = usingEvent.StartPeriod;
            Duration = usingEvent.Duration;
            Status_ = usingEvent.Status_;
            Description_ = usingEvent.Description_;

            EVENT_ = usingEvent.EVENT_;
            ROOM = usingEvent.ROOM;
            PERIOD_TIMERANGE = usingEvent.PERIOD_TIMERANGE;
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

        public string GetDisplayString()
        {
            return "Sự kiện" + Environment.NewLine + 
                "-------------------" + Environment.NewLine +
                DataProvider.Ins.DB.EVENT_.Find(EventID).EventName;    
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
