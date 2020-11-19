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
        #endregion

    }
}
