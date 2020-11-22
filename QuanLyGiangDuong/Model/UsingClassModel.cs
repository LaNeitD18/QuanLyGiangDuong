using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QuanLyGiangDuong.Model
{
    public partial class USINGCLASS : IUsing
    {
        //public UsingClassModel(USINGCLASS uSINGCLASS)
        //{
        //    UsingClassID = uSINGCLASS.UsingClassID;
        //    RoomID = uSINGCLASS.RoomID;
        //    ClassID = uSINGCLASS.ClassID;
        //    StartPeriod = uSINGCLASS.StartPeriod;
        //    Duration = uSINGCLASS.Duration;
        //    RepeatCycle = uSINGCLASS.RepeatCycle;
        //    Day_ = uSINGCLASS.Day_;
        //    Status_ = uSINGCLASS.Status_;
        //    Description_ = uSINGCLASS.Description_;
        //    CLASS = uSINGCLASS.CLASS;

        //    PERIOD_TIMERANGE = uSINGCLASS.PERIOD_TIMERANGE;
        //    ROOM = uSINGCLASS.ROOM;
        //    ROOMIGNOREs = uSINGCLASS.ROOMIGNOREs;
        //}

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
            var value = (from cl in DataProvider.Ins.DB.CLASSes
                         join lec in DataProvider.Ins.DB.LECTURERs on cl.LecturerID equals lec.LecturerID
                         where cl.ClassID == ClassID
                         select new { cl, lec }).ToList()[0];

            return value.cl.ClassName + Environment.NewLine + value.lec.LecturerName + 
                Environment.NewLine + "______________" +
                Environment.NewLine + "Sỉ số: " + value.cl.Population_;
        }
    }
}