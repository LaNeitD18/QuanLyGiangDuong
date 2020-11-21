using QuanLyGiangDuong.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyGiangDuong.ViewModel
{
    public class RoomViewModel
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

            listRoomTableRow.Clear();

            var data = (from uc in DataProvider.Ins.DB.USINGCLASSes
                        join cl in DataProvider.Ins.DB.CLASSes on uc.ClassID equals cl.ClassID
                        join rm in DataProvider.Ins.DB.ROOMs on uc.RoomID equals rm.RoomID
                        select new { uc, cl, rm }
                        ).ToList();

            foreach(var ins in data)
            {
                if(!IsExistRoom(ins.rm))
                {
                    var listPeriod = (from period in DataProvider.Ins.DB.PERIOD_TIMERANGE
                                      where period.PeriodID != -1
                                      select period).ToList();
                    foreach(var period in listPeriod)
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
                        var roomTableRow = listRoomTableRow.First((x) => { return x.room.RoomID == ins.rm.RoomID && 
                                                                                  x.period.PeriodID == periodTimeRange.PeriodID; });

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
    }
}
