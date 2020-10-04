using System;
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

        public void Add(string ClassID, string ClassName, int StartPeriod, int EndPeriod)
        {
            for(int i=StartPeriod; i<EndPeriod+1; i++)
            {
                tiet[i-1] = new Tuple<string, string>(ClassID, ClassName);
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

        public void GetTimeTable()
        {
            tb.Clear();

            DateTime targetDate = new DateTime(selectedYear, selectedMonth.monthValue, selectedDay);
            var data = (from u in DataProvider.Ins.DB.USINGCLASSes
                        join c in DataProvider.Ins.DB.CLASSes on u.ClassID equals c.ClassID
                        where targetDate >= c.StartDate && targetDate <= c.EndDate &&
                              (int)targetDate.DayOfWeek == u.Day_ &&
                              (DbFunctions.DiffDays(targetDate, c.StartDate) / 7) % u.RepeatCycle == 0
                        select new { u.UsingClassID, u.RoomID, c.ClassName, u.StartPeriod, u.EndPeriod }).ToList();

            foreach(var item in data)
            {
                if(IsExistRoom(item.RoomID))
                {
                    var room = GetRoom(item.RoomID);
                    room.Add(item.UsingClassID, item.ClassName, item.StartPeriod, item.EndPeriod);
                }
                else
                {
                    var room = new table();
                    room.Add(item.UsingClassID, item.ClassName, item.StartPeriod, item.EndPeriod);
                    room.roomID = item.RoomID;
                    tb.Add(room);
                }
            }
        }
    }
}
