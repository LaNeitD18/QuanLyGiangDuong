using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QuanLyGiangDuong.Model
{
    class UsingClassModel : USINGCLASS, IUsing
    {
        public UsingClassModel(USINGCLASS uSINGCLASS)
        {
            UsingClassID = uSINGCLASS.UsingClassID;
            RoomID = uSINGCLASS.RoomID;
            ClassID = uSINGCLASS.ClassID;
            StartPeriod = uSINGCLASS.StartPeriod;
            EndPeriod = uSINGCLASS.EndPeriod;
            RepeatCycle = uSINGCLASS.RepeatCycle;
            Day_ = uSINGCLASS.Day_;
            Status_ = uSINGCLASS.Status_;
            Description_ = uSINGCLASS.Description_;
            CLASS = uSINGCLASS.CLASS;

            PERIOD_TIMERANGE = uSINGCLASS.PERIOD_TIMERANGE;
            PERIOD_TIMERANGE1 = uSINGCLASS.PERIOD_TIMERANGE1;
            ROOM = uSINGCLASS.ROOM;
            ROOMIGNOREs = uSINGCLASS.ROOMIGNOREs;
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

        public string GetDisplayString()
        {
            return DataProvider.Ins.DB.CLASSes.Find(ClassID).ClassName;
        }
    }
}