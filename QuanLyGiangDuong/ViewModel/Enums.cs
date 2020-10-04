using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace QuanLyGiangDuong.ViewModel
{
    public static class Enums
    {
        #region enums
        public enum UsingStatus
        {
            Pending     = 0,
            Approved    = 1,
            Rejected    = 2,
        }

        public enum UserType
        {
            Admin       = 0,
            Lecturer    = 1,
        }

        public enum TimeRange
        {
            Period1     = 1,
            Period2     = 2,
            Period3     = 3,
            Period4     = 4,
            Period5     = 5,
            LunchTime   = 6,
            Period6     = 7,
            Period7     = 8,
            Period8     = 9,
            Period9     = 10,
            Period10    = 11,
            AfterSchool = 12,
        }
        #endregion

        #region to String
        static public string GetStringOf(UsingStatus us)
        {
            switch (us)
            {
                case UsingStatus.Pending:
                    return "Đang chờ";
                case UsingStatus.Approved:
                    return "Đã duyệt";
                case UsingStatus.Rejected:
                    return "Đã từ chối";
                default:
                    throw new Exception(us.ToString() + "'s string was not define");
            }
        }

        static public string GetStringOf(UserType ut)
        {
            switch (ut)
            {
                case UserType.Admin:
                    return "Ban giám hiệu";
                case UserType.Lecturer:
                    return "Giảng viên";
                default:
                    throw new Exception(ut.ToString() + "'s string was not define");
            }
        }

        static public string GetStringOf(TimeRange tr)
        {
            string periodToString(int n)
            {
                return "Tiết " + n.ToString();
            }

            switch (tr)
            {
                case TimeRange.Period1:
                    return periodToString(1);
                case TimeRange.Period2:
                    return periodToString(2);
                case TimeRange.Period3:
                    return periodToString(3);
                case TimeRange.Period4:
                    return periodToString(4);
                case TimeRange.Period5:
                    return periodToString(5);
                case TimeRange.Period6:
                    return periodToString(6);
                case TimeRange.Period7:
                    return periodToString(7);
                case TimeRange.Period8:
                    return periodToString(8);
                case TimeRange.Period9:
                    return periodToString(9);
                case TimeRange.Period10:
                    return periodToString(10);
                case TimeRange.LunchTime:
                    return "Giờ nghỉ trưa";
                case TimeRange.AfterSchool:
                    return "Sau giờ học";

                default:
                    throw new Exception(tr.ToString() + "'s string was not define");
            }
        }
        #endregion

    }
}
