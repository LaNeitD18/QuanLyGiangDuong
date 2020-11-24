using QuanLyGiangDuong.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyGiangDuong.ViewModel
{
    class ExamInputViewModel : BaseViewModel
    {
        #region Variables
        private ObservableCollection<ROOM> _ListRoom;
        public ObservableCollection<ROOM> ListRoom
        {
            get { return _ListRoom; }
            set { _ListRoom = value; OnPropertyChanged(); }
        }

        private ROOM _SelectedRoom;
        public ROOM SelectedRoom
        {
            get { return _SelectedRoom; }
            set { _SelectedRoom = value; OnPropertyChanged(); }
        }

        private ObservableCollection<CLASS> _ListClass;
        public ObservableCollection<CLASS> ListClass
        {
            get { return _ListClass; }
            set { _ListClass = value; OnPropertyChanged(); }
        }

        private CLASS _SelectedClass;
        public CLASS SelectedClass
        {
            get { return _SelectedClass; }
            set { _SelectedClass = value; OnPropertyChanged(); }
        }

        private ObservableCollection<USINGEXAM> _ListUsingExam;
        public ObservableCollection<USINGEXAM> ListUsingExam
        {
            get { return _ListUsingExam; }
            set { _ListUsingExam = value; OnPropertyChanged(); }
        }

        private USINGEXAM _SelectedUsingExam;
        public USINGEXAM SelectedUsingExam
        {
            get { return _SelectedUsingExam; }
            set { _SelectedUsingExam = value; OnPropertyChanged(); }
        }

        private ObservableCollection<USINGEXAM> _ListSelectedUsingExam;
        public ObservableCollection<USINGEXAM> ListSelectedUsingExam
        {
            get { return _ListSelectedUsingExam; }
            set { _ListSelectedUsingExam = value; OnPropertyChanged(); }
        }

        private ObservableCollection<EXAM> _ListExam;
        public ObservableCollection<EXAM> ListExam
        {
            get { return _ListExam; }
            set { _ListExam = value; OnPropertyChanged(); }
        }

        private EXAM _SelectedExam;
        public EXAM SelectedExam
        {
            get { return _SelectedExam; }
            set { _SelectedExam = value; OnPropertyChanged(); }
        }

        private ObservableCollection<LECTURER> _ListSupervisor;
        public ObservableCollection<LECTURER> ListSupervisor
        {
            get { return _ListSupervisor; }
            set { _ListSupervisor = value; OnPropertyChanged(); }
        }

        private LECTURER _SelectedSupervisor;
        public LECTURER SelectedSupervisor
        {
            get { return _SelectedSupervisor; }
            set { _SelectedSupervisor = value; OnPropertyChanged(); }
        }

        private string _ExamSubject;
        public string ExamSubject
        {
            get { return _ExamSubject; }
            set { _ExamSubject = value; OnPropertyChanged(); }
        }

        private string _UsingExamID;
        public string UsingExamID
        {
            get { return _UsingExamID; }
            set { _UsingExamID = value; OnPropertyChanged(); }
        }

        private string _ExamID;
        public string ExamID
        {
            get { return _ExamID; }
            set { _ExamID = value; OnPropertyChanged(); }
        }

        private string _Supervisor;
        public string Supervisor
        {
            get { return _Supervisor; }
            set { _Supervisor = value; OnPropertyChanged(); }
        }

        private string _ExamTime;
        public string ExamTime
        {
            get { return _ExamTime; }
            set { _ExamTime = value; OnPropertyChanged(); }
        }

        private string _Population;
        public string Population
        {
            get { return _Population; }
            set { _Population = value; OnPropertyChanged(); }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; OnPropertyChanged(); }
        }

        private string _StartDate;
        public string StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; OnPropertyChanged(); }
        }

        private string _EndDate;
        public string EndDate
        {
            get { return _EndDate; }
            set { _EndDate = value; OnPropertyChanged(); }
        }

        private bool _IsAddingEnabled;
        public bool IsAddingEnabled
        {
            get { return _IsAddingEnabled; }
            set { _IsAddingEnabled = value; OnPropertyChanged(); }
        }

        private bool _IsEditingEnabled;
        public bool IsEditingEnabled
        {
            get { return _IsEditingEnabled; }
            set { _IsEditingEnabled = value; OnPropertyChanged(); }
        }

        private bool _IsDeletingEnabled;
        public bool IsDeletingEnabled
        {
            get { return _IsDeletingEnabled; }
            set { _IsDeletingEnabled = value; OnPropertyChanged(); }
        }

        private bool _IsApprovingEnabled;
        public bool IsApprovingEnabled
        {
            get { return _IsApprovingEnabled; }
            set { _IsApprovingEnabled = value; OnPropertyChanged(); }
        }

        private bool _IsRejectingEnabled;
        public bool IsRejectingEnabled
        {
            get { return _IsRejectingEnabled; }
            set { _IsRejectingEnabled = value; OnPropertyChanged(); }
        }

        private bool _IsBeingInTask;
        public bool IsBeingInTask
        {
            get { return _IsBeingInTask; }
            set { _IsBeingInTask = value; OnPropertyChanged(); }
        }

        private bool _IsEnabledElements;
        public bool IsEnabledElements
        {
            get { return _IsEnabledElements; }
            set { _IsEnabledElements = value; OnPropertyChanged(); }
        }
        #endregion

        #region Function
        private void LoadData()
        {
            var listRoom = DataProvider.Ins.DB.ROOMs;
            ListRoom = new ObservableCollection<ROOM>(listRoom);

            var listClass = DataProvider.Ins.DB.CLASSes;
            ListClass = new ObservableCollection<CLASS>(listClass);

            var listUsingExam = DataProvider.Ins.DB.USINGEXAMs.Where(x => x.Status_ <= 3).OrderBy(x => x.Status_);
            ListUsingExam = new ObservableCollection<USINGEXAM>(listUsingExam);
            ListSelectedUsingExam = new ObservableCollection<USINGEXAM>();

            var listSupervisor = DataProvider.Ins.DB.LECTURERs;
            ListSupervisor = new ObservableCollection<LECTURER>(listSupervisor);
        }
        private void DisableButtons()
        {
            IsAddingEnabled = false;
            IsEditingEnabled = false;
            IsDeletingEnabled = false;
            IsApprovingEnabled = false;
            IsRejectingEnabled = false;
            IsBeingInTask = false;
        }
        private void DisableElements() {
            IsEnabledElements = false;
        }
        private void EnableElements()
        {
            IsEnabledElements = true;
        }
        private void EnableOnlyAdding()
        {
            DisableButtons();
            IsAddingEnabled = true;
            IsBeingInTask = true;
        }
        private void EnableOnlyEditing()
        {
            DisableButtons();
            IsEditingEnabled = true;
            IsBeingInTask = true;
        }
        private void EnableOnlyDeleting()
        {
            DisableButtons();
            IsDeletingEnabled = true;
            IsBeingInTask = true;
        }
        private void EnableOnlyApproving()
        {
            DisableButtons();
            IsApprovingEnabled = true;
            IsBeingInTask = true;
        }
        private void EnableOnlyRejecting()
        {
            DisableButtons();
            IsRejectingEnabled = true;
            IsBeingInTask = true;
        }
        private void ResetElements()
        {
            SelectedRoom = null;
            SelectedClass = null;
            SelectedSupervisor = null;
            StartDate = null;
            ExamTime = "";
            Population = "";
            Description = "";
        }
        private void ResetAll()
        {
            ResetElements();
            DisableElements();
            EnableOnlyAdding();
            IsBeingInTask = false;

            SelectedUsingExam = null;
            SelectedExam = null;
            UsingExamID = "";
        }

        private void SetValueForElements()
        {
            SelectedClass = ListClass.Where(x => x.ClassID == SelectedExam.ClassID).FirstOrDefault();
            SelectedRoom = ListRoom.Where(x => x.RoomID == SelectedUsingExam.RoomID).FirstOrDefault();
            SelectedSupervisor = ListSupervisor.Where(x => x.LecturerID == SelectedExam.LecturerID).FirstOrDefault();
            Population = SelectedExam.Population_.ToString();
            StartDate = SelectedUsingExam.Date_.ToString();
            ExamTime = SelectedUsingExam.Duration.Minutes.ToString();
            Description = SelectedUsingExam.Description_;
        }

        private string AutoGenerateExamID() {
            string result = "";

            ObservableCollection<EXAM> listExam = new ObservableCollection<EXAM>(DataProvider.Ins.DB.EXAMs);
            if(listExam.Count() == 0) {
                result = "1";
            } else {
                result = (listExam.Count() + 1).ToString();
            }

            return result;
        }

        private string AutoGenerateUsingExamID()
        {
            string result = "";

            ObservableCollection<USINGEXAM> listUsingExam = new ObservableCollection<USINGEXAM>(DataProvider.Ins.DB.USINGEXAMs);
            if (listUsingExam.Count() == 0) {
                result = "1";
            } else {
                result = (listUsingExam.Count() + 1).ToString();
            }

            return result;
        }

        private void AddExam()
        {
            ExamID = AutoGenerateExamID();

            // add exam to database
            var exam = new EXAM()
            {
                ExamID = ExamID,
                LecturerID = Supervisor,
                ClassID = SelectedClass.ClassID,
                Population_ = Convert.ToInt32(Population)
            };
            DataProvider.Ins.DB.EXAMs.Add(exam);
            DataProvider.Ins.DB.SaveChanges();
        }

        private void AddUsingExam() {
            var usingExam = new USINGEXAM() {
                UsingExamID = UsingExamID,
                RoomID = SelectedRoom.RoomID,
                ExamID = ExamID,
                Date_ = Convert.ToDateTime(StartDate),
                StartPeriod = 1,
                Duration = new TimeSpan(0, Convert.ToInt32(ExamTime), 0),
                Status_ = 0
            };
            MessageBox.Show(StartDate);
            DataProvider.Ins.DB.USINGEXAMs.Add(usingExam);
            DataProvider.Ins.DB.SaveChanges();

            // add to display
            ListUsingExam.Add(usingExam);
        }

        private void EditExam() {
            SelectedExam.LecturerID = SelectedSupervisor.LecturerID;
            SelectedExam.ClassID = SelectedClass.ClassID;
            SelectedExam.Population_ = Convert.ToInt32(Population);
            DataProvider.Ins.DB.SaveChanges();
        }

        private void EditUsingExam()
        {
            SelectedUsingExam.RoomID = SelectedRoom.RoomID;
            SelectedUsingExam.Date_ = Convert.ToDateTime(StartDate);
            SelectedUsingExam.Duration = new TimeSpan(0, Convert.ToInt32(ExamTime), 0);
            SelectedUsingExam.Description_ = Description;
            DataProvider.Ins.DB.SaveChanges();

            var temp = SelectedUsingExam;
            int length = ListUsingExam.Count();
            for (int i = 0; i < length; i++)
            {
                if (ListUsingExam[i].UsingExamID == SelectedUsingExam.UsingExamID)
                {
                    ListUsingExam.RemoveAt(i);
                    ListUsingExam.Insert(i, temp);
                    break;
                }
            }

            SelectedUsingExam = temp;
        }

        private void DeleteUsingExam()
        {
            SelectedUsingExam.Status_ = 3;
            DataProvider.Ins.DB.SaveChanges();

            int length = ListUsingExam.Count();
            for (int i = 0; i < length; i++)
            {
                if (ListUsingExam[i].UsingExamID == SelectedUsingExam.UsingExamID)
                {
                    ListUsingExam.RemoveAt(i);
                    break;
                }
            }
            SelectedUsingExam = null;
        }

        private void DeleteUsingExams()
        {
            List<USINGEXAM> list = ListSelectedUsingExam.ToList();
            foreach (var usingExam in list)
            {
                SelectedUsingExam = usingExam;
                DeleteUsingExam();
            }
        }

        private void ApproveUsingExam()
        {
            SelectedUsingExam.Status_ = 1;
            DataProvider.Ins.DB.SaveChanges();

            int length = ListUsingExam.Count();
            for (int i = 0; i < length; i++)
            {
                if (ListUsingExam[i].UsingExamID == SelectedUsingExam.UsingExamID)
                {
                    ListUsingExam.RemoveAt(i);
                    break;
                }
            }
            SelectedUsingExam = null;
        }

        private void ApproveUsingExams()
        {
            List<USINGEXAM> list = ListSelectedUsingExam.ToList();
            foreach (var usingExam in list)
            {
                SelectedUsingExam = usingExam;
                ApproveUsingExam();
            }
        }

        private void RejectUsingExam()
        {
            SelectedUsingExam.Status_ = 2;
            DataProvider.Ins.DB.SaveChanges();

            int length = ListUsingExam.Count();
            for (int i = 0; i < length; i++)
            {
                if (ListUsingExam[i].UsingExamID == SelectedUsingExam.UsingExamID)
                {
                    ListUsingExam.RemoveAt(i);
                    break;
                }
            }
            SelectedUsingExam = null;
        }

        private void RejectUsingExams()
        {
            List<USINGEXAM> list = ListSelectedUsingExam.ToList();
            foreach (var usingExam in list)
            {
                SelectedUsingExam = usingExam;
                RejectUsingExam();
            }
        }

        private bool IsValidInput()
        {
            bool flag = true;

            if (string.IsNullOrWhiteSpace(UsingExamID)) {
                flag = false;
            }
            if (string.IsNullOrWhiteSpace(Population)) {
                flag = false;
            }
            if (string.IsNullOrWhiteSpace(ExamTime)) {
                flag = false;
            }
            if(SelectedClass == null) {
                flag = false;
            }
            if (SelectedRoom == null) {
                flag = false;
            }
            if(SelectedSupervisor == null) {
                flag = false;
            }
            return flag;
        }

        private bool IsNotDuplicatedForAdding()
        {
            bool flag = true;
            var classID = DataProvider.Ins.DB.CLASSes.Where(x => x.ClassID == SelectedClass.ClassID);

            if (classID.Count() != 0)
            {
                MessageBox.Show("Lớp đã được xếp phòng thi.");
                flag = false;
            }

            return flag;
        }

        private bool IsNotDuplicatedForEditing()
        {
            bool flag = false;

            if(SelectedRoom.RoomID != SelectedUsingExam.RoomID) {
                flag = true;
            }
            if(SelectedClass.ClassID != SelectedExam.ClassID) {
                flag = true;
            }
            if(SelectedSupervisor.LecturerID != SelectedExam.LecturerID) {
                flag = true;
            }
            if(Population != SelectedExam.Population_.ToString()) {
                flag = true;
            }
            if(StartDate != SelectedUsingExam.Date_.ToString()) {
                flag = true;
            }
            if(ExamTime != SelectedUsingExam.Duration.ToString()) {
                flag = true;
            }
            if(Description != SelectedUsingExam.Description_) {
                flag = true;
            }

            return flag;
        }

        private void GetDataGridSelectedItems(DataGrid dataGrid)
        {
            ListSelectedUsingExam.Clear();

            foreach (var item in dataGrid.SelectedItems)
            {
                ListSelectedUsingExam.Add((USINGEXAM)item);
            }
        }
        #endregion

        #region ICommand
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ApproveCommand { get; set; }
        public ICommand RejectCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand UsingExam_SelectionChangedCommand { get; set; }
        #endregion

        public ExamInputViewModel()
        {
            LoadData();

            ResetAll();

            AddCommand = new RelayCommand((p) => {
                EnableOnlyAdding();

                EnableElements();
                ResetElements();
                SelectedUsingExam = null;

                // generate id
                UsingExamID = AutoGenerateUsingExamID();
            });

            EditCommand = new RelayCommand((p) => {
                EnableOnlyEditing();
                EnableElements();

                UsingExamID = SelectedUsingExam.UsingExamID;
            });

            DeleteCommand = new RelayCommand((p) => {
                EnableOnlyDeleting();
                EnableElements();

                UsingExamID = SelectedUsingExam.UsingExamID;
            });

            ApproveCommand = new RelayCommand((p) => {
                EnableOnlyApproving();
                EnableElements();

                UsingExamID = SelectedUsingExam.UsingExamID;
            });

            RejectCommand = new RelayCommand((p) => {
                EnableOnlyRejecting();
                EnableElements();

                UsingExamID = SelectedUsingExam.UsingExamID;
            });

            ConfirmCommand = new RelayCommand((p) => {
                // xong het nho lam check dk
                if(IsBeingInTask) { 
                    if(IsAddingEnabled) {
                        if(IsValidInput() && IsNotDuplicatedForAdding()) {
                            AddExam();
                            AddUsingExam();
                            ResetAll();
                        }
                    }
                    else if(IsEditingEnabled) {
                        if(IsValidInput() && IsNotDuplicatedForEditing()) {
                            EditExam();
                            EditUsingExam();
                            ResetAll();
                        }
                    }
                    else if(IsDeletingEnabled) {
                        if(ListSelectedUsingExam.Count() == 1) {
                            DeleteUsingExam();
                        } else {
                            DeleteUsingExams();
                        }
                        ResetAll();
                    }
                    else if(IsApprovingEnabled) {
                        if(ListSelectedUsingExam.Count() == 1) {
                            ApproveUsingExam();
                        } else {
                            ApproveUsingExams();
                        }
                        ResetAll();
                    }
                    else {
                        if(ListSelectedUsingExam.Count() == 1) {
                            RejectUsingExam();
                        } else {
                            RejectUsingExams();
                        }
                        ResetAll();
                    }
                }

                //if (IsBeingInTask && IsEditingEnabled)
                //{
                //    if (IsValidInput() && IsNotDuplicatedForEditing())
                //    {
                //        EditRoom();
                //        ResetAll();
                //    }
                //}
                //if (IsBeingInTask && IsDeletingEnabled)
                //{
                //    if (ListSelectedRoom.Count() == 1)
                //    {
                //        DeleteRoom();
                //        ResetAll();
                //    }
                //    else
                //    {
                //        DeleteRooms();
                //        ResetAll();
                //    }
                //}
            });

            CancelCommand = new RelayCommand((p) => {
                ResetAll();
            });

            UsingExam_SelectionChangedCommand = new RelayCommand((p) =>
            {
                if (!(p is DataGrid)) {
                    MessageBox.Show("ZZZ");
                    return;
                }

                var dataGrid = p as DataGrid;
                GetDataGridSelectedItems(dataGrid);

                if (SelectedUsingExam != null)
                {
                    SelectedExam = DataProvider.Ins.DB.EXAMs.Where(x => x.ExamID == SelectedUsingExam.ExamID).SingleOrDefault();
                    SetValueForElements();
                    IsEditingEnabled = IsDeletingEnabled = IsApprovingEnabled = IsRejectingEnabled = true;
                }
            });
        }
    }
}
