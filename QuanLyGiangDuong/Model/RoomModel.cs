using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyGiangDuong.Utilities;

namespace QuanLyGiangDuong.Model
{
    public partial class ROOM
    {
        public string RoomName
        {
            get
            {
                if(RoomID == Utils.NullStringId)
                    return "[Tự động]";
                return RoomID;
            }

            set
            {
                if(value == "[Tự động]")
                    RoomID = Utils.NullStringId;
                RoomID = value;
            }
        }
    }
}
