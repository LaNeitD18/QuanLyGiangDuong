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
using System.Runtime.CompilerServices;

namespace QuanLyGiangDuong.ViewModel
{
    class EventInputViewModel: BaseViewModel
    {
        #region Fields

        #region Default Fields
        private string _defaultLecturerId = null;
        public string DefaultLecturerId
        {
            // CuteTN: current user ID, will be fixed later
            get
            {
                if(_defaultLecturerId == null)
                    _defaultLecturerId = "admin";
                return _defaultLecturerId;
            }
        }

        private const string _nullEventIdPlaceholder = "[ Sự kiện mới ]";
        private const string _nullUsingEventIdPlaceholder = "[ Phiếu mượn mới ]";
        readonly private EVENT_ _nullEvent = new EVENT_{ EventID = _nullEventIdPlaceholder, };

        #endregion


        #region Enabled bool
        private bool _isEdittingFormMode = false;
        public bool IsEdittingFormMode
        {
            get => _isEdittingFormMode;
            set
            {
                _isEdittingFormMode = value;
                UpdateEnabledViewElements();
                OnPropertyChanged();
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////// 
        /// all the boolean variables below doesnt actually have its own value
        /// it depends on the state of the form (Datagrid number of selected rows, and editting mode)
        
        public bool IsAddButtonEnabled
        { 
            get => (!IsEdittingFormMode);
            set => OnPropertyChanged();
        }

        public bool IsEditButtonEnabled
        {
            get => (!IsEdittingFormMode) && SelectedUsingEvents.Count == 1;
            set => OnPropertyChanged();
        }

        public bool IsDeleteButtonEnabled
        {
            get => (!IsEdittingFormMode) && SelectedUsingEvents.Count > 0;
            set => OnPropertyChanged();
        }

        public bool IsApproveButtonEnabled
        {
            get => (!IsEdittingFormMode) && SelectedUsingEvents.Count > 0;
            set => OnPropertyChanged();
        }

        public bool IsRejectButtonEnabled
        {
            get => (!IsEdittingFormMode) && SelectedUsingEvents.Count > 0;
            set => OnPropertyChanged();
        }

        /// <summary>
        /// we also have to invoke changes to view by calling enabled bools setter when needed.
        ///</summary> 
        private void UpdateEnabledViewElements()
        {
            IsAddButtonEnabled = true;
            IsEditButtonEnabled = true;
            IsDeleteButtonEnabled = true;
            IsApproveButtonEnabled = true;
            IsRejectButtonEnabled = true;
        }
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

        private int _duration = 50;
        public int Duration
        {
            get => _duration;
            set
            {
                _duration = value;
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
            get 
            { 
                if(_selectedRoom == null)
                    _selectedRoom = DataProvider.Ins.DB.ROOMs.Find(Utils.NullStringId);

                return _selectedRoom;
            }
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
            get 
            {
                if(_selectedStartTimeRange == null)
                    _selectedStartTimeRange = DataProvider.Ins.DB.PERIOD_TIMERANGE.Find(Utils.NullIntId);

                return _selectedStartTimeRange;
            }
            set
            {
                _selectedStartTimeRange = value;
                OnPropertyChanged();
            }
        }
        #endregion


        #region datagrid using events
        private ObservableCollection<USINGEVENT> LoadUsingEvents()
        {
            ObservableCollection<USINGEVENT> result = new ObservableCollection<USINGEVENT>
                (
                    DataProvider.Ins.DB.USINGEVENTs.Where(x => x.Status_ != (int)Enums.UsingStatus.Deleted).ToList()
                );

            return result;
        }

        private ObservableCollection<USINGEVENT> _listUsingEvent = null;
        public ObservableCollection<USINGEVENT> ListUsingEvent
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

        private ObservableCollection<USINGEVENT> _selectedUsingEvents = new ObservableCollection<USINGEVENT>();
        public ObservableCollection<USINGEVENT> SelectedUsingEvents
        {
            get => _selectedUsingEvents;
            set
            {
                _selectedUsingEvents = value;
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


        #region Lecturer list
        private BindingList<LECTURER> LoadLecturers()
        {
            BindingList<LECTURER> result = new BindingList<LECTURER>(DataProvider.Ins.DB.LECTURERs.ToList());
            return result;
        }

        private BindingList<LECTURER> _listLecturer = null;
        public BindingList<LECTURER> ListLecturer
        {
            get
            {
                if (_listLecturer == null)
                    _listLecturer = LoadLecturers();
                return _listLecturer;
            }
            set
            {
                _listLecturer = value;
                OnPropertyChanged();
            }
        }

        private LECTURER _selectedLecturer = null;
        public LECTURER SelectedLecturer
        {
            get
            {
                if (_selectedLecturer == null)
                    _selectedLecturer = DataProvider.Ins.DB.LECTURERs.Find(Utils.NullStringId);

                return _selectedLecturer;
            }
            set
            {
                _selectedLecturer = value;
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
            var errors = Validate();

            // if there was no error, print test, save DB and reset the form
            if(errors.Count == 0)
            { 
                // PrintEventInfoTest();
                AddEventToPendingList();
                Reset();
            }
            else
            { 
                string msg = errors.Aggregate((x, y) => x + "\n" + y);
                MessageBox.Show(msg);
            }
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
            var dlgRes = MessageBox.Show("Bạn có chắc muốn huỷ đăng ký sự kiện này không?", "Huỷ", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if(dlgRes == MessageBoxResult.Yes)
            { 
                Reset();
                LoadSelectedContentToForm();
            }
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
            var dlgRes = MessageBox.Show("Bạn có chắc muốn xoá các đăng ký sự kiện này không?", "Xoá", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (dlgRes == MessageBoxResult.Yes)
                SetStatusSelectedUsingEvents(Enums.UsingStatus.Deleted, x => true);
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

        #region approve button
        private void HandleApproveButtonClick()
        {
            var dlgRes = MessageBox.Show("Bạn có chắc muốn duyệt các đăng ký sự kiện này không?", "Duyệt", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (dlgRes == MessageBoxResult.Yes)
                SetStatusSelectedUsingEvents(Enums.UsingStatus.Approved, Utils.ValidateForApprove);
        }
        private ICommand _approveCmd = null;
        public ICommand ApproveCmd
        {
            get
            {
                if (_approveCmd == null)
                    _approveCmd = new RelayCommand(obj => HandleApproveButtonClick());
                return _approveCmd;
            }
            set
            {
                _approveCmd = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region reject button
        private void HandleRejectButtonClick()
        {
            var dlgRes = MessageBox.Show("Bạn có chắc muốn từ chối các đăng ký sự kiện này không?", "Từ chối", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (dlgRes == MessageBoxResult.Yes)
                SetStatusSelectedUsingEvents(Enums.UsingStatus.Rejected, x => true);
        }
        private ICommand _rejectCmd = null;
        public ICommand RejectCmd
        {
            get
            {
                if (_rejectCmd == null)
                    _rejectCmd = new RelayCommand(obj => HandleRejectButtonClick());
                return _rejectCmd;
            }
            set
            {
                _rejectCmd = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #endregion

        #region Utils


        /// <summary>
        /// return a list of strings to notify error message. if all the fields are valid, return an empty list
        /// </summary>
        /// <returns></returns>
        private List<string> Validate()
        {
            List<string> result = new List<string>();

            if(string.IsNullOrEmpty(EventName))
                result.Add("Vui lòng nhập tên sự kiện");

            if (Population <= 0)
                result.Add("Vui lòng nhập số người dự tính hợp lệ (số nguyên dương)");

            if(Duration <= 0)
                result.Add("Vui lòng nhập thời lượng hợp lệ (số nguyên dương)");

            return result;
        }

        /// <summary>
        /// Add NEW entry if the id has not exist. otherwise update the old entry
        /// Causion: this function does not validate data before saving
        /// </summary>
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
            e.LecturerID = SelectedLecturer.LecturerID;
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


        /// <summary>
        /// Add NEW entry if the id has not exist. otherwise update the old entry
        /// Causion: this function does not validate data before saving
        /// </summary>
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
            ue.Duration = TimeSpan.FromMinutes(Duration);
            ue.Status_ = (int)Enums.UsingStatus.Pending;
            ue.Description_ = Description;

            if(isNewUsingEvent)
            { 
                ue.UsingEventID = Utils.GenerateStringId(DataProvider.Ins.DB.USINGEVENTs);
                DataProvider.Ins.DB.USINGEVENTs.Add(ue);
            }

            SaveDB();
        }

        /// <summary>
        /// Add Event then Add Using Event.
        /// Causion: this function does not validate data before saving
        /// </summary>
        /// <returns></returns>
        private void AddEventToPendingList()
        {
            AddEventDB();
            AddUsingEventDB();
        }

        private void ResetForm()
        {
            UsingEventId = null;
            SelectedEvent = null;
            SelectedLecturer = null;
            DateOccurs = DateTime.Today;
            SelectedStartTimeRange = null;
            Duration = 45;
            Population = 50;
            SelectedRoom = null;
            Description = "";
        }

        private void Reset()
        {
            RefreshData();
            ResetForm();
            IsEdittingFormMode = false;
        }

        private void PrintEventInfoTest()
        {
            // CuteNote: TEST
            string toPrint = "";
            toPrint += "Event Id = " + SelectedEvent.EventID + "\n";
            toPrint += "Event Name = " + EventName + "\n";
            toPrint += "Lecturer = " + SelectedLecturer.LecturerName + "\n";
            toPrint += "Date = " + DateOccurs.Date.ToString() + "\n";
            toPrint += "Population = " + Population.ToString() + "\n";
            toPrint += "From = " + SelectedStartTimeRange.PeriodName.ToString() + "\n";
            toPrint += "Duration = " + Duration.ToString() + "\n";
            toPrint += "Room ID = " + SelectedRoom?.RoomID + "\n";
            toPrint += "Desc = " + Description + "\n";

            MessageBox.Show(toPrint);
        }

        private void EnableEditingForm()
        {
            IsEdittingFormMode = true;
        }

        private void SetStatusSelectedUsingEvents(Enums.UsingStatus newStatus, Func<USINGEVENT, bool> validateFunc)
        {
            foreach (var ue in SelectedUsingEvents)
            {
                if (validateFunc(ue))
                {
                    ue.Status_ = (int)newStatus;
                    SaveDB();
                }
            }

            ListUsingEvent = LoadUsingEvents();
        }


        private void ChangeToAddState()
        {
            ResetForm();
            EnableEditingForm();
        }

        private void ChangeToEditState()
        {
            EnableEditingForm();
        }

        private void LoadSelectedContentToForm()
        {
            if (SelectedUsingEvents.Count == 1)
            {
                var selectedUE = SelectedUsingEvents[0];
                LoadContentToForm(selectedUE);
            }
            else
            {
                ResetForm();
            }
        }

        /// <summary>
        /// Get selected item info from datagrid for previewing.
        /// </summary>
        /// <param name="usingEvent"></param>
        private void LoadContentToForm(USINGEVENT usingEvent)
        {
            UsingEventId = usingEvent.UsingEventID;
            SelectedEvent = usingEvent.EVENT_;
            EventName = usingEvent.EVENT_.EventName;
            SelectedLecturer = usingEvent.EVENT_.LECTURER;
            DateOccurs = usingEvent.Date_;
            Population = usingEvent.EVENT_.Population_;
            SelectedStartTimeRange = usingEvent.PERIOD_TIMERANGE;  
            Duration = (int)Math.Round(usingEvent.Duration.TotalMinutes);
            SelectedRoom = usingEvent.ROOM;
            Description = usingEvent.Description_;
        }

        private void GetDataGridSelectedItems(DataGrid datagrid)
        {
            SelectedUsingEvents.Clear();

            foreach(var item in datagrid.SelectedItems)
            {
                USINGEVENT ue = (USINGEVENT)item;
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
            UpdateEnabledViewElements();

            // update form content when user is not in editting mode
            if(!IsEdittingFormMode)
            { 
                LoadSelectedContentToForm();
            }
        }

        /// <summary>
        /// reload all list including comboboxes, datagrids data sources
        /// </summary>
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
