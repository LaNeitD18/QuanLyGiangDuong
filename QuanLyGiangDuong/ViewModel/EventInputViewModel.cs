using QuanLyGiangDuong.Model;
using Syncfusion.GridCommon.ScrollAxis;
using Syncfusion.SfDataGrid.XForms;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using QuanLyGiangDuong.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace QuanLyGiangDuong.ViewModel
{
    class EventInputViewModel: BaseViewModel
    {
        #region Note
        /*
        CREATE TABLE EVENT_
        (
            EventID VARCHAR(20) NOT NULL PRIMARY KEY,
            LecturerID VARCHAR(20) NOT NULL FOREIGN KEY REFERENCES LECTURER(LecturerID),
            EventName NVARCHAR(MAX) NOT NULL,
            Population_ INT NOT NULL,
            Description_ NVARCHAR(MAX),
        )
        CREATE TABLE USINGEVENT
        (
            UsingEventID VARCHAR(20) NOT NULL PRIMARY KEY,
            RoomID VARCHAR(20) NOT NULL FOREIGN KEY REFERENCES ROOM(RoomID),
            EventID VARCHAR(20) NOT NULL FOREIGN KEY REFERENCES EVENT_(EventID),
            Date_ SMALLDATETIME NOT NULL,
            StartPeriod INT NOT NULL FOREIGN KEY REFERENCES PERIOD_TIMERANGE(PeriodID),
            EndPeriod INT NOT NULL FOREIGN KEY REFERENCES PERIOD_TIMERANGE(PeriodID),
            Status_ INT NOT NULL,
            Description_ NVARCHAR(50),
        )
         */
        #endregion

        #region Fields

        #region Default Fields
        private string _defaultLecturerId = null;
        public string DefaultLecturerId
        {
            // CuteTN: current user ID, will be fixed later
            get
            {
                if(_defaultLecturerId == null)
                    _defaultLecturerId = "001";
                return _defaultLecturerId;
            }
        }

        private const string _nullEventIdPlaceholder = "[ Sự kiện mới ]";
        private const string _nullUsingEventIdPlaceholder = "[ Phiếu mượn mới ]";
        readonly private EVENT_ _nullEvent = new EVENT_{ EventID = _nullEventIdPlaceholder, };

        #endregion


        #region Simple Fields
        private string _usingEventId = null;
        public string UsingEventId
        {
            get => _usingEventId ?? _nullUsingEventIdPlaceholder;
            set
            {
                _usingEventId = value;
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

        private string _lecturerId = "001";
        public string LecturerId
        {
            get => _lecturerId;
            set
            {
                _lecturerId = value;
                OnPropertyChanged();
            }
        }

        private DateTime _dateOccurs = DateTime.Now;
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


        private bool _enableEdittingForm = false;

        public bool EnableEdittingForm
        { 
            get => _enableEdittingForm;
            set
            {
                _enableEdittingForm = value;
                OnPropertyChanged();
            }
        }
        #endregion


        #region Event list
        private BindingList<EVENT_> LoadEvents()
        {
            BindingList<EVENT_> result = new BindingList<EVENT_>(DataProvider.Ins.DB.EVENT_.ToList());
            
            // CuteTN note: null object pattern: add a fake-null EVENT_ to let user create new event
            result.Insert(0, _nullEvent);


            return result;
        }

        private BindingList<EVENT_> _listEvent = null;
        public BindingList<EVENT_> ListEvent
        {
            get
            {
                if (_listEvent == null)
                    _listEvent = LoadEvents();
                return _listEvent;
            }
            set
            {
                _listEvent = value;
                OnPropertyChanged();
            }
        }

        private EVENT_ _selectedEvent = null;
        public EVENT_ SelectedEvent
        {
            get
            {
                if(_selectedEvent == null)
                    _selectedEvent = _nullEvent;
                return _selectedEvent;
            }
            set
            {
                _selectedEvent = value ?? _nullEvent;
                EventName = _selectedEvent.EventName;
                OnPropertyChanged();
            }
        }
        #endregion


        #region Room List
        private BindingList<ROOM> LoadRooms()
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
                    _listRoom = LoadRooms();
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


        #region TimeRange List
        private BindingList<PERIOD_TIMERANGE> LoadPeriod_TimeRanges()
        {
            BindingList<PERIOD_TIMERANGE> result = new BindingList<PERIOD_TIMERANGE>(DataProvider.Ins.DB.PERIOD_TIMERANGE.ToList());
            return result;
        }

        private BindingList<PERIOD_TIMERANGE> _listTimeRange = null;
        public BindingList<PERIOD_TIMERANGE> ListTimeRange
        {
            get
            {
                if (_listTimeRange == null)
                    _listTimeRange = LoadPeriod_TimeRanges();
                return _listTimeRange;
            }
            set
            {
                _listTimeRange = value;
                OnPropertyChanged();
            }
        }

        private PERIOD_TIMERANGE _selectedStartTimeRange = null;
        public PERIOD_TIMERANGE SelectedStartTimeRange
        {
            get => _selectedStartTimeRange;
            set
            {
                _selectedStartTimeRange = value;
                OnPropertyChanged();
            }
        }

        private PERIOD_TIMERANGE _selectedEndTimeRange = null;
        public PERIOD_TIMERANGE SelectedEndTimeRange
        {
            get => _selectedEndTimeRange;
            set
            {
                _selectedEndTimeRange = value;
                OnPropertyChanged();
            }
        }
        #endregion


        #region datagrid using events
        private ObservableCollection<UsingEventModel> LoadUsingEvents()
        {
            ObservableCollection<UsingEventModel> result = new ObservableCollection<UsingEventModel>();
            
            foreach(var ue in DataProvider.Ins.DB.USINGEVENTs)
            {
                result.Add(new UsingEventModel(ue));
            }

            return result;
        }

        private ObservableCollection<UsingEventModel> _listUsingEvent = null;
        public ObservableCollection<UsingEventModel> ListUsingEvent
        {
            get
            {
                if(_listUsingEvent == null)
                    _listUsingEvent = LoadUsingEvents();
                return _listUsingEvent;
            }
            set
            {
                _listUsingEvent = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<UsingEventModel> _selectedUsingEvents = new ObservableCollection<UsingEventModel>();
        public ObservableCollection<UsingEventModel> SelectedUsingEvents
        {
            get => _selectedUsingEvents;
            set
            {
                SelectedUsingEvents = value;
                ListUsingEvent_OnSelectionChanged();
                OnPropertyChanged();
            }
        }

        private ICommand _listUsingEvent_SelectionChangedCmd = null;
        public ICommand ListUsingEvent_SelectionChangedCmd
        {
            get
            {
                if(_listUsingEvent_SelectionChangedCmd == null)
                    _listUsingEvent_SelectionChangedCmd = new RelayCommand(obj => ListUsingEvent_OnSelectionChanged(obj));
                return _listUsingEvent_SelectionChangedCmd;
            }
            set
            {
                _listUsingEvent_SelectionChangedCmd = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #endregion

        #region button handler

        #region confirm button
        private void HandleConfirmButtonClick()
        {
            // CuteTN: More code here...
            PrintEventInfoTest();
            AddEventToPendingList();
            Reset();
        }
        private ICommand _confirmCmd = null;
        public ICommand ConfirmCmd
        {
            get
            {
                if(_confirmCmd == null)
                    _confirmCmd = new RelayCommand(obj => HandleConfirmButtonClick());
                return _confirmCmd;
            }
            set
            {
                _confirmCmd = value;
                OnPropertyChanged();
            }
        }
        #endregion


        #region cancel button
        private void HandleCancelButton()
        {
            MessageBox.Show("cancel");
            Reset();
        }
        private ICommand _cancelCmd = null;
        public ICommand CancelCmd
        {
            get
            {
                if (_cancelCmd == null)
                    _cancelCmd = new RelayCommand(obj => HandleCancelButton());
                return _cancelCmd;
            }
            set
            {
                _cancelCmd = value;
                OnPropertyChanged();
            }
        }
        #endregion


        #region add button
        private void HandleAddButtonClick()
        {
            // CuteTN: More code here...
            MessageBox.Show("add");
            ChangeToAddState();
        }
        private ICommand _addCmd = null;
        public ICommand AddCmd
        {
            get
            {
                if (_addCmd == null)
                    _addCmd = new RelayCommand(obj => HandleAddButtonClick());
                return _addCmd;
            }
            set
            {
                _addCmd = value;
                OnPropertyChanged();
            }
        }
        #endregion


        #region edit button
        private void HandleEditButtonClick()
        {
            // CuteTN: More code here...
            MessageBox.Show("edit");
            ChangeToEditState();
        }

        private ICommand _editCmd = null;
        public ICommand EditCmd
        {
            get
            {
                if (_editCmd == null)
                    _editCmd = new RelayCommand(obj => HandleEditButtonClick());
                return _editCmd;
            }
            set
            {
                _editCmd = value;
                OnPropertyChanged();
            }
        }
        #endregion


        #region delete button
        private void HandleDeleteButtonClick()
        {
            // CuteTN: More code here...
            MessageBox.Show("delete");
        }
        private ICommand _deleteCmd = null;
        public ICommand DeleteCmd
        {
            get
            {
                if (_deleteCmd == null)
                    _deleteCmd = new RelayCommand(obj => HandleDeleteButtonClick());
                return _deleteCmd;
            }
            set
            {
                _deleteCmd = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #endregion

        #region Utils
        private void AddEventDB()
        {
            // CuteTN note:
            // why not using SelectedEvent directly?
            // heck, I fixed this from my old code, too lazy re-write this.
            EVENT_ e = DataProvider.Ins.DB.EVENT_.Find(SelectedEvent.EventID);
            bool isNewEvent = SelectedEvent.EventID == _nullEventIdPlaceholder;
            if(isNewEvent)
                e = new EVENT_();            

            e.EventName = EventName;
            e.LecturerID = LecturerId;
            e.Population_ = Population;
            e.Description_ = Description;

            if(isNewEvent)
            {
                e.EventID = Utils.GenerateStringId(DataProvider.Ins.DB.EVENT_);
                DataProvider.Ins.DB.EVENT_.Add(e);
            }

            // if found, e is a reference to an EVENT_ in model...
            // lets hope it changed, too...

            SaveDB();
            SelectedEvent = e;
        }

        private void AddUsingEventDB()
        {
            USINGEVENT ue;
            bool isNewUsingEvent = UsingEventId == _nullUsingEventIdPlaceholder;
            if (isNewUsingEvent)
            { 
                ue = new USINGEVENT();
            }
            else
            {
                ue = DataProvider.Ins.DB.USINGEVENTs.Find(UsingEventId);
            }
                
            ue.RoomID = SelectedRoom.RoomID;
            ue.EventID = SelectedEvent.EventID;
            ue.Date_ = DateOccurs;
            ue.StartPeriod = (int)SelectedStartTimeRange.PeriodID;
            ue.EndPeriod = (int)SelectedEndTimeRange.PeriodID;
            ue.Status_ = (int)Enums.UsingStatus.Pending;
            ue.Description_ = Description;

            if(isNewUsingEvent)
                DataProvider.Ins.DB.USINGEVENTs.Add(ue);

            SaveDB();
        }

        private void AddEventToPendingList()
        {
            AddEventDB();
            AddUsingEventDB();
        }

        private void ResetForm()
        {
            UsingEventId = null;
            SelectedEvent = null;
            EventName = "";
            LecturerId = DefaultLecturerId;
            DateOccurs = DateTime.Today;
            SelectedStartTimeRange = null;
            SelectedEndTimeRange = null;
            Population = 50;
            SelectedRoom = null;
            Description = "";
        }

        private void Reset()
        {
            ResetForm();
            RefreshData();
            EnableEdittingForm = false;
        }

        private void PrintEventInfoTest()
        {
            // CuteNote: TEST
            string toPrint = "";
            toPrint += "Event Id = " + SelectedEvent.EventID + "\n";
            toPrint += "Event Name = " + EventName + "\n";
            toPrint += "Lecturer = " + LecturerId + "\n";
            toPrint += "Date = " + DateOccurs.Date.ToString() + "\n";
            toPrint += "Population = " + Population.ToString() + "\n";
            toPrint += "From = " + SelectedStartTimeRange.PeriodName.ToString() + "\n";
            toPrint += "To = " + SelectedEndTimeRange.PeriodName.ToString() + "\n";
            toPrint += "Room ID = " + SelectedRoom?.RoomID + "\n";
            toPrint += "Desc = " + Description + "\n";

            MessageBox.Show(toPrint);
        }

        private void EnableEditingForm()
        {
            EnableEdittingForm = true;
        }

        private void DeleteSelectedRowsFromList()
        {
        }

        private void ChangeToAddState()
        {
            EnableEditingForm();
            ResetForm();
        }

        private void ChangeToEditState()
        {
            EnableEditingForm();
        }

        private void LoadContentToForm(USINGEVENT usingEvent)
        {
            UsingEventId = usingEvent.UsingEventID;
            SelectedEvent = usingEvent.EVENT_;
            EventName = usingEvent.EVENT_.EventName;
            LecturerId = usingEvent.EVENT_.LecturerID;
            DateOccurs = usingEvent.Date_;
            Population = usingEvent.EVENT_.Population_;
            SelectedStartTimeRange = usingEvent.PERIOD_TIMERANGE1;  // auto generated code, idk
            SelectedEndTimeRange = usingEvent.PERIOD_TIMERANGE;     // auto generated code, idk
            SelectedRoom = usingEvent.ROOM;
            Description = usingEvent.Description_;
        }

        private void GetDataGridSelectedItems(DataGrid datagrid)
        {
            SelectedUsingEvents.Clear();

            foreach(var item in datagrid.SelectedItems)
            {
                UsingEventModel ue = (UsingEventModel)item;
                if( ue != null && !SelectedUsingEvents.Contains(ue) )
                    SelectedUsingEvents.Add(ue);
            }
        }

        private void ListUsingEvent_OnSelectionChanged(Object obj = null)
        {
            DataGrid dataGrid = obj as DataGrid;
            if(dataGrid == null)
                return;

            GetDataGridSelectedItems(dataGrid);

            if(SelectedUsingEvents.Count == 1)
            {
                var selectedUE = SelectedUsingEvents[0];
                LoadContentToForm(selectedUE);
            }
            else
            {
                ResetForm();
            }
        }

        private void RefreshData()
        {
            ListEvent = LoadEvents();
            ListRoom = LoadRooms();
            ListTimeRange = LoadPeriod_TimeRanges();
            ListUsingEvent = LoadUsingEvents();
        }

        private void SaveDB()
        {
            DataProvider.Ins.DB.SaveChanges();
        }
        #endregion
    }
}
