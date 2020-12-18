using QuanLyGiangDuong.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// </summary>
        /// <param name="usingClass"></param>
        /// <param name="class_"></param>
        /// <param name="selectedRoom">If selected room is null, function auto choose the room for you</param>
        /// <param name="selectedDayOfWeek">If selected day is null, function auto choose the day of week</param>
        /// <returns> 
        ///     If success, return list of new UsingClass that auto scheduled
        ///     Otherwise, return null
        /// </returns>
        static public List<USINGCLASS> AutoMakeSchedule(USINGCLASS usingClass, CLASS class_, ROOM selectedRoom, DayOfWeek? selectedDayOfWeek, Nullable<int> selectedStartPeriod)
        {
            List<int> listDayOfWeekToChoose = new List<int>();

            if(selectedDayOfWeek == null)
            {
                for (int i = 1; i <= 6; i++)
                {
                    listDayOfWeekToChoose.Add(i);
                }
            }
            else
            {
                listDayOfWeekToChoose.Add((int)selectedDayOfWeek);
            }
            

            foreach(int dayOfWeek in listDayOfWeekToChoose)
            {
                usingClass.Day_ = dayOfWeek;
                List<USINGCLASS> result;
                if(selectedStartPeriod == null)
                {
                    result = AutoMakeSchedule(usingClass, class_, selectedRoom);
                }
                else
                {
                    result = AutoMakeSchedule(usingClass, class_, selectedRoom, (int)selectedStartPeriod);
                }

                if(result != null)
                {
                    return result;
                }
            }


            return null;
        }

        /// <summary>
        /// Auto choose the Room and Start period for the usingclass
        /// 
        /// </summary>
        /// <param name="usingClass"></param>
        /// <param name="class_"></param>
        /// <param name="selectedRoom">If selected room is null, function auto choose the room for you</param>
        /// <returns> 
        ///     If success, return list of new UsingClass that auto scheduled
        ///     Otherwise, return null
        /// </returns>
        static private List<USINGCLASS> AutoMakeSchedule(USINGCLASS usingClass, CLASS class_, ROOM selectedRoom)
        {
            List<USINGCLASS> result = null;

            var beginningRoomID = usingClass.RoomID;
            var beginningStartPeriod = usingClass.StartPeriod;

            List<ROOM> listRoomFiltered;

            if (selectedRoom == null)
            {
                listRoomFiltered = (from room in DataProvider.Ins.DB.ROOMs
                                        where room.Capacity >= class_.Population_
                                        select room).ToList();
            }
            else if(selectedRoom.Capacity >= class_.Population_)
            {

                listRoomFiltered = new List<ROOM>();
                listRoomFiltered.Add(selectedRoom);
            }
            else
            {
                listRoomFiltered = new List<ROOM>();
            }

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
                usingClass.StartPeriod = 1;

                USINGCLASS overlappedUsingClass = CheckOverlapUsingClass(usingClass, class_);
                USINGCLASS overlappedLecturerUsingClass = CheckOverlapLecturerTimeUsingClass(usingClass, class_);
                List<USINGEVENT> listOverlappedEvent = new List<USINGEVENT>();

                if (overlappedUsingClass == null && overlappedLecturerUsingClass == null)
                {                        // we found an empty room and time, finish here
                    result = HandleOverlapEvent(usingClass, class_,out listOverlappedEvent);

                    if (result != null)
                    {
                        return result;
                    }
                }

                if(result == null)
                {
                    int newStartPeriod = 0;
                    int endPeriod = 0;

                    // loop through period to find the next fit in room on target date
                    while (endPeriod != -1)
                    {
                        if(overlappedUsingClass == null && overlappedLecturerUsingClass == null)
                        // if both are null, then the problem is with the event
                        {
                            listOverlappedEvent.Sort((x, y) =>
                            {
                                return Convert.ToInt32(CalcEndPeriod(x.StartPeriod, x.Duration) < CalcEndPeriod(y.StartPeriod, y.Duration));
                            });

                            newStartPeriod = CalcEndPeriod(listOverlappedEvent[0].StartPeriod, listOverlappedEvent[0].Duration) + 1;
                        }
                        else if (overlappedUsingClass == null)
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

                        if (endPeriod == -1) break;

                        if (newStartPeriod <= 5 && endPeriod > 5)
                        {
                            newStartPeriod = 6;
                            endPeriod = CalcEndPeriod(newStartPeriod, usingClass.Duration);
                        }
                        
                        if(endPeriod == -1) break;

                        usingClass.StartPeriod = newStartPeriod;

                        overlappedUsingClass = CheckOverlapUsingClass(usingClass, class_);
                        overlappedLecturerUsingClass = CheckOverlapLecturerTimeUsingClass(usingClass, class_);

                        if (overlappedUsingClass == null && overlappedLecturerUsingClass == null)
                        {
                            result = HandleOverlapEvent(usingClass, class_,out listOverlappedEvent);

                            if (result != null)
                            {
                                return result;
                            }
                        }
                    }
                }
            }

            usingClass.RoomID = beginningRoomID;
            usingClass.StartPeriod = beginningStartPeriod;

            return result;
        }

        static private List<USINGCLASS> AutoMakeSchedule(USINGCLASS usingClass, CLASS class_, ROOM selectedRoom, int selectedStartPeriod)
        {
            if(CalcEndPeriod(selectedStartPeriod, usingClass.Duration) > 5 && selectedStartPeriod <= 5 ||
                CalcEndPeriod(selectedStartPeriod, usingClass.Duration) == -1)
            {
                return null;
            }

            List<USINGCLASS> result = null;

            var beginningRoomID = usingClass.RoomID;
            var beginningStartPeriod = usingClass.StartPeriod;

            List<ROOM> listRoomFiltered;

            if (selectedRoom == null)
            {
                listRoomFiltered = (from room in DataProvider.Ins.DB.ROOMs
                                        where room.Capacity >= class_.Population_
                                        select room).ToList();
            }
            else if(selectedRoom.Capacity >= class_.Population_)
            {

                listRoomFiltered = new List<ROOM>();
                listRoomFiltered.Add(selectedRoom);
            }
            else
            {
                listRoomFiltered = new List<ROOM>();
            }

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

            foreach (var room in listRoomFiltered)
            {
                usingClass.RoomID = room.RoomID;
                usingClass.StartPeriod = selectedStartPeriod;

                USINGCLASS overlappedUsingClass = CheckOverlapUsingClass(usingClass, class_);
                USINGCLASS overlappedLecturerUsingClass = CheckOverlapLecturerTimeUsingClass(usingClass, class_);
                List<USINGEVENT> listOverlappedEvent = new List<USINGEVENT>();

                if (overlappedUsingClass == null && overlappedLecturerUsingClass == null)
                {
                    // we found an empty room and time, finish here
                    result = HandleOverlapEvent(usingClass, class_, out listOverlappedEvent);

                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }


        static public USINGEXAM AutoMakeExam(USINGEXAM usingExam, EXAM exam, DateTime startdate, DateTime endDate)
        {
            DateTime selectedDate = startdate;
            List<ROOM> listRoomFiltered = (from r in DataProvider.Ins.DB.ROOMs
                                           where r.Capacity >= exam.Population_
                                           orderby r.Capacity ascending
                                           select r).ToList();
            
            while(selectedDate <= endDate)
            {
                if(selectedDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    foreach (ROOM room in listRoomFiltered)
                    {
                        usingExam.Date_ = selectedDate;
                        usingExam.StartPeriod = 1;
                        usingExam.RoomID = room.RoomID;

                        while (CalcEndPeriod(usingExam.StartPeriod, usingExam.Duration) != -1)
                        {
                            var listOverlapExam = CheckOverlapExam(usingExam);
                            var listOverlapEvent = CheckOverlapEvent(usingExam);

                            int maxOverlapEndPeriod = 0;

                            if (listOverlapEvent == null && listOverlapExam == null)
                            {
                                return usingExam;
                            }

                            if (listOverlapExam != null)
                            {
                                foreach (var overlapExam in listOverlapExam)
                                {
                                    int overlapEndPeriod = CalcEndPeriod(overlapExam.StartPeriod, overlapExam.Duration);

                                    if (maxOverlapEndPeriod < overlapEndPeriod)
                                    {
                                        maxOverlapEndPeriod = overlapEndPeriod;
                                    }
                                }
                            }

                            if (listOverlapEvent != null)
                            {
                                foreach (var overlapEvent in listOverlapEvent)
                                {
                                    int overlapEndPeriod = CalcEndPeriod(overlapEvent.StartPeriod, overlapEvent.Duration);

                                    if (maxOverlapEndPeriod < overlapEndPeriod)
                                    {
                                        maxOverlapEndPeriod = overlapEndPeriod;
                                    }
                                }
                            }

                            int newStartPeriod = maxOverlapEndPeriod + 1;
                            int newEndperiod = CalcEndPeriod(newStartPeriod, usingExam.Duration);

                            if (newStartPeriod <= 5 && newEndperiod > 5)
                            {
                                newStartPeriod = 6;
                            }

                            usingExam.StartPeriod = newStartPeriod;

                        }
                    }
                }

                selectedDate = selectedDate.AddDays(1);
            }

            return null;
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

        static USINGCLASS CheckOverlapApprovedUsingClass(USINGCLASS usingClass, CLASS class_)
        {
            var startPeriod = (from period in DataProvider.Ins.DB.PERIOD_TIMERANGE
                               where period.PeriodID == usingClass.StartPeriod
                               select period).Single();

            var listUsingClassMayOverlap = (from usgClass in DataProvider.Ins.DB.USINGCLASSes
                                            join Class in DataProvider.Ins.DB.CLASSes on usgClass.ClassID equals Class.ClassID
                                            join StartPeriod in DataProvider.Ins.DB.PERIOD_TIMERANGE on usgClass.StartPeriod equals StartPeriod.PeriodID
                                            where usgClass.RoomID == usingClass.RoomID &&
                                                  usgClass.Day_ == usingClass.Day_ &&
                                                  usgClass.Status_ == (int)Enums.UsingStatus.Approved
                                            select new { usgClass, Class, StartPeriod }).ToList();

            listUsingClassMayOverlap = listUsingClassMayOverlap.FindAll(x =>
                {
                    // var cmcm = !(x.StartPeriod.StartTime + x.usgClass.Duration < startPeriod.StartTime || x.StartPeriod.StartTime > startPeriod.StartTime + usingClass.Duration);

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

        static USINGCLASS CheckOverlapApprovedLecturerTimeUsingClass(USINGCLASS usingClass, CLASS class_)
        {
            var startPeriod = (from period in DataProvider.Ins.DB.PERIOD_TIMERANGE
                               where period.PeriodID == usingClass.StartPeriod
                               select period).Single();

            var listUsingClassMayOverlap = (from usgClass in DataProvider.Ins.DB.USINGCLASSes
                                            join Class in DataProvider.Ins.DB.CLASSes on usgClass.ClassID equals Class.ClassID
                                            join StartPeriod in DataProvider.Ins.DB.PERIOD_TIMERANGE on usgClass.StartPeriod equals StartPeriod.PeriodID
                                            where Class.LecturerID == class_.LecturerID &&
                                                  usgClass.Day_ == usingClass.Day_ &&
                                                  usgClass.Status_ == (int)Enums.UsingStatus.Approved
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

        static public List<USINGEVENT> CheckOverlapApprovedEvent(USINGCLASS usingClass, CLASS class_)
        {
            List<USINGEVENT> listOverlappedEvent = new List<USINGEVENT>();

            var listEventMayOverlap = (from UsingEvent in DataProvider.Ins.DB.USINGEVENTs
                                       join StartPeriod in DataProvider.Ins.DB.PERIOD_TIMERANGE on UsingEvent.StartPeriod equals StartPeriod.PeriodID
                                       where UsingEvent.RoomID == usingClass.RoomID &&
                                             UsingEvent.Status_ == (int)Enums.UsingStatus.Approved
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

        static public List<USINGEXAM> CheckOverlapExam(USINGEXAM usingExam)
        {
            var listExamMayOverlap = (from ue in DataProvider.Ins.DB.USINGEXAMs
                                      where ue.RoomID == usingExam.RoomID &&
                                            ue.Date_ == usingExam.Date_
                                      select ue).ToList();

            List<USINGEXAM> listExamOverlap = listExamMayOverlap.FindAll(x =>
            {
                var ueStartTime = DataProvider.Ins.DB.PERIOD_TIMERANGE.Find(usingExam.StartPeriod).StartTime;
                var overlapStartTime = DataProvider.Ins.DB.PERIOD_TIMERANGE.Find(x.StartPeriod).StartTime;

                if(ueStartTime + usingExam.Duration <= overlapStartTime ||
                   ueStartTime >= overlapStartTime + x.Duration)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            });

            if (listExamOverlap.Count == 0) return null;

            return listExamOverlap;
        }

        static public List<USINGEVENT> CheckOverlapEvent(USINGEXAM usingExam)
        {
            var listEventMayOverlap = (from ue in DataProvider.Ins.DB.USINGEVENTs
                                      where ue.RoomID == usingExam.RoomID &&
                                            ue.Date_ == usingExam.Date_
                                      select ue).ToList();

            List<USINGEVENT> listExamOverlap = listEventMayOverlap.FindAll(x =>
            {
                var ueStartTime = DataProvider.Ins.DB.PERIOD_TIMERANGE.Find(usingExam.StartPeriod).StartTime;
                var overlapStartTime = DataProvider.Ins.DB.PERIOD_TIMERANGE.Find(x.StartPeriod).StartTime;

                if(ueStartTime + usingExam.Duration <= overlapStartTime ||
                   ueStartTime >= overlapStartTime + x.Duration)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            });

            if (listExamOverlap.Count == 0) return null;

            return listExamOverlap;
        }

        /// <summary>
        /// This function split 1 using class into multiple using class
        /// so that it cannot overlap with event
        /// </summary>
        /// <param name="usingClass"></param>
        /// <param name="class_"></param>
        /// <returns></returns>
        static public List<USINGCLASS> HandleOverlapEvent(USINGCLASS usingClass, CLASS class_, out List<USINGEVENT> listOverlappedEvent)
        {
            List<USINGCLASS> listSplitedUsingClass = new List<USINGCLASS>();

            listSplitedUsingClass.Add(usingClass);

            listOverlappedEvent = CheckOverLapEvent(usingClass, class_);


            if (listOverlappedEvent.Count > 3)
            {
                // NOT HANDLED YET
                return null;
            }
            else
            {
                var listRoomFiltered = (from room in DataProvider.Ins.DB.ROOMs
                                        where room.Capacity >= class_.Population_
                                        select room).ToList();

                listRoomFiltered.Reverse();

                listOverlappedEvent.Sort((x, y) => { return Convert.ToInt32(x.Date_ > y.Date_); });

                foreach (var overlapEvent in listOverlappedEvent)
                {
                    // split the last using class
                    USINGCLASS targetUsingClass = listSplitedUsingClass[listSplitedUsingClass.Count - 1];
                    listSplitedUsingClass.RemoveAt(listSplitedUsingClass.Count - 1);

                    USINGCLASS firstHalf = new USINGCLASS(targetUsingClass);
                    USINGCLASS secondHalf = new USINGCLASS(targetUsingClass);

                    firstHalf.EndDate = overlapEvent.Date_.AddDays(-(int)overlapEvent.Date_.DayOfWeek);
                    secondHalf.StartDate = overlapEvent.Date_.AddDays(7 - ((int)overlapEvent.Date_.DayOfWeek - 1));

                    USINGCLASS middle = new USINGCLASS(targetUsingClass);
                    middle.StartDate = ((DateTime)firstHalf.EndDate).AddDays(1);
                    middle.EndDate = ((DateTime)secondHalf.StartDate).AddDays(-1);

                    bool middleFound = false;

                    foreach (var room in listRoomFiltered)
                    {
                        middle.RoomID = room.RoomID;

                        var overlapUsingClass = CheckOverlapUsingClass(middle, class_);

                        var listOverlapEvent = CheckOverLapEvent(middle, class_);

                        if (overlapUsingClass == null && listOverlapEvent.Count == 0)
                        {
                            listSplitedUsingClass.Add(firstHalf);
                            listSplitedUsingClass.Add(middle);
                            listSplitedUsingClass.Add(secondHalf);

                            middleFound = true;

                            break;
                        }
                    }

                    if (middleFound == false)
                    {
                        return null;
                    }
                }
            }

            return listSplitedUsingClass;
        }
        #endregion

        #region data that is int for db but string for UI
        /// <summary>
        /// using keyvalue pair is actually the same, but wrapping this would make the code more readable
        /// </summary>
        public class IdNamePair<IdType>
        {
            private KeyValuePair<IdType, string> data;

            public IdNamePair(IdType id, string name)
            {
                data = new KeyValuePair<IdType, string>(id, name);
            }

            public IdType Id { get => data.Key; }
            public string Name { get => data.Value; }
        }

        static private BindingList<IdNamePair<int>> _semesters = null;
        static public BindingList<IdNamePair<int>> Semesters
        {
            get
            {
                if(_semesters == null)
                {
                    _semesters = new BindingList<IdNamePair<int>>();

                    _semesters.Add(new IdNamePair<int>(1, "Học kỳ 1"));
                    _semesters.Add(new IdNamePair<int>(2, "Học kỳ 2"));
                    _semesters.Add(new IdNamePair<int>(3, "Học kỳ hè"));
                }

                return _semesters;
            }
        }

        static private BindingList<IdNamePair<int>> _schoolYears;
        static public BindingList<IdNamePair<int>> SchoolYears
        {
            get
            {
                if(_schoolYears == null)
                {
                    _schoolYears = new BindingList<IdNamePair<int>>();

                    // CuteTN Note: a const for 2000 maybe better...
                    for(int i = DateTime.Now.Year; i >= 2000; i--)
                    {
                        _schoolYears.Add(new IdNamePair<int>(i, $"{i} - {i+1}"));
                    }
                }

                return _schoolYears;
            }
        }

        static private BindingList<IdNamePair<int>> _daysOfWeek = null;
        static public BindingList<IdNamePair<int>> DaysOfWeek
        {
            get
            {
                if(_daysOfWeek == null)
                {
                    _daysOfWeek = new BindingList<IdNamePair<int>>();

                    _daysOfWeek.Add(new IdNamePair<int>(NullIntId, "[Tự động]"));
                    _daysOfWeek.Add(new IdNamePair<int>((int)DayOfWeek.Monday, "Thứ hai"));
                    _daysOfWeek.Add(new IdNamePair<int>((int)DayOfWeek.Tuesday, "Thứ ba"));
                    _daysOfWeek.Add(new IdNamePair<int>((int)DayOfWeek.Wednesday, "Thứ tư"));
                    _daysOfWeek.Add(new IdNamePair<int>((int)DayOfWeek.Thursday, "Thứ năm"));
                    _daysOfWeek.Add(new IdNamePair<int>((int)DayOfWeek.Friday, "Thứ sáu"));
                    _daysOfWeek.Add(new IdNamePair<int>((int)DayOfWeek.Saturday, "Thứ bảy"));
                    _daysOfWeek.Add(new IdNamePair<int>((int)DayOfWeek.Sunday, "Chủ nhật"));
                }

                return _daysOfWeek;
            }
        }

        /// <summary>
        /// return the element with a specified Id from a bindinglist of IdNamePair
        /// </summary>
        /// <typeparam name="IdType"></typeparam>
        /// <param name=""></param>
        /// <returns></returns>
        static public IdNamePair<IdType> GetElementById<IdType>(BindingList<IdNamePair<IdType>> items, IdType id)
        {
            try
            { 
                return items.Where(x => x.Id.Equals(id)).ElementAt(0);
            }
            catch
            {
                return null;
            }
        }

        static public IdNamePair<IdType> GetElementByName<IdType>(BindingList<IdNamePair<IdType>> items, string name, bool caseSensitive = false)
        {
            try
            {
                return items.Where
                    (
                        x => 
                        { 
                            if(caseSensitive)
                                return x.Name == name;
                            else
                                return x.Name.ToUpper() == name.ToUpper();
                        }

                    ).ElementAt(0);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region print test
        static public string  Convert2DListToString(List<List<string>> list)
        {
            string result = list.Select
                (
                    x => x.Aggregate((s1, s2) => s1 + "\t\t" + s2)
                ).Aggregate((s1, s2) => s1 + "\n" + s2);

            return result;
        }

        #endregion

        #region validate for changing using rooms status
        public static bool ValidateForApprove(USINGEVENT usingEvent)
        {
            DateTime dateOccurs = usingEvent.Date_;
            ROOM room = usingEvent.ROOM;

            var listPeriod = GetListPeriodTimeRange(usingEvent.StartPeriod, usingEvent.Duration);

            TimeTableViewModel ttvm = new TimeTableViewModel();
            ttvm.selectedDay = dateOccurs.Day;
            ttvm.selectedMonth.monthValue = dateOccurs.Month;
            ttvm.selectedYear = dateOccurs.Year;

            ttvm.GetTimeTable();

            var TimeTable = ttvm.tb;

            if(TimeTable.Where(x => x.roomID == room.RoomID).ToList().Count == 0) return true;

            table TimeTableRoom = TimeTable.Where(x =>  x.roomID == room.RoomID).First();

            foreach(var period in listPeriod)
            {
                if(TimeTableRoom.tiet[period.PeriodID - 1] != null)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ValidateForApprove(USINGEXAM usingExam)
        {
            // dry DRY DRYYYYYYYYYYYYYYY
            USINGEVENT ue = new USINGEVENT();
            ue.Date_ = usingExam.Date_;
            ue.RoomID = usingExam.RoomID;

            ue.StartPeriod = usingExam.StartPeriod;
            ue.Duration = usingExam.Duration;

            return ValidateForApprove(ue);
        }

        public static bool ValidateForApprove(USINGCLASS usingClass)
        {
            CLASS class_ = usingClass.CLASS;

            if(CheckOverlapApprovedUsingClass(usingClass, class_) == null &&
               CheckOverlapApprovedLecturerTimeUsingClass(usingClass, class_) == null &&
               CheckOverlapApprovedEvent(usingClass, class_).Count == 0)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}