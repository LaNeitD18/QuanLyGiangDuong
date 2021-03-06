﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using QuanLyGiangDuong.Model;
using System.Data.Entity;

namespace QuanLyGiangDuong.ViewModel
{
    public class table
    {
        private string _roomID;
        public string roomID
        {
            get
            {
                return _roomID;
            }
            set { _roomID = value; }
        }

        /// <summary>
        /// Tuple:
        ///     first parameter: Using ID
        ///     second parameter: Using Display string
        /// </summary>
        private List<Tuple<string, string>> _tiet;
        
        public List<Tuple<string, string>> tiet
        {
            get
            {
                if (_tiet == null)
                {
                    _tiet = new List<Tuple<string, string>>();

                    for (int i = 0; i < 12; i++)
                        _tiet.Add(null);
                }

                return _tiet;
            }

            set { _tiet = value; }
        }

        public void Add(string UsingID, string UsingName, int StartPeriod, TimeSpan Duration)
        {
            var x = Utilities.Utils.GetListPeriodTimeRange(StartPeriod, Duration);

            for(int i=0; i<x.Count; i++)
            {
                tiet[x[i].PeriodID - 1] = new Tuple<string, string>(UsingID, UsingName);
            }
        }
    }

    public class monthForComboBox
    {
        private int _monthValue;
        public int monthValue
        {
            get { return _monthValue; }
            set { _monthValue = value; }
        }

        private string _monthStr;
        public string monthStr
        {
            get { return _monthStr; }
            set { _monthStr = value; }
        }
    }

    class TimeTableViewModel : ViewModelBase
    {
        private ObservableCollection<table> _tb;
        public ObservableCollection<table> tb
        {
            get
            {
                if (_tb == null)
                {
                    _tb = new ObservableCollection<table>();
                }

                return _tb;
            }

            set { _tb = value; OnPropertyChange("tb"); }
        }

        private table _selectedTB;

        public table selectedTB
        {
            get 
            {
                return _selectedTB;
            }

            set
            {
                _selectedTB = value;
            }
        }

        private string _selectedUsingClassID;
        public string selectedUsingClassID
        {
            get
            {
                return _selectedUsingClassID;
            }

            set
            {
                _selectedUsingClassID = value;
            }
        }

        private string _selectedDate;
        public string selectedDate
        {
            get
            {
                DateTime targetDate = new DateTime(selectedYear, selectedMonth.monthValue, selectedDay);
                string dayOfWeek;

                switch((int)targetDate.DayOfWeek)
                {
                    case 0: dayOfWeek = "Chủ nhật"; break;
                    default: dayOfWeek = "Thứ " + ((int)targetDate.DayOfWeek + 1).ToString(); break;
                }

                _selectedDate = dayOfWeek + " - ngày " + selectedDay.ToString() + "/" + selectedMonth.monthValue.ToString() + "/" + selectedYear.ToString();

                return _selectedDate;
            }

            set { _selectedDate = value; }
        }

        #region Day Combobox
        private int _selectedDay = -1;
        public int selectedDay
        {
            get
            {
                if(_selectedDay == -1)
                {
                    _selectedDay = DateTime.Now.Day;
                }
                return _selectedDay;
            }
            set { _selectedDay = value; }
        }

        private List<int> _dayList;
        public List<int> dayList
        {
            get
            {
                if(_dayList == null)
                {
                    _dayList = new List<int>();
                    for (int i = 1; i < 32; i++)
                    {
                        _dayList.Add(i);
                    }
                }
                return _dayList;
            }
            set { _dayList = value; }
        }
        #endregion
        #region Month Combobox
        private List<monthForComboBox> _monthList;
        public List<monthForComboBox> monthList
        {
            get
            {
                if(_monthList == null)
                {
                    _monthList = new List<monthForComboBox>();
                    for(int i=1; i<13; i++)
                    {
                        _monthList.Add(new monthForComboBox() { monthValue = i, monthStr = "Th" + i.ToString() });
                    }
                }
                return _monthList;
            }
            set { _monthList = value; }
        }

        private monthForComboBox _selectedMonth;
        public monthForComboBox selectedMonth
        {
            get
            {
                if(_selectedMonth == null)
                {
                    _selectedMonth = monthList.Where(item => item.monthValue == DateTime.Now.Month).First();
                }
                return _selectedMonth;
            }
            set
            {
                _selectedMonth = value;
            }
        }
        #endregion
        #region Year Combobox
        private List<int> _yearList;
        public List<int> yearList
        {
            get
            {
                if(_yearList == null)
                {
                    _yearList = new List<int>();
                    for (int i = DateTime.Now.Year - 1; i < DateTime.Now.Year + 2; i++)
                        _yearList.Add(i);
                }
                return _yearList;
            }
            set { _yearList = value; }
        }

        private int _selectedYear = -1;
        public int selectedYear
        {
            get
            {
                if(_selectedYear == -1)
                {
                    _selectedYear = DateTime.Now.Year;
                }

                return _selectedYear;
            }
            set { _selectedYear = value; }
        }
        #endregion

        private ICommand _getTimeTableCommand;
        public ICommand GetTimeTableCommand
        {
            get
            {
                if (_getTimeTableCommand == null)
                {
                    _getTimeTableCommand = new RelayCommand(
                        param => this.GetTimeTable()
                    );
                }
                return _getTimeTableCommand;
            }
        }

        bool IsExistRoom(string RoomID)
        {
            foreach(var row in tb)
            {
                if (row.roomID == RoomID)
                    return true;
            }
            return false;
        }

        table GetRoom(string RoomID)
        {
            foreach (var row in tb)
            {
                if (row.roomID == RoomID)
                    return row;
            }
            return null;
        }

        void GetUsingClass()
        {
            DateTime targetDate = new DateTime(selectedYear, selectedMonth.monthValue, selectedDay);

            // Select all needed info from USINGCLASS that fit selected Date
            var data = (from u in DataProvider.Ins.DB.USINGCLASSes
                        join c in DataProvider.Ins.DB.CLASSes on u.ClassID equals c.ClassID
                        where u.Status_ == (int)Enums.UsingStatus.Approved &&
                              targetDate >= u.StartDate && targetDate <= u.EndDate &&
                              (int)targetDate.DayOfWeek == u.Day_ && // satisfied the day of week
                              (DbFunctions.DiffDays(targetDate, c.StartDate) / 7) % u.RepeatCycle == 0 // satisfied the repeat cycle
                        select new { u.UsingClassID, u.RoomID, c.ClassName, u.StartPeriod, u.Duration }).ToList();

            data.Sort((a, b) => { return string.Compare(a.RoomID, b.RoomID); });

            foreach (var item in data)
            {
                if (IsExistRoom(item.RoomID))
                {
                    var room = GetRoom(item.RoomID);
                    room.Add(item.UsingClassID, item.ClassName, item.StartPeriod, item.Duration);
                }
                else
                {
                    var room = new table();
                    room.Add(item.UsingClassID, item.ClassName, item.StartPeriod, item.Duration);
                    room.roomID = item.RoomID;
                    tb.Add(room);
                }
            }
        }

        void GetUsingEvent()
        {
            DateTime targetDate = new DateTime(selectedYear, selectedMonth.monthValue, selectedDay);

            // Select all needed info from USINGCLASS that fit selected Date
            var data = (from u in DataProvider.Ins.DB.USINGEVENTs
                        join e in DataProvider.Ins.DB.EVENT_ on u.EventID equals e.EventID
                        where u.Date_ == targetDate &&
                              u.Status_ == (int)Enums.UsingStatus.Approved
                        select new { u.UsingEventID, u.RoomID, e.EventName, u.StartPeriod, u.Duration }).ToList();

            data.Sort((a, b) => { return string.Compare(a.RoomID, b.RoomID); });

            foreach (var item in data)
            {
                if (IsExistRoom(item.RoomID))
                {
                    var room = GetRoom(item.RoomID);
                    room.Add(item.UsingEventID, item.EventName, item.StartPeriod, item.Duration);
                }
                else
                {
                    var room = new table();
                    room.Add(item.UsingEventID, item.EventName, item.StartPeriod, item.Duration);
                    room.roomID = item.RoomID;
                    tb.Add(room);
                }
            }
        }

        void GetUsingExam()
        {
            DateTime targetDate = new DateTime(selectedYear, selectedMonth.monthValue, selectedDay);

            // Select all needed info from USINGCLASS that fit selected Date
            var data = (from u in DataProvider.Ins.DB.USINGEXAMs
                        join e in DataProvider.Ins.DB.EXAMs on u.ExamID equals e.ExamID
                        join c in DataProvider.Ins.DB.CLASSes on e.ClassID equals c.ClassID
                        where u.Date_ == targetDate &&
                              u.Status_ == (int)Enums.UsingStatus.Approved
                        select new { u.UsingExamID, u.RoomID, c.ClassName, u.StartPeriod, u.Duration }).ToList();

            data.Sort((a, b) => { return string.Compare(a.RoomID, b.RoomID); });

            foreach (var item in data)
            {
                if (IsExistRoom(item.RoomID))
                {
                    var room = GetRoom(item.RoomID);
                    room.Add(item.UsingExamID, "Thi: " + item.ClassName, item.StartPeriod, item.Duration);
                }
                else
                {
                    var room = new table();
                    room.Add(item.UsingExamID, "Thi: " + item.ClassName, item.StartPeriod, item.Duration);
                    room.roomID = item.RoomID;
                    tb.Add(room);
                }
            }
        }

        public void GetTimeTable()
        {
            DateTime targetDate;
            try 
            {
                targetDate = new DateTime(selectedYear, selectedMonth.monthValue, selectedDay);
            }
            catch(Exception e)
            {
                throw new Exception("Invalid Date");
            }

            OnPropertyChange("selectedDate");

            tb.Clear();

            GetUsingClass();
            GetUsingEvent();
            GetUsingExam();
        }
    }
}
