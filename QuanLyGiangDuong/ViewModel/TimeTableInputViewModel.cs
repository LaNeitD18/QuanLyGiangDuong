using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using QuanLyGiangDuong.Model;
using QuanLyGiangDuong.Utilities;

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
                // CuteTN note delete this later
                // UpdateEnabledViewElements();
                OnPropertyChanged();
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////// 
        /// all the boolean variables below doesnt actually have its own value
        /// it depends on the state of the form (Datagrid number of selected rows, and editting mode)

        /*
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
        */
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
                PrintClassInfoTest();
                AddClassToPendingList();
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
            var dlgRes = MessageBox.Show("Huỷ", "Bạn có chắc muốn huỷ lớp học này không?", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (dlgRes == MessageBoxResult.Yes)
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

        #endregion

        #region Utils
        private List<string> Validate()
        {
            List<string> result = new List<string>();

            if (string.IsNullOrEmpty(ClassName))
                result.Add("Vui lòng nhập tên lớp");

            if (Population <= 0)
                result.Add("Vui lòng nhập số người dự tính hợp lệ (số nguyên dương)");

            if (Duration <= 0)
                result.Add("Vui lòng nhập thời lượng hợp lệ (số nguyên dương)");

            return result;
        }

        private void ResetForm()
        {
            SelectedSubject = null;
            SelectedTrainingProgram = null;
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

        }

        private void Reset()
        {
            RefreshData();
            ResetForm();
            IsEdittingFormMode = false;
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

            MessageBox.Show(toPrint);
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
            c.LecturerID = LecturerId;
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

            if (isNewUsingClass)
            {
                uc.UsingClassID = Utils.GenerateStringId(DataProvider.Ins.DB.USINGCLASSes);
                DataProvider.Ins.DB.USINGCLASSes.Add(uc);
            }

            SaveDB();
        }

        private void SaveDB()
        {
            DataProvider.Ins.DB.SaveChanges();
        }

        #endregion
    }
}
