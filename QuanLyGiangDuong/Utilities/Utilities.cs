using QuanLyGiangDuong.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using QuanLyGiangDuong.ViewModel;

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

        #region Auto make schedule functions

        static void AutoMakeSchedule(USINGCLASS usingClass, CLASS class_)
        {
            var listRoomFiltered = (from room in DataProvider.Ins.DB.ROOMs
                            where room.Capacity >= class_.Population_
                            select room).ToList();

            var targetDate = class_.StartDate.AddDays(usingClass.Day_ - 1);

            TimeTableViewModel timeTableViewModel = new TimeTableViewModel();

            timeTableViewModel.selectedDay = targetDate.Day;
            timeTableViewModel.selectedMonth.monthValue = targetDate.Month;
            timeTableViewModel.selectedYear = targetDate.Year;

            timeTableViewModel.GetTimeTable();

            var listUsingClass = timeTableViewModel.tb; // get all using class for that day

            // only get using on the room that satisfy the capacity
            var listUsingClassFiltered = listUsingClass.Where(x => { return DataProvider.Ins.DB.ROOMs.Find(x.roomID).Capacity >= class_.Population_; }).ToList();
            var ListUsedRoom = (from usg in listUsingClassFiltered
                                select usg.roomID).ToList();

            List<ROOM> listUnusedRoom = listRoomFiltered.Where(x => {return !ListUsedRoom.Contains(x.RoomID); }).ToList();

            foreach(var room in listUnusedRoom)
            {
                usingClass.RoomID = room.RoomID;
                usingClass.StartPeriod = 1; // if we found a room that not used, just start at Tiet 1
                if(CheckOverlapUsingClass(usingClass, class_) == null)
                {
                    // we found an empty room and time, finish here
                    return;
                }
            }
        }

        /// <summary>
        /// return the first usingclass that overlap with using class parameter
        /// </summary>
        /// <param name="usingClass"></param>
        /// <param name="class_"></param>
        /// <returns></returns>
        static USINGCLASS CheckOverlapUsingClass(USINGCLASS usingClass, CLASS class_)
        {
            var startPeriod = (from period in DataProvider.Ins.DB.PERIOD_TIMERANGE
                               where period.PeriodID == usingClass.StartPeriod
                               select period).Single();

            var listUsingClassMayOverlap = (from usgClass in DataProvider.Ins.DB.USINGCLASSes
                                            join Class in DataProvider.Ins.DB.CLASSes on usgClass.ClassID equals Class.ClassID
                                            join strPeriod in DataProvider.Ins.DB.PERIOD_TIMERANGE on usgClass.StartPeriod equals strPeriod.PeriodID
                                            where usgClass.RoomID == usingClass.RoomID &&
                                                  usgClass.Day_ == usingClass.Day_ &&
                                                  !(Class.EndDate < class_.StartDate || Class.StartDate > class_.EndDate) &&
                                                  !(strPeriod.StartTime + usgClass.Duration < startPeriod.StartTime || strPeriod.StartTime > startPeriod.StartTime + usingClass.Duration)
                                            select new { usgClass, Class }).ToList();

            foreach(var usingClassMayOverlap in listUsingClassMayOverlap)
            {
                if (usingClass.RepeatCycle != usingClassMayOverlap.usgClass.RepeatCycle)
                {
                    return usingClassMayOverlap.usgClass;
                }
                // have the same repeat cycle but start after
                else if (((class_.StartDate - usingClassMayOverlap.Class.StartDate).Days / 7) % usingClass.RepeatCycle == 0)
                {
                    return usingClassMayOverlap.usgClass;
                }
            }

            return null;
        }

        #endregion
    }
}
