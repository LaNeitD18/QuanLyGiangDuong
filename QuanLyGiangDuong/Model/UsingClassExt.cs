using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QuanLyGiangDuong.Utilities;

namespace QuanLyGiangDuong.Model
{
    public partial class USINGCLASS
    {
        public int EndPeriod
        {
            get
            {
                return Utils.CalcEndPeriod(StartPeriod, Duration);
            }

            set
            {
                Duration = TimeSpan.FromMinutes(Utils.CalcDuration(StartPeriod, value));
            }
        }
    }
}
