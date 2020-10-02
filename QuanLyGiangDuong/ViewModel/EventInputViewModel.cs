using QuanLyGiangDuong.Model;
using Syncfusion.GridCommon.ScrollAxis;
using Syncfusion.SfDataGrid.XForms;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLyGiangDuong.ViewModel
{
    class EventInputViewModel: BaseViewModel
    {
        #region Note
        // Event status should be in USING EVENT

        /*
        CREATE TABLE EVENT_
        (
            EventID VARCHAR(20) NOT NULL PRIMARY KEY,
            EventName NVARCHAR(MAX) NOT NULL,
            Status_ INT NOT NULL,
            Population_ INT NOT NULL,
            LecturerID VARCHAR(20) NOT NULL,
            Description_ NVARCHAR(MAX),
        )
        
        CREATE TABLE USINGEVENT
        (
            UsingEventID VARCHAR(20) NOT NULL PRIMARY KEY,
            RoomID VARCHAR(20) NOT NULL FOREIGN KEY REFERENCES ROOM(RoomID),
            EventID VARCHAR(20) NOT NULL FOREIGN KEY REFERENCES EVENT_(EventID),
            Date_ SMALLDATETIME NOT NULL,
            StartTime SMALLDATETIME NOT NULL,
            EndTime SMALLDATETIME NOT NULL,
            Description_ NVARCHAR(50),
        )
        */
        #endregion

        #region Fields
        #region Simple Fields
        private string _eventId = null;
        public string EventId
        {
            get => _eventId;
            set
            {
                _eventId = value;
                OnPropertyChanged();
            }
        }

        private string _eventName = null;
        public string EventName
        {
            get => _eventName;
            set 
            { 
                _eventName = value; 
                OnPropertyChanged();
            }
        }

        private string _lecturerId = null;
        public string LecturerId
        {
            get => _lecturerId;
            set
            {
                _lecturerId = value;
                OnPropertyChanged();
            }
        }

        private DateTime _dateOccurs;
        public DateTime DateOccurs
        {
            get => _dateOccurs;
            set
            {
                _dateOccurs = value;
                OnPropertyChanged();
            }
        }

        private int _population = 50;
        public int Population
        {
            get => _population;
            set
            {
                _population = value;
                OnPropertyChanged();
            }
        }

        private DateTime _fromTime;
        public DateTime FromTime
        {
            get => _fromTime;
            set
            {
                _fromTime = value;
                OnPropertyChanged();
            }
        }

        private DateTime _toTime;
        public DateTime ToTime
        {
            get => _toTime;
            set
            {
                _toTime = value;
                OnPropertyChanged();
            }
        }

        private string _description = null;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Combobox Room
        private BindingList<ROOM> _loadRooms()
        {
            BindingList<ROOM> result = new BindingList<ROOM>( DataProvider.Ins.DB.ROOMs.ToList() );
            return result;
        }

        private BindingList<ROOM> _listRoom = null;
        public BindingList<ROOM> ListRoom
        {
            get
            {
                if(_listRoom == null)
                    _listRoom = _loadRooms();
                return _listRoom;
            }
            set
            {
                _listRoom = value;
                OnPropertyChanged();
            }
        }

        private ROOM _selectedRoom = null;
        public ROOM SelectedRoom
        {
            get => _selectedRoom;
            set
            { 
                _selectedRoom = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #endregion

        #region Add button handler
        private ICommand _addEventToPendingListCmd = null;
        public ICommand AddEventToPendingListCmd
        {
            get
            {
                MessageBox.Show("Yen");
                if(_addEventToPendingListCmd == null)
                    _addEventToPendingListCmd = new RelayCommand(obj => _addEventToPendingList());
                return _addEventToPendingListCmd;
            }
            set
            {
                _addEventToPendingListCmd = value;
                OnPropertyChanged();
            }
        }

        private void _addEventToPendingList()
        {
            _printEventInfoTest();
        }

        private ICommand _cancelCmd = null;
        public ICommand CancelCmd
        {
            get
            {
                if (_cancelCmd == null)
                    _cancelCmd = new RelayCommand(obj => _cancel());
                return _cancelCmd;
            }
            set
            {
                _cancelCmd = value;
                OnPropertyChanged();
            }
        }

        private void _cancel()
        {
            _resetAllFields();
        }
        #endregion

        #region Utils
        private void _resetAllFields()
        {
            EventId = "";
            EventName = "";
            LecturerId = "";
            DateOccurs = DateTime.Today;
            Population = 50;
            FromTime = DateTime.Now;
            ToTime = DateTime.Now;
            SelectedRoom = null;
            Description = "";
        }

        private void _printEventInfoTest()
        {
            // CuteNote: TEST
            string toPrint = "";
            toPrint += "Event Id = " + EventId + "\n";
            toPrint += "Event Name = " + EventName + "\n";
            toPrint += "Lecturer = " + LecturerId + "\n";
            toPrint += "Date = " + DateOccurs.Date.ToString() + "\n";
            toPrint += "Population = " + Population.ToString() + "\n";
            toPrint += "From = " + FromTime.TimeOfDay.ToString() + "\n";
            toPrint += "To = " + ToTime.TimeOfDay.ToString() + "\n";
            toPrint += "Room ID = " + SelectedRoom.RoomID + "\n";
            toPrint += "Desc = " + Description + "\n";

            MessageBox.Show(toPrint);
        }
        #endregion

    }
}
