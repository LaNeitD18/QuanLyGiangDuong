using QuanLyGiangDuong.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QuanLyGiangDuong.Model
{
    public partial class USINGEXAM : IUsing
    {
        public USINGEXAM()
        {

        }

        public USINGEXAM(USINGEXAM usingExam)
        {
            UsingExamID = usingExam.UsingExamID;
            RoomID = usingExam.RoomID;
            ExamID = usingExam.ExamID;
            Date_ = usingExam.Date_;
            StartPeriod = usingExam.StartPeriod;
            Duration = usingExam.Duration;
            Status_ = usingExam.Status_;
            Description_ = usingExam.Description_;

            EXAM = usingExam.EXAM;
            ROOM = usingExam.ROOM;
            PERIOD_TIMERANGE = usingExam.PERIOD_TIMERANGE;
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
            var value = (from ex in DataProvider.Ins.DB.EXAMs
                         join cl in DataProvider.Ins.DB.CLASSes on ex.ClassID equals cl.ClassID
                         join lec in DataProvider.Ins.DB.LECTURERs on cl.LecturerID equals lec.LecturerID
                         where ex.ExamID == ExamID
                         select new { ex, cl, lec}).ToList()[0];

            return "Thi" + Environment.NewLine + 
                "-------------------" + Environment.NewLine +
                value.cl.ClassName + Environment.NewLine + 
                "GV: " + value.lec.LecturerName + Environment.NewLine +
                "______________" + Environment.NewLine +
                "Sỉ số: " + value.ex.Population_;
        }

        public string StatusString
        {
            get => Enums.GetStringOf((Enums.UsingStatus)Status_);
            set { /* cant set this field */ }
        }
    }
}