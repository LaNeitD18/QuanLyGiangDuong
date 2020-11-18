using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyGiangDuong.ViewModel
{
    public class RoomViewModel
    {
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
    }
}
