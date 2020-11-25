using QuanLyGiangDuong.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using QuanLyGiangDuong.ViewModel;
using System.Data.Entity.Core.Objects;

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

        /// <summary>
        /// Auto choose the Room and Start period for the usingclass
        /// 
        /// If successs, the Room and Start period inside the usingClass parameter will be modified 
        /// follow the result. Otherwise, every parameter stay unchanged.
        /// 
        /// </summary>
        /// <param name="usingClass"></param>
        /// <param name="class_"></param>
        /// <returns> 
        ///     return 0 if successs find the room and start period
        ///     otherwise, return -1;
        /// </returns>
        static public int AutoMakeSchedule(USINGCLASS usingClass, CLASS class_)
        {
            var beginningRoomID = usingClass.RoomID;
            var beginningStartPeriod = usingClass.StartPeriod;

            var listRoomFiltered = (from room in DataProvider.Ins.DB.ROOMs
                            where room.Capacity >= class_.Population_
                            select room).ToList();

            var targetDate = ((DateTime)usingClass.StartDate).AddDays(usingClass.Day_ - 1);

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

            foreach(var room in listRoomFiltered)
            {
                usingClass.RoomID = room.RoomID;
                usingClass.StartPeriod = 1; // if we found a room that not used, just start at Tiet 1

                USINGCLASS overlappedUsingClass = CheckOverlapUsingClass(usingClass, class_);
                USINGCLASS overlappedLecturerUsingClass = CheckOverlapLecturerTimeUsingClass(usingClass, class_);

                if (overlappedUsingClass == null && overlappedLecturerUsingClass == null)
                {
                    // we found an empty room and time, finish here
                    return 0;
                }
                else
                {
                    int newStartPeriod = CalcEndPeriod(overlappedUsingClass.StartPeriod, overlappedUsingClass.Duration) + 1;
                    int endPeriod = CalcEndPeriod(newStartPeriod, usingClass.Duration);

                    // loop through period to find the next fit in room on target date
                    while (endPeriod != -1)
                    {
                        if (newStartPeriod <= 5 && endPeriod > 5)
                        {
                            newStartPeriod = 6;
                            endPeriod = CalcEndPeriod(newStartPeriod, usingClass.Duration);
                        }

                        usingClass.StartPeriod = newStartPeriod;

                        overlappedUsingClass = CheckOverlapUsingClass(usingClass, class_);
                        overlappedLecturerUsingClass = CheckOverlapLecturerTimeUsingClass(usingClass, class_);

                        if (overlappedUsingClass == null && overlappedLecturerUsingClass == null)
                        {
                            return 0;
                        }

                        if(overlappedUsingClass == null)
                        {
                            newStartPeriod = CalcEndPeriod(overlappedLecturerUsingClass.StartPeriod, overlappedLecturerUsingClass.Duration) + 1;
                        }
                        else if (overlappedLecturerUsingClass == null || overlappedUsingClass.StartPeriod > overlappedLecturerUsingClass.StartPeriod)
                        {
                            newStartPeriod = CalcEndPeriod(overlappedUsingClass.StartPeriod, overlappedUsingClass.Duration) + 1;
                        }
                        else
                        {
                            newStartPeriod = CalcEndPeriod(overlappedLecturerUsingClass.StartPeriod, overlappedLecturerUsingClass.Duration) + 1;
                        }

                        endPeriod = CalcEndPeriod(newStartPeriod, usingClass.Duration);
                    }
                }
            }

            usingClass.RoomID = beginningRoomID;
            usingClass.StartPeriod = beginningStartPeriod;

            return -1;
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
                                            join StartPeriod in DataProvider.Ins.DB.PERIOD_TIMERANGE on usgClass.StartPeriod equals StartPeriod.PeriodID
                                            where usgClass.RoomID == usingClass.RoomID &&
                                                  usgClass.Day_ == usingClass.Day_
                                            select new { usgClass, Class, StartPeriod }).ToList();

            listUsingClassMayOverlap = listUsingClassMayOverlap.FindAll(x =>
                {
                    var cmcm = !(x.StartPeriod.StartTime + x.usgClass.Duration < startPeriod.StartTime || x.StartPeriod.StartTime > startPeriod.StartTime + usingClass.Duration);

                    return
                    !(x.usgClass.EndDate < usingClass.StartDate || x.usgClass.StartDate > usingClass.EndDate) &&
                    !(x.StartPeriod.StartTime + x.usgClass.Duration <= startPeriod.StartTime || x.StartPeriod.StartTime >= startPeriod.StartTime + usingClass.Duration);
                });

            

            foreach (var usingClassMayOverlap in listUsingClassMayOverlap)
            {
                if (usingClass.RepeatCycle != usingClassMayOverlap.usgClass.RepeatCycle)
                {
                    return usingClassMayOverlap.usgClass;
                }
                // have the same repeat cycle but start after
                else if ((((DateTime)usingClass.StartDate - (DateTime)usingClassMayOverlap.usgClass.StartDate).Days / 7) % usingClass.RepeatCycle == 0)
                {
                    return usingClassMayOverlap.usgClass;
                }
            }

            return null;
        }

        static USINGCLASS CheckOverlapLecturerTimeUsingClass(USINGCLASS usingClass, CLASS class_)
        {
            var startPeriod = (from period in DataProvider.Ins.DB.PERIOD_TIMERANGE
                               where period.PeriodID == usingClass.StartPeriod
                               select period).Single();

            var listUsingClassMayOverlap = (from usgClass in DataProvider.Ins.DB.USINGCLASSes
                                            join Class in DataProvider.Ins.DB.CLASSes on usgClass.ClassID equals Class.ClassID
                                            join StartPeriod in DataProvider.Ins.DB.PERIOD_TIMERANGE on usgClass.StartPeriod equals StartPeriod.PeriodID
                                            where Class.LecturerID == class_.LecturerID &&
                                                  usgClass.Day_ == usingClass.Day_
                                            select new { usgClass, Class, StartPeriod }).ToList();

            listUsingClassMayOverlap = listUsingClassMayOverlap.FindAll(x =>
            {
                var cmcm = !(x.StartPeriod.StartTime + x.usgClass.Duration < startPeriod.StartTime || x.StartPeriod.StartTime > startPeriod.StartTime + usingClass.Duration);

                return
                !(x.usgClass.EndDate < usingClass.StartDate || x.usgClass.StartDate > usingClass.EndDate) &&
                !(x.StartPeriod.StartTime + x.usgClass.Duration <= startPeriod.StartTime || x.StartPeriod.StartTime >= startPeriod.StartTime + usingClass.Duration);
            });



            foreach (var usingClassMayOverlap in listUsingClassMayOverlap)
            {
                if (usingClass.RepeatCycle != usingClassMayOverlap.usgClass.RepeatCycle)
                {
                    return usingClassMayOverlap.usgClass;
                }
                // have the same repeat cycle but start after
                else if ((((DateTime)usingClass.StartDate - (DateTime)usingClassMayOverlap.usgClass.StartDate).Days / 7) % usingClass.RepeatCycle == 0)
                {
                    return usingClassMayOverlap.usgClass;
                }
            }

            return null;
        }

        static public List<USINGEVENT> CheckOverLapEvent(USINGCLASS usingClass, CLASS class_)
        {
            List<USINGEVENT> listOverlappedEvent = new List<USINGEVENT>();

            var listEventMayOverlap = (from UsingEvent in DataProvider.Ins.DB.USINGEVENTs
                                       join StartPeriod in DataProvider.Ins.DB.PERIOD_TIMERANGE on UsingEvent.StartPeriod equals StartPeriod.PeriodID
                                       where UsingEvent.RoomID == usingClass.RoomID
                                       select new { UsingEvent, StartPeriod }).ToList();

            listEventMayOverlap = listEventMayOverlap.FindAll(x =>
            {
                int dayOfWeek = (int)x.UsingEvent.Date_.DayOfWeek;
                var startPeriod = (from period in DataProvider.Ins.DB.PERIOD_TIMERANGE
                                   where period.PeriodID == usingClass.StartPeriod
                                   select period).Single();

                return
                ((x.UsingEvent.Date_ - ((DateTime)usingClass.StartDate)).Days / 7) % usingClass.RepeatCycle == 0 &&
                dayOfWeek == usingClass.Day_ &&
                (x.UsingEvent.Date_ >= usingClass.StartDate && x.UsingEvent.Date_ <= usingClass.EndDate) &&
                !(x.StartPeriod.StartTime + x.UsingEvent.Duration <= startPeriod.StartTime || x.StartPeriod.StartTime >= startPeriod.StartTime + usingClass.Duration);
            });

            foreach(var event_ in listEventMayOverlap)
            {
                listOverlappedEvent.Add(event_.UsingEvent);
            }

            return listOverlappedEvent;
        }

        static public bool HandleOverlapEvent(USINGCLASS usingClass, CLASS class_)
        {
            List<USINGCLASS> listSplitedUsingClass = new List<USINGCLASS>();

            listSplitedUsingClass.Add(usingClass);

            var listOverlappedEvent = CheckOverLapEvent(usingClass, class_);

            if(listOverlappedEvent.Count > 3)
            {
                // NOT HANDLED YET
                return false;
            }
            else
            {
                var listRoomFiltered = (from room in DataProvider.Ins.DB.ROOMs
                                        where room.Capacity >= class_.Population_
                                        select room).ToList();

                listRoomFiltered.Reverse();

                listOverlappedEvent.OrderBy(x => x.Date_);

                foreach(var overlapEvent in listOverlappedEvent)
                {
                    // split the last using class
                    USINGCLASS targetUsingClass = listSplitedUsingClass[listSplitedUsingClass.Count - 1];
                    listSplitedUsingClass.RemoveAt(listSplitedUsingClass.Count - 1);

                    USINGCLASS firstHalf = new USINGCLASS(targetUsingClass);
                    USINGCLASS secondHalf = new USINGCLASS(targetUsingClass);

                    firstHalf.EndDate = overlapEvent.Date_.AddDays(-(int)overlapEvent.Date_.DayOfWeek);
                    secondHalf.StartDate = overlapEvent.Date_.AddDays(7 - ((int)overlapEvent.Date_.DayOfWeek - 1));

                    listSplitedUsingClass.Add(firstHalf);
                    listSplitedUsingClass.Add(secondHalf);
                }
            }

            return false;
        }

        #endregion
    }
}
