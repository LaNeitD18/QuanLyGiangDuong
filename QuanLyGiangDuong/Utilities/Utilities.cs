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
        static public readonly string NullStringId = "[NULL]";
        static public readonly int NullIntId = -1;

        #region Id generator
        static public string GenerateStringId(DbSet dbset)
        {
            // try generate random id 10 times first 
            //for (int i = 0; i <= 10; i++)
            //{
            //    string attempt = _random.Next().ToString();
            //    if (dbset.Find(attempt.ToString()) == null)
            //        return attempt.ToString();
            //}

            // I no longer want to randomize the Ids, sorry ;)

            // bruteforce to find real love
            for (int i = 0; i <= int.MaxValue; i++)
                if (dbset.Find(i.ToString()) == null)
                    return i.ToString();

            throw new Exception("out of IDs");
        }
        #endregion

        #region StartTime - EndTime - Duration
        /// <summary>
        /// calculate the duration from startPeriod to endPeriod in Minute
        /// </summary>
        /// <param name="startPeriodId"></param>
        /// <param name="endPeriodId"></param>
        /// <returns></returns>
        static public int CalcDuration(int startPeriodId, int endPeriodId)
        {
            PERIOD_TIMERANGE startPeriod = DataProvider.Ins.DB.PERIOD_TIMERANGE.Find(startPeriodId);

            int startMinute = (int)Math.Round(startPeriod.StartTime.TotalMinutes);
            int endMinute   = (int)Math.Round(CalcEndTime(endPeriodId).TotalMinutes);

            return endMinute - startMinute;
        }

        static public TimeSpan CalcEndTime(int periodId)
        {
            PERIOD_TIMERANGE period = DataProvider.Ins.DB.PERIOD_TIMERANGE.Find(periodId);
            return CalcEndTime(period);
        }

        static public TimeSpan CalcEndTime(PERIOD_TIMERANGE period)
        {
            return period.StartTime + period.Duration;
        }

        /// <summary>
        /// Calculate the end period from startPeriodId and duration (minutes)
        /// </summary>
        /// <param name="startPeriod"></param>
        /// <param name="duration"></param>
        /// <returns>end period Id. returns -1 if there is no valid end period</returns>
        static public int CalcEndPeriod(int startPeriodId, int duration)
        {
            PERIOD_TIMERANGE startPeriod = DataProvider.Ins.DB.PERIOD_TIMERANGE.Find(startPeriodId);
            int startMinute = (int)Math.Round(startPeriod.StartTime.TotalMinutes);
            int endMinute   = startMinute + duration;

            int bestEndMinute = int.MaxValue;
            int bestPeriodId = -1;

            foreach(var period in DataProvider.Ins.DB.PERIOD_TIMERANGE)
            {
                int endPeriodMinute = (int)Math.Round(CalcEndTime(period).TotalMinutes);
                
                // firstly, the period must end after the end of the duration
                if(endPeriodMinute >= endMinute)
                {
                    if(endPeriodMinute < bestEndMinute)
                    {
                        bestEndMinute = endPeriodMinute;
                        bestPeriodId = period.PeriodID;
                    }
                }
            }

            return bestPeriodId;
        }

        /// <summary>
        /// Calculate the end period from startPeriodId and duration
        /// </summary>
        /// <returns>end period Id. returns -1 if there is no valid end period</returns>
        static public int CalcEndPeriod(int startPeriod, TimeSpan duration)
        {
            int durationInMinute = (int)Math.Round(duration.TotalMinutes);
            return CalcEndPeriod(startPeriod, durationInMinute);
        }

        /// <summary>
        /// Get List of Period_TimeRange base on the start period and Duration of its
        /// </summary>
        /// <param name="StartPeriod"></param>
        /// <param name="Duration"></param>
        /// <returns></returns>
        static public List<PERIOD_TIMERANGE> GetListPeriodTimeRange(int StartPeriod, TimeSpan Duration)
        {
            var startTime = DataProvider.Ins.DB.PERIOD_TIMERANGE.Find(StartPeriod).StartTime;

            var endtime = startTime + Duration;

            var listPeriodTimeRange = (from t in DataProvider.Ins.DB.PERIOD_TIMERANGE
                     where t.StartTime >= startTime &&
                           t.StartTime < endtime
                     select t).ToList();

            return listPeriodTimeRange;
        }
        #endregion
    }
}
