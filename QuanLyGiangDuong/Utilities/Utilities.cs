using QuanLyGiangDuong.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyGiangDuong.Utilities
{
    static class Utils
    {
        static private Random _random = new Random();
        static public readonly string NullRoomId = "[NULL]";
        static public readonly int NullPeriodTimeRangeId = -1;

        static public string GenerateStringId(DbSet dbset)
        {
            // try generate random id 10 times first 
            for (int i = 0; i <= 10; i++)
            {
                string attempt = _random.Next().ToString();
                if (dbset.Find(attempt.ToString()) == null)
                    return attempt.ToString();
            }

            // bruteforce to find real love
            for (int i = 0; i <= int.MaxValue; i++)
                if (dbset.Find(i.ToString()) == null)
                    return i.ToString();

            throw new Exception("out of IDs");
        }

        
        static public void InitDatabase()
        {
            // Null values
            if(DataProvider.Ins.DB.ROOMs.Find(NullRoomId) == null)
                DataProvider.Ins.DB.ROOMs.Add
                (
                    new ROOM() 
                    { 
                        RoomID = NullRoomId, 
                        Status_ = "",
                        Capacity = 0 
                    }
                );

            if(DataProvider.Ins.DB.PERIOD_TIMERANGE.Find(NullPeriodTimeRangeId) == null)
                DataProvider.Ins.DB.PERIOD_TIMERANGE.Add(
                    new PERIOD_TIMERANGE() 
                    { 
                        PeriodID = NullPeriodTimeRangeId, 
                        StartTime = DateTime.Now, EndTime = DateTime.Now,
                        PeriodName = "NULL"
                    }
                );

            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
