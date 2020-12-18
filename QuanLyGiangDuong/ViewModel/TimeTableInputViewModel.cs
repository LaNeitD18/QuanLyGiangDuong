using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Input;
using QuanLyGiangDuong.Model;
using QuanLyGiangDuong.Utilities;
using System.Globalization;
using System.Data.Entity.Migrations;

namespace QuanLyGiangDuong.ViewModel
{
    class TimeTableInputViewModel: BaseViewModel
    {
        #region Fields

        #region Default Fields
        private string _defaultLecturerId = null;
        public string DefaultLecturerId
        {
            // CuteTN: current user ID, will be fixed later
            get
            {
                if (_defaultLecturerId == null)
                    _defaultLecturerId = "001";
                return _defaultLecturerId;
            }
        }

        private const string _nullClassIdPlaceholder = "[ Lớp học mới ]";
        private const string _nullUsingClassIdPlaceholder = "[ Phiếu mượn mới ]";
        readonly private CLASS _nullClass = new CLASS { ClassID = _nullClassIdPlaceholder, };

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
            get => (!IsEdittingFormMode) && SelectedUsingClasses.Count == 1;
            set => OnPropertyChanged();
        }

        public bool IsDeleteButtonEnabled
        {
            get => (!IsEdittingFormMode) && SelectedUsingClasses.Count > 0;
            set => OnPropertyChanged();
        }

        public bool IsApproveButtonEnabled
        {
            get => (!IsEdittingFormMode) && SelectedUsingClasses.Count > 0;
            set => OnPropertyChanged();
        }

        public bool IsRejectButtonEnabled
        {
            get => (!IsEdittingFormMode) && SelectedUsingClasses.Count > 0;
            set => OnPropertyChanged();
        }

        public bool IsReadExcelButtonEnabled
        {
            get => (!IsEdittingFormMode);
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
            IsReadExcelButtonEnabled = true;
        }
        #endregion

        #region Simple Fields
        private string _usingClassId = null;
        public string UsingClassId
        {
            get => _usingClassId ?? _nullUsingClassIdPlaceholder;
            set
            {
                _usingClassId = value;
                OnPropertyChanged();
            }
        }

        private string _className = null;
        public string ClassName
        {
            get => _className;
            set
            {
                _className = value;
                OnPropertyChanged();
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(); }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set { _endDate = value; OnPropertyChanged(); }
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

        private int _duration = 90;
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

        #region Class list
        private BindingList<CLASS> LoadClasses()
        {
            BindingList<CLASS> result = new BindingList<CLASS>(DataProvider.Ins.DB.CLASSes.ToList());

            // CuteTN note: null object pattern: add a fake-null EVENT_ to let user create new event
            result.Insert(0, _nullClass);


            return result;
        }

        private BindingList<CLASS> _listClass = null;
        public BindingList<CLASS> ListClass
        {
            get
            {
                if (_listClass == null)
                    _listClass = LoadClasses();
                return _listClass;
            }
            set
            {
                _listClass = value;
                OnPropertyChanged();
            }
        }

        private CLASS _selectedClass = null;
        public CLASS SelectedClass
        {
            get
            {
                if (_selectedClass == null)
                    _selectedClass = _nullClass;
                return _selectedClass;
            }
            set
            {
                _selectedClass = value ?? _nullClass;
                ClassName = _selectedClass.ClassName;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Room List
        private BindingList<ROOM> LoadRooms()
        {
            BindingList<ROOM> result = new BindingList<ROOM>(DataProvider.Ins.DB.ROOMs.ToList());
            return result;
        }

        private BindingList<ROOM> _listRoom = null;
        public BindingList<ROOM> ListRoom
        {
            get
            {
                if (_listRoom == null)
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
                if (_selectedRoom == null)
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
                if (_selectedStartTimeRange == null)
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

        #region Subject List
        private BindingList<SUBJECT_> LoadSubjects()
        {
            BindingList<SUBJECT_> result = new BindingList<SUBJECT_>(DataProvider.Ins.DB.SUBJECT_.ToList());
            return result;
        }

        private BindingList<SUBJECT_> _listSubject = null;
        public BindingList<SUBJECT_> ListSubject
        {
            get
            {
                if (_listSubject == null)
                    _listSubject = LoadSubjects();
                return _listSubject;
            }
            set
            {
                _listSubject = value;
                OnPropertyChanged();
            }
        }

        private SUBJECT_ _selectedSubject = null;
        public SUBJECT_ SelectedSubject
        {
            get
            {
                if(_selectedSubject == null)
                    try
                    { 
                        _selectedSubject = ListSubject.ElementAt(0);
                    }
                    catch { }

                return _selectedSubject;
            }
            set
            {
                _selectedSubject = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region training programs List
        private BindingList<TRAINING_PROGRAM> LoadTrainingPrograms()
        {
            BindingList<TRAINING_PROGRAM> result = new BindingList<TRAINING_PROGRAM>(DataProvider.Ins.DB.TRAINING_PROGRAM.ToList());
            return result;
        }

        private BindingList<TRAINING_PROGRAM> _listTrainingProgram = null;
        public BindingList<TRAINING_PROGRAM> ListTrainingProgram
        {
            get
            {
                if (_listTrainingProgram == null)
                    _listTrainingProgram = LoadTrainingPrograms();
                return _listTrainingProgram;
            }
            set
            {
                _listTrainingProgram = value;
                OnPropertyChanged();
            }
        }

        private TRAINING_PROGRAM _selectedTrainingProgram = null;
        public TRAINING_PROGRAM SelectedTrainingProgram
        {
            get
            {
                if(_selectedTrainingProgram == null)
                    try 
                    {
                        _selectedTrainingProgram = ListTrainingProgram.ElementAt(0);
                    }
                    catch { }

                return _selectedTrainingProgram;
            }
            set
            {
                _selectedTrainingProgram = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Semester List
        public BindingList<Utils.IdNamePair<int>> ListSemester
        {
            get => Utils.Semesters;
            set { /*cant set this*/ }
               
        }

        private Utils.IdNamePair<int> _selectedSemester;
        public Utils.IdNamePair<int> SelectedSemester
        {
            get 
            {
                if(_selectedSemester == null)
                    _selectedSemester = ListSemester.ElementAt(0);

                return _selectedSemester;
            }
            set
            {
                _selectedSemester = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region School Year List
        public BindingList<Utils.IdNamePair<int>> ListSchoolYear
        {
            get => Utils.SchoolYears;
            set { /*cant set this*/ }
        }

        private Utils.IdNamePair<int> _selectedSchoolYear;
        public Utils.IdNamePair<int> SelectedSchoolYear
        {
            get
            {
                if(_selectedSchoolYear == null)
                    _selectedSchoolYear = ListSchoolYear.ElementAt(0);

                return _selectedSchoolYear;
            }
            set
            {
                _selectedSchoolYear = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region DaysOfWeek List
        public BindingList<Utils.IdNamePair<int>> ListDayOfWeek
        {
            get => Utils.DaysOfWeek;
            set { /*cant set this*/ }
        }

        private Utils.IdNamePair<int> _selectedDayOfWeek;
        public Utils.IdNamePair<int> SelectedDayOfWeek
        {
            get
            {
                if (_selectedDayOfWeek == null)
                    _selectedDayOfWeek = ListDayOfWeek.ElementAt(0);

                return _selectedDayOfWeek;
            }
            set
            {
                _selectedDayOfWeek = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region datagrid using classes
        private ObservableCollection<USINGCLASS> LoadUsingClasses()
        {
            ObservableCollection<USINGCLASS> result = new ObservableCollection<USINGCLASS>
                (
                    DataProvider.Ins.DB.USINGCLASSes.Where(x => x.Status_ != (int)Enums.UsingStatus.Deleted).ToList()
                );
            return result;
        }

        private ObservableCollection<USINGCLASS> _listUsingClass = null;
        public ObservableCollection<USINGCLASS> ListUsingClass
        {
            get
            {
                if (_listUsingClass == null)
                    _listUsingClass = LoadUsingClasses();
                return _listUsingClass;
            }
            set
            {
                _listUsingClass = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<USINGCLASS> _selectedUsingClasses = new ObservableCollection<USINGCLASS>();
        public ObservableCollection<USINGCLASS> SelectedUsingClasses
        {
            get => _selectedUsingClasses;
            set
            {
                _selectedUsingClasses = value;
                ListUsingClass_OnSelectionChanged();
                OnPropertyChanged();
            }
        }

        private ICommand _listUsingEvent_SelectionChangedCmd = null;
        public ICommand ListUsingEvent_SelectionChangedCmd
        {
            get
            {
                if (_listUsingEvent_SelectionChangedCmd == null)
                    _listUsingEvent_SelectionChangedCmd = new RelayCommand(obj => ListUsingClass_OnSelectionChanged(obj));
                return _listUsingEvent_SelectionChangedCmd;
            }
            set
            {
                _listUsingEvent_SelectionChangedCmd = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region lecturer List
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
            if (errors.Count == 0)
            {
                // PrintClassInfoTest();
                // auto schedule
                AddClassToPendingList();
                Reset();
            }
            else
            {
                string msg = errors.Aggregate((x, y) => x + "\n" + y);
                System.Windows.MessageBox.Show(msg);
            }
        }
        private ICommand _confirmCmd = null;
        public ICommand ConfirmCmd
        {
            get
            {
                if (_confirmCmd == null)
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
            var dlgRes = System.Windows.MessageBox.Show("Bạn có chắc muốn huỷ lớp học này không?", "Huỷ", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (dlgRes == MessageBoxResult.Yes)
            { 
                Reset();
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
            var dlgRes = System.Windows.MessageBox.Show("Bạn có chắc muốn xoá các đăng ký phòng học này không?", "Xoá", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (dlgRes == MessageBoxResult.Yes)
                SetStatusSelectedUsingClasses(Enums.UsingStatus.Deleted, x => true);
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
            var dlgRes = System.Windows.MessageBox.Show("Bạn có chắc muốn duyệt các đăng ký phòng học này không?", "Duyệt", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (dlgRes == MessageBoxResult.Yes)
                SetStatusSelectedUsingClasses(Enums.UsingStatus.Approved, Utils.ValidateForApprove);
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
            var dlgRes = System.Windows.MessageBox.Show("Bạn có chắc muốn từ chối các đăng ký phòng học này không?", "Từ chối", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (dlgRes == MessageBoxResult.Yes)
                SetStatusSelectedUsingClasses(Enums.UsingStatus.Rejected, x => true);
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

        #region read Excel button
        private void HandleReadExcelButtonClick()
        {
            // CuteTN note: more code. ..
            ReadFromExcel();
        }
        private ICommand _readExcelCmd = null;
        public ICommand ReadExcelCmd
        {
            get
            {
                if (_readExcelCmd == null)
                    _readExcelCmd = new RelayCommand(obj => HandleReadExcelButtonClick());
                return _readExcelCmd;
            }
            set
            {
                _readExcelCmd = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #endregion

        #region Utils
        private List<string> Validate()
        {
            List<string> result = new List<string>();

            if (string.IsNullOrEmpty(ClassName))
                result.Add("Vui lòng nhập tên lớp");

            if (Population <= 0)
                result.Add("Vui lòng nhập số người dự tính hợp lệ (số nguyên dương)");

            // CuteTN Note: magic number
            if (Duration <= 0 || Duration > 45 * 5)
                result.Add("Vui lòng nhập thời lượng hợp lệ (Thời lượng là một số nguyên dương nhỏ hơn 225 phút (5 tiết))");

            return result;
        }

        private void ResetForm()
        {
            UsingClassId = null;
            SelectedSubject = null;
            SelectedTrainingProgram = null;
            SelectedLecturer = null;
            // SelectedSemester = null; 
            // SelectedSchoolYear = null;  // keeping old value would be cool
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            Duration = 90;
            Population = 50;
            SelectedClass = null;
            ClassName = "";
            SelectedStartTimeRange = null;
            SelectedRoom = null;
            Description = "";
        }

        private void RefreshData()
        {
            ListSubject = LoadSubjects();
            ListTrainingProgram = LoadTrainingPrograms();
            ListClass = LoadClasses();
            ListTimeRange = LoadPeriod_TimeRanges();
            ListRoom = LoadRooms();
            ListUsingClass = LoadUsingClasses();
        }

        private void Reset()
        {
            RefreshData();
            ResetForm();
            IsEdittingFormMode = false;
            LoadSelectedContentToForm();
        }

        private void PrintClassInfoTest()
        {
            // CuteNote: TEST
            string toPrint = "";
            toPrint += $"class Id = {SelectedClass.ClassID}\n";
            toPrint += $"class name = {ClassName}\n";
            toPrint += $"subject name = {SelectedSubject.SubjectName}\n";
            toPrint += $"facaulty name = {SelectedSubject.FACAULTY.FacaultyName}\n";
            toPrint += $"semester = {SelectedSemester.Name}\n";
            toPrint += $"Schoolyear = {SelectedSchoolYear.Name}\n";
            toPrint += $"startdate = {StartDate}\n";
            toPrint += $"enddate = {EndDate}\n";
            toPrint += $"duration = {Duration}\n";
            toPrint += $"population = {Population}\n";
            toPrint += $"room name = {SelectedRoom.RoomName}\n";
            toPrint += $"start period = {SelectedStartTimeRange.PeriodName}\n";

            System.Windows.MessageBox.Show(toPrint);
        }

        /// <summary>
        /// Add Class then Add Using class.
        /// Causion: this function does not validate data before saving
        /// </summary>
        /// <returns></returns>
        private void AddClassToPendingList()
        {
            AddClassDB();
            AddUsingClassDB();
        }

        /// <summary>
        /// Add NEW entry if the id has not exist. otherwise update the old entry
        /// Causion: this function does not validate data before saving
        /// </summary>
        private void AddClassDB()
        {
            CLASS c = SelectedClass;

            bool isNewClass = SelectedClass.ClassID == _nullClassIdPlaceholder;
            if (isNewClass)
                c = new CLASS();

            c.ClassName = ClassName;
            c.SubjectID = SelectedSubject.SubjectID;
            c.TrainingProgramID = SelectedTrainingProgram.TrainingProgramID;
            c.Semester = SelectedSemester.Id;
            c.Year_ = SelectedSchoolYear.Id;
            c.StartDate = StartDate;
            c.EndDate = EndDate;
            c.Population_ = Population;
            c.LecturerID = SelectedLecturer.LecturerID;
            c.Description_ = Description;

            if (isNewClass)
            {
                c.ClassID = Utils.GenerateStringId(DataProvider.Ins.DB.CLASSes);
                DataProvider.Ins.DB.CLASSes.Add(c);
            }

            // if found, e is a reference to an CLASS in model...
            // lets hope it changed, too...

            SaveDB();
            SelectedClass = c;
        }

        /// <summary>
        /// Add NEW entry if the id has not exist. otherwise update the old entry
        /// Causion: this function does not validate data before saving
        /// </summary>
        private void AddUsingClassDB()
        {
            USINGCLASS uc;
            bool isNewUsingClass = UsingClassId == _nullUsingClassIdPlaceholder;
            if (isNewUsingClass)
            {
                uc = new USINGCLASS();
            }
            else
            {
                uc = DataProvider.Ins.DB.USINGCLASSes.Find(UsingClassId);
            }

            uc.RoomID = SelectedRoom.RoomID;
            uc.ClassID = SelectedClass.ClassID;
            uc.StartPeriod = SelectedStartTimeRange.PeriodID;
            uc.Duration = TimeSpan.FromMinutes(Duration);
            uc.RepeatCycle = 1; // CuteTN Note: technical debt
            uc.Day_ = SelectedDayOfWeek.Id;
            uc.Status_ = (int)Enums.UsingStatus.Pending;
            uc.Description_ = Description;

            uc.StartDate = StartDate;
            uc.EndDate = EndDate;

            if (isNewUsingClass)
            {
                uc.UsingClassID = Utils.GenerateStringId(DataProvider.Ins.DB.USINGCLASSes);
                ScheduleAndAdd(uc, true);
            }
            
            // CuteTN Note:
            // when editting old using class, it's not neccessary to validate, since we would do that when approving yey.

            SaveDB();
        }

        /// <summary>
        /// Get selected item info from datagrid for previewing.
        /// </summary>
        /// <param name="usingEvent"></param>
        private void LoadContentToForm(USINGCLASS usingClass)
        {
            UsingClassId = usingClass.UsingClassID;

            SelectedClass = usingClass.CLASS;
            ClassName = usingClass.CLASS.ClassName;
            SelectedLecturer = usingClass.CLASS.LECTURER;
            SelectedTrainingProgram = usingClass.CLASS.TRAINING_PROGRAM;
            SelectedDayOfWeek = Utils.GetElementById(ListDayOfWeek, usingClass.Day_);
            SelectedSemester = Utils.GetElementById(ListSemester, usingClass.CLASS.Semester);
            SelectedSchoolYear = Utils.GetElementById(ListSchoolYear, usingClass.CLASS.Year_);
            SelectedSubject = usingClass.CLASS.SUBJECT_;
            StartDate = usingClass.CLASS.StartDate;
            EndDate = usingClass.CLASS.EndDate;
            Duration = (int)Math.Round(usingClass.Duration.TotalMinutes);
            Population = usingClass.CLASS.Population_;
            SelectedStartTimeRange = usingClass.PERIOD_TIMERANGE;
            SelectedRoom = usingClass.ROOM;
            Description = usingClass.Description_;
        }

        private void LoadSelectedContentToForm()
        {
            if (SelectedUsingClasses.Count == 1)
            {
                var selectedUE = SelectedUsingClasses[0];
                LoadContentToForm(selectedUE);
            }
            else
            {
                ResetForm();
            }
        }

        private void GetDataGridSelectedItems(System.Windows.Controls.DataGrid datagrid)
        {
            SelectedUsingClasses.Clear();

            foreach (var item in datagrid.SelectedItems)
            {
                USINGCLASS uc = (USINGCLASS)item;
                if (uc != null && !SelectedUsingClasses.Contains(uc))
                    SelectedUsingClasses.Add(uc);
            }
        }

        private void ListUsingClass_OnSelectionChanged(Object obj = null)
        {
            System.Windows.Controls.DataGrid dataGrid = obj as System.Windows.Controls.DataGrid;
            if (dataGrid == null)
                return;

            GetDataGridSelectedItems(dataGrid);
            UpdateEnabledViewElements(); 

            // update form content when user is not in editting mode
            if (!IsEdittingFormMode)
            {
                LoadSelectedContentToForm();   
            }
        }

        private void EnableEditingForm()
        {
            IsEdittingFormMode = true;
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

        private void SetStatusSelectedUsingClasses(Enums.UsingStatus newStatus, Func<USINGCLASS, bool> validateFunc)
        {
            foreach (var uc in SelectedUsingClasses)
            {
                if(validateFunc(uc))
                { 
                    uc.Status_ = (int)newStatus;
                    SaveDB();
                }
            }

            ListUsingClass = LoadUsingClasses();
        }

        private void SaveDB()
        {
            DataProvider.Ins.DB.SaveChanges();
        }


        /// <summary>
        /// <para> parse a row raw information into CLASS and USINGCLASS </para>
        /// <para> WARNING: this function does NOT generate the Ids </para>
        /// </summary>
        /// <param name="template"></param>
        /// <param name="rawData"></param>
        /// <param name="outputClass"></param>
        /// <param name="outputUsingClass"></param>
        private void ParseExcelRowFromTemplate(List<string> template, List<string> rawData, out CLASS outputClass, out USINGCLASS outputUsingClass)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            
            for(int i=0; i<template.Count; i++)
            {
                dict.Add(template[i].ToUpper(), rawData[i]);
            }

            outputClass = new CLASS();
            outputUsingClass = new USINGCLASS();

            // CuteTN: using string directly here is way tooooo DIRTY. but I have no time lol
            outputClass.ClassName = dict["TÊN LỚP"];
            outputClass.SubjectID = dict["MÃ MÔN HỌC"];

            string trainingProgramName = dict["HỆ ĐÀO TẠO"];
            outputClass.TrainingProgramID = DataProvider.Ins.DB.TRAINING_PROGRAM.Where(x => x.TrainingProgramName == trainingProgramName).First().TrainingProgramID;

            outputClass.Year_ = Utils.GetElementByName(ListSchoolYear, dict["NĂM HỌC"]).Id;
            outputClass.Semester = Utils.GetElementByName(ListSemester, dict["HỌC KỲ"]).Id;
            outputClass.LecturerID = dict["MÃ GIẢNG VIÊN"];
            outputClass.StartDate = DateTime.ParseExact(dict["NGÀY BẮT ĐẦU"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            outputClass.EndDate = DateTime.ParseExact(dict["NGÀY KẾT THÚC"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            outputClass.Population_ = int.Parse(dict["SĨ SỐ"]);

            outputUsingClass.Duration = TimeSpan.FromMinutes(int.Parse(dict["THỜI LƯỢNG"]));
            outputUsingClass.Description_ = dict["GHI CHÚ"];

            // default fields
            outputUsingClass.RoomID = Utils.NullStringId;
            outputUsingClass.StartPeriod = Utils.NullIntId;
            outputUsingClass.Day_ = Utils.NullIntId;
            outputUsingClass.RepeatCycle = 1;
            outputUsingClass.Status_ = (int)Enums.UsingStatus.Pending;
        }

        private void ReadFromExcel()
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Excel file (*.xlsx;*.xls)|*.xlsx;*.xls|All files (*.*)|*.*";
                
            var dlgRes = dlg.ShowDialog();

            if(dlgRes != DialogResult.OK)
                return;

            List<List<string>> importedData;

            try
            {
                importedData = MsExcelReader.Read(dlg.FileName);
            }
            catch(Exception e)
            {
                System.Windows.MessageBox.Show($"Đã xảy ra lỗi trong quá trình đọc file, vui lòng kiểm tra định dạng.\n{e.Message}");
                return;
            }

            // var testStr = Utils.Convert2DListToString(importedData);
            // System.Windows.MessageBox.Show(testStr.ToUpper());

            CLASS parsedClass, tempClass;
            USINGCLASS parsedUsingClass;
            List<int> errorLines = new List<int>();

            for(int i = 1; i < importedData.Count; i++)
            {

                try
                { 
                    ParseExcelRowFromTemplate(importedData[0], importedData[i], out parsedClass, out parsedUsingClass);

                    tempClass = null;

                    // trying searching for the same class first...
                    try
                    {
                        tempClass = DataProvider.Ins.DB.CLASSes.Where
                            (x =>
                                (x.ClassName == parsedClass.ClassName)
                                &&(x.Year_ == parsedClass.Year_)
                                &&(x.Semester == parsedClass.Semester)
                                &&(x.TrainingProgramID == parsedClass.TrainingProgramID)
                            ).First();
                    }
                    catch(Exception e)
                    {
                    }

                    // if this class was created before
                    if (tempClass != null)
                    {
                        parsedClass.ClassID = tempClass.ClassID;

                        tempClass.SubjectID = parsedClass.SubjectID;
                        tempClass.LecturerID = parsedClass.LecturerID;
                        tempClass.StartDate = parsedClass.StartDate;
                        tempClass.EndDate = parsedClass.EndDate;
                        tempClass.Population_ = parsedClass.Population_;
                    }
                    else
                    {
                        parsedClass.ClassID = Utils.GenerateStringId(DataProvider.Ins.DB.CLASSes);
                        DataProvider.Ins.DB.CLASSes.AddOrUpdate(parsedClass); // using System.Data.Entity.Migrations
                    }
                    SaveDB();

                    parsedUsingClass.UsingClassID = Utils.GenerateStringId(DataProvider.Ins.DB.USINGCLASSes);
                    parsedUsingClass.ClassID = parsedClass.ClassID;
                    
                    // DataProvider.Ins.DB.USINGCLASSes.Add(parsedUsingClass);
                    ScheduleAndAdd(parsedUsingClass, false);

                    SaveDB();
                }
                catch
                {
                    errorLines.Add(i);
                    continue;
                }
            }

            if(errorLines.Count == 0) { }
            else
                System.Windows.MessageBox.Show(
                    "Đã xảy ra lỗi ở các dòng sau: " +
                    errorLines.Select(x => (x + 1).ToString()).Aggregate((x, y) => x + "; " + y)
                    );

            Reset();
        }

        private List<USINGCLASS> AutoMakeSchedule(USINGCLASS uc)
        {
            CLASS class_ = DataProvider.Ins.DB.CLASSes.Find(uc.ClassID);

            ROOM room = null;
            if(uc.RoomID != Utils.NullStringId)
                room = DataProvider.Ins.DB.ROOMs.Find(uc.RoomID);

            DayOfWeek? dow = null;
            if(uc.Day_ != Utils.NullIntId)
                dow = (DayOfWeek)uc.Day_;

            Nullable<int> sp = null;
            if(uc.StartPeriod != Utils.NullIntId)
                sp = uc.StartPeriod;

            return Utils.AutoMakeSchedule(uc, class_, room, dow, sp);
        }

        private void ScheduleAndAdd(USINGCLASS uc, bool enableShowError)
        {
            List<USINGCLASS> ucs = AutoMakeSchedule(uc);

            if (ucs == null)
            {
                if(enableShowError)
                    System.Windows.MessageBox.Show("Không thể tự động sắp phòng học");

                DataProvider.Ins.DB.USINGCLASSes.Add(uc);
            }
            else
                foreach (var temp in ucs)
                {
                    DataProvider.Ins.DB.USINGCLASSes.Add(temp);
                }
        }
        #endregion
    }
}
