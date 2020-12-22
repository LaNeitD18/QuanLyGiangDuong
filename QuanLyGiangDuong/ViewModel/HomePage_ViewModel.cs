using QuanLyGiangDuong.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace QuanLyGiangDuong.ViewModel
{
    public class HomePage_ViewModel : BaseViewModel
    {
        #region Variables
        private string _CurrentDateTime;
        public string CurrentDateTime
        {
            get { return _CurrentDateTime; }
            set {
                if (_CurrentDateTime == value)
                    return;
                _CurrentDateTime = value; OnPropertyChanged(); 
            }
        }

        private int _EmptyRooms;
        public int EmptyRooms
        {
            get { return _EmptyRooms; }
            set { _EmptyRooms = value; OnPropertyChanged(); }
        }

        public DispatcherTimer _timer;

        private ObservableCollection<table> _tb;
        public ObservableCollection<table> tb
        {
            get
            {
                if (_tb == null)
                {
                    _tb = new ObservableCollection<table>();
                    GetTimeTable();
                }

                return _tb;
            }

            set { _tb = value; OnPropertyChanged("tb"); }
        }
        #endregion

        #region Functions
        private void GetTimeNow()
        {
            CurrentDateTime = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
            // Copy from https://stackoverflow.com/questions/31160201/time-ticking-in-c-sharp-wpf-mvvm
            _timer = new DispatcherTimer(DispatcherPriority.Render);
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (sender, args) =>
            {
                //Khởi tạo tài khoản cho toàn bộ chương trình
                //InitTaiKhoan();
                CurrentDateTime = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
            };
            _timer.Start();
        }

        private void InitTaiKhoan()
        {
            
        }

        bool IsExistRoom(string RoomID)
        {
            foreach (var row in tb)
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
            DateTime targetDate = DateTime.Now;

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
            DateTime targetDate = DateTime.Now;

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
            DateTime targetDate = DateTime.Now;

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
                targetDate = DateTime.Now;
            }
            catch (Exception e)
            {
                throw new Exception("Invalid Date");
            }

            tb.Clear();

            GetUsingClass();
            GetUsingEvent();
            GetUsingExam();
        }
        #endregion

        public HomePage_ViewModel()
        {
            GetTimeNow();

            // count empty rooms
            EmptyRooms = DataProvider.Ins.DB.ROOMs.Where(x => x.Status_ == "Còn sử dụng").Count();
        }
    }
}
