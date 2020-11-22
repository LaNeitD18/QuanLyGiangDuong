using QuanLyGiangDuong.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyGiangDuong.ViewModel
{
    public class RoomViewModel: BaseViewModel
    {
        public class RoomTableRow
        {
            private ROOM _room;
            public ROOM room
            {
                get
                {
                    if (_room == null)
                        _room = new ROOM();
                    return _room;
                }

                set
                {
                    _room = value;
                }
            }

            private PERIOD_TIMERANGE _period;
            public PERIOD_TIMERANGE period
            {
                get
                {
                    if (_period == null)
                        _period = new PERIOD_TIMERANGE();
                    return _period;
                }

                set
                {
                    _period = value;
                }
            }


            private List<IUsing> _usings;
            public List<IUsing> usings
            {
                get
                {
                    if(_usings == null)
                    {
                        _usings = new List<IUsing>();
                        for (int i = 0; i < 7; i++)
                        {
                            _usings.Add(null);
                        }
                    }
                    return _usings;
                }

                set
                {
                    _usings = value;
                }
            }

        }

        #region Day Combobox
        private int _selectedDay = -1;
        public int selectedDay
        {
            get
            {
                if (_selectedDay == -1)
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
                if (_dayList == null)
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
                if (_monthList == null)
                {
                    _monthList = new List<monthForComboBox>();
                    for (int i = 1; i < 13; i++)
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
                if (_selectedMonth == null)
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
                if (_yearList == null)
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
                if (_selectedYear == -1)
                {
                    _selectedYear = DateTime.Now.Year;
                }

                return _selectedYear;
            }
            set { _selectedYear = value; }
        }
        #endregion

        private ObservableCollection<RoomTableRow> _listRoomTableRow = new ObservableCollection<RoomTableRow>();
        public ObservableCollection<RoomTableRow> listRoomTableRow
        {
            get
            {
                return _listRoomTableRow;
            }
            set
            {
                _listRoomTableRow = value;
            }
        }

        private DateTime _startDate;
        public DateTime startDate
        {
            get
            {
                return _startDate;
            }

            set
            {
                _startDate = value;
                OnPropertyChanged("weekTitle");
            }
        }

        private DateTime _endDate;
        public DateTime endDate
        {
            get
            {
                return _endDate;
            }

            set
            {
                _endDate = value;
                OnPropertyChanged("weekTitle");
            }
        }

        private string _weekTitle;
        public string weekTitle
        {
            get
            {
                _weekTitle = _startDate.ToShortDateString() + " - " + _endDate.ToShortDateString();
                return _weekTitle;
            }

            set
            {
                _weekTitle = value;
            }
        }

        #region Get Schedule Button
        private ICommand _getRoomScheduleCommand;
        public ICommand GetRoomScheduleCommand
        {
            get
            {
                if (_getRoomScheduleCommand == null)
                {
                    _getRoomScheduleCommand = new RelayCommand(
                        param => this.GetRoomSchedule()
                    );
                }
                return _getRoomScheduleCommand;
            }
        }

        public void GetRoomSchedule()
        {
            startDate = new DateTime(selectedYear, selectedMonth.monthValue, selectedDay);
            startDate = startDate.AddDays(-((int)startDate.DayOfWeek + 6)%7);
            endDate = startDate.AddDays(6);

            listRoomTableRow.Clear();

            GetUsingClass();
            GetUsingEvent();
            GetUsingExam();
        }
        #endregion

        #region Get Using Functions

        public void GetUsingClass()
        {
            var usingClass = (from uc in DataProvider.Ins.DB.USINGCLASSes
                              join cl in DataProvider.Ins.DB.CLASSes on uc.ClassID equals cl.ClassID
                              join rm in DataProvider.Ins.DB.ROOMs on uc.RoomID equals rm.RoomID
                              where DbFunctions.AddDays(_startDate, (uc.Day_ + 6) % 7) > cl.StartDate &&   // Satisfy the start and end date of class
                                    DbFunctions.AddDays(_startDate, (uc.Day_ + 6) % 7) < cl.EndDate &&      // Satisfy the start and end date of class
                                    (DbFunctions.DiffDays(DbFunctions.AddDays(_startDate, (uc.Day_ + 6) % 7), cl.StartDate) / 7) % uc.RepeatCycle == 0// satisfied the repeat cycle
                              select new { uc, cl, rm }
                        ).ToList();

            foreach (var ins in usingClass)
            {
                if (!IsExistRoom(ins.rm))
                {
                    var listPeriod = (from period in DataProvider.Ins.DB.PERIOD_TIMERANGE
                                      where period.PeriodID != -1
                                      select period).ToList();
                    foreach (var period in listPeriod)
                    {
                        RoomTableRow roomTableRow = new RoomTableRow();
                        roomTableRow.room = ins.rm;
                        roomTableRow.period = period;

                        listRoomTableRow.Add(roomTableRow);
                    }
                }

                // for each using, we split it into multiple period row
                var listPeriodTimeRange = Utilities.Utils.GetListPeriodTimeRange(ins.uc.StartPeriod, ins.uc.Duration);

                foreach (var periodTimeRange in listPeriodTimeRange)
                {
                    if (IsExistRoomRow(ins.rm, periodTimeRange))
                    {
                        var roomTableRow = listRoomTableRow.First((x) => {
                            return x.room.RoomID == ins.rm.RoomID &&
                                   x.period.PeriodID == periodTimeRange.PeriodID;
                        });

                        roomTableRow.usings[ins.uc.Day_] = ins.uc;
                    }
                    else
                    {
                        RoomTableRow roomTableRow = new RoomTableRow();
                        roomTableRow.room = ins.rm;
                        roomTableRow.period = periodTimeRange;
                        roomTableRow.usings[ins.uc.Day_] = ins.uc;

                        listRoomTableRow.Add(roomTableRow);
                    }
                }
            }
        }
        public void GetUsingEvent()
        {
            var usingEvent = (from ev in DataProvider.Ins.DB.USINGEVENTs
                              join cl in DataProvider.Ins.DB.EVENT_ on ev.EventID equals cl.EventID
                              join rm in DataProvider.Ins.DB.ROOMs on ev.RoomID equals rm.RoomID
                              where ev.Date_ > startDate &&   // Satisfy the start and end date of choosen week
                                    ev.Date_ < endDate     // Satisfy the start and end date of choosen week
                              select new { ev, cl, rm }
                        ).ToList();

            foreach (var ins in usingEvent)
            {
                if (!IsExistRoom(ins.rm))
                {
                    var listPeriod = (from period in DataProvider.Ins.DB.PERIOD_TIMERANGE
                                      where period.PeriodID != -1
                                      select period).ToList();
                    foreach (var period in listPeriod)
                    {
                        RoomTableRow roomTableRow = new RoomTableRow();
                        roomTableRow.room = ins.rm;
                        roomTableRow.period = period;

                        listRoomTableRow.Add(roomTableRow);
                    }
                }

                // for each using, we split it into multiple period row
                var listPeriodTimeRange = Utilities.Utils.GetListPeriodTimeRange(ins.ev.StartPeriod, ins.ev.Duration);

                foreach (var periodTimeRange in listPeriodTimeRange)
                {
                    if (IsExistRoomRow(ins.rm, periodTimeRange))
                    {
                        var roomTableRow = listRoomTableRow.First((x) => {
                            return x.room.RoomID == ins.rm.RoomID &&
                                   x.period.PeriodID == periodTimeRange.PeriodID;
                        });

                        roomTableRow.usings[(int)ins.ev.Date_.DayOfWeek] = ins.ev;
                    }
                    else
                    {
                        RoomTableRow roomTableRow = new RoomTableRow();
                        roomTableRow.room = ins.rm;
                        roomTableRow.period = periodTimeRange;
                        roomTableRow.usings[(int)ins.ev.Date_.DayOfWeek] = ins.ev;

                        listRoomTableRow.Add(roomTableRow);
                    }
                }
            }
        }
        public void GetUsingExam()
        {
            var usingExam = (from ue in DataProvider.Ins.DB.USINGEXAMs
                              join rm in DataProvider.Ins.DB.ROOMs on ue.RoomID equals rm.RoomID
                              where ue.Date_ > startDate &&   // Satisfy the start and end date of choosen week
                                    ue.Date_ < endDate     // Satisfy the start and end date of choosen week
                              select new { ue, rm }
                        ).ToList();

            foreach (var ins in usingExam)
            {
                if (!IsExistRoom(ins.rm))
                {
                    var listPeriod = (from period in DataProvider.Ins.DB.PERIOD_TIMERANGE
                                      where period.PeriodID != -1
                                      select period).ToList();
                    foreach (var period in listPeriod)
                    {
                        RoomTableRow roomTableRow = new RoomTableRow();
                        roomTableRow.room = ins.rm;
                        roomTableRow.period = period;

                        listRoomTableRow.Add(roomTableRow);
                    }
                }

                // for each using, we split it into multiple period row
                var listPeriodTimeRange = Utilities.Utils.GetListPeriodTimeRange(ins.ue.StartPeriod, ins.ue.Duration);

                foreach (var periodTimeRange in listPeriodTimeRange)
                {
                    if (IsExistRoomRow(ins.rm, periodTimeRange))
                    {
                        var roomTableRow = listRoomTableRow.First((x) => {
                            return x.room.RoomID == ins.rm.RoomID &&
                                   x.period.PeriodID == periodTimeRange.PeriodID;
                        });

                        roomTableRow.usings[(int)ins.ue.Date_.DayOfWeek] = ins.ue;
                    }
                    else
                    {
                        RoomTableRow roomTableRow = new RoomTableRow();
                        roomTableRow.room = ins.rm;
                        roomTableRow.period = periodTimeRange;
                        roomTableRow.usings[(int)ins.ue.Date_.DayOfWeek] = ins.ue;

                        listRoomTableRow.Add(roomTableRow);
                    }
                }
            }
        }

        #endregion

        #region Next Week Button

        private ICommand _moveToNextWeekCommand;
        public ICommand moveToNextWeekCommand
        {
            get
            {
                if (_moveToNextWeekCommand == null)
                {
                    _moveToNextWeekCommand = new RelayCommand(
                        param => this.MoveToNextWeek()
                    );
                }
                return _moveToNextWeekCommand;
            }
        }

        public void MoveToNextWeek()
        {
            startDate = startDate.AddDays(7);
            endDate = endDate.AddDays(7);

            listRoomTableRow.Clear();

            GetUsingClass();
            GetUsingEvent();
            GetUsingExam();
        }

        #endregion

        #region Previous Week Button

        private ICommand _moveToPrevWeekCommand;
        public ICommand moveToPrevWeekCommand
        {
            get
            {
                if (_moveToPrevWeekCommand == null)
                {
                    _moveToPrevWeekCommand = new RelayCommand(
                        param => this.MoveToPrevWeek()
                    );
                }
                return _moveToPrevWeekCommand;
            }
        }

        public void MoveToPrevWeek()
        {
            startDate = startDate.AddDays(-7);
            endDate = endDate.AddDays(-7);

            listRoomTableRow.Clear();

            GetUsingClass();
            GetUsingEvent();
            GetUsingExam();
        }

        #endregion

        bool IsExistRoomRow(ROOM rm, PERIOD_TIMERANGE period)
        {
            foreach(var roomTableRow in listRoomTableRow)
            {
                if(roomTableRow.room.RoomID == rm.RoomID && roomTableRow.period.PeriodID == period.PeriodID)
                {
                    return true;
                }
            }
            return false;
        }

        bool IsExistRoom(ROOM rm)
        {
            foreach (var roomTableRow in listRoomTableRow)
            {
                if (roomTableRow.room.RoomID == rm.RoomID)
                {
                    return true;
                }
            }
            return false;
        }

        public RoomViewModel()
        {
            // Init startDate and endDate
            startDate = DateTime.Now;
            startDate = startDate.AddDays(-((int)startDate.DayOfWeek + 6) % 7); // get the start of the week
            endDate = startDate.AddDays(6); // get the end of the week

            GetUsingClass();
            GetUsingEvent();
            GetUsingExam();
        }
    }
}
