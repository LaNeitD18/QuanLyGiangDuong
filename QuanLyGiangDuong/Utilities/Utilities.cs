using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyGiangDuong.Utilities
{
    static class Utils
    {
        static private Random _random = new Random();

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
    }
}
