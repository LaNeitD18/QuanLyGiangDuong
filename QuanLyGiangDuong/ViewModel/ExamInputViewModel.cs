﻿using QuanLyGiangDuong.Model;
using QuanLyGiangDuong.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Migrations;
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
        #region Date fields
        private DateTime _selectedStartTime = DateTime.Now;
        public DateTime SelectedStartDate
        {
            get => _selectedStartTime;
            set
            {
                _selectedStartTime = value;
                OnPropertyChanged();
            }
        }

        private DateTime _selectedEndTime = DateTime.Now;
        public DateTime SelectedEndDate
        {
            get => _selectedEndTime;
            set
            {
                _selectedEndTime = value;
                OnPropertyChanged();
            }
        }

        #endregion

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
            ExamTime = SelectedUsingExam.Duration.TotalMinutes.ToString();
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
                LecturerID = SelectedSupervisor.LecturerID,
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
                Duration = TimeSpan.FromMinutes(Convert.ToInt32(ExamTime)),
                Status_ = 0,
            };
            // MessageBox.Show(StartDate);
            // DataProvider.Ins.DB.USINGEXAMs.Add(usingExam);
            if(StartDate == "")
                ScheduleAndAdd(usingExam, SelectedStartDate, SelectedEndDate, true);
            else
            {
                DateTime examDate = Convert.ToDateTime(StartDate);
                ScheduleAndAdd(usingExam, examDate, examDate, true);
            }

            DataProvider.Ins.DB.SaveChanges();

            // add to display
            // ListUsingExam.Add(usingExam);
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
            SelectedUsingExam.Status_ = (int)Enums.UsingStatus.Pending;
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
            SelectedUsingExam.Status_ = (int)Enums.UsingStatus.Deleted;
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
            // CuteTN + Basa code
            // validation before approving
            if(!Utils.ValidateForApprove(SelectedUsingExam))
                return;

            SelectedUsingExam.Status_ = (int)Enums.UsingStatus.Approved;
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
            SelectedUsingExam.Status_ = (int)Enums.UsingStatus.Rejected;
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
            var classID = DataProvider.Ins.DB.EXAMs.Where(x => x.ClassID == SelectedClass.ClassID);

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
        public ICommand ReadExcelCommand { get; set; }
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
                //EnableOnlyDeleting();
                //EnableElements();

                //UsingExamID = SelectedUsingExam.UsingExamID;
                var dlgRes = MessageBox.Show("Bạn có muốn xóa không?", "cap", MessageBoxButton.YesNo);
                if(dlgRes == MessageBoxResult.Yes) { 
                    if(ListSelectedUsingExam.Count() == 1) {
                        DeleteUsingExam();
                    } else {
                        DeleteUsingExams();
                    }
                    ResetAll();
                }
            });

            ApproveCommand = new RelayCommand((p) => {
                var dlgRes = MessageBox.Show("Bạn có muốn duyệt không?", "cap", MessageBoxButton.YesNo);
                if(dlgRes == MessageBoxResult.Yes) { 
                    if(ListSelectedUsingExam.Count() == 1) {
                        ApproveUsingExam();
                    } else {
                        ApproveUsingExams();
                    }
                    ResetAll();
                }
            });

            RejectCommand = new RelayCommand((p) => {
                var dlgRes = MessageBox.Show("Bạn có muốn từ chối không?", "cap", MessageBoxButton.YesNo);
                if(dlgRes == MessageBoxResult.Yes) { 
                    if(ListSelectedUsingExam.Count() == 1) {
                        RejectUsingExam();
                    } else {
                        RejectUsingExams();
                    }
                    ResetAll();
                }
            });

            // CuteTN - input by Excel
            ReadExcelCommand = new RelayCommand((p) => {
                ReadFromExcel();
            });

            ConfirmCommand = new RelayCommand((p) => {
                // xong het nho lam check dk
                if(IsBeingInTask) { 
                    if(IsAddingEnabled) {
                        if(IsValidInput()) {
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
                }
            });

            CancelCommand = new RelayCommand((p) => {
                ResetAll();
            });

            UsingExam_SelectionChangedCommand = new RelayCommand((p) =>
            {
                if (!(p is DataGrid)) {
                    return;
                }

                var dataGrid = p as DataGrid;
                GetDataGridSelectedItems(dataGrid);

                ResetElements();
                DisableElements();
                UsingExamID = "";

                if (ListSelectedUsingExam.Count() == 1) {
                    SelectedExam = DataProvider.Ins.DB.EXAMs.Where(x => x.ExamID == SelectedUsingExam.ExamID).SingleOrDefault();
                    SetValueForElements(); // count=1 --> display data

                    IsEditingEnabled = IsDeletingEnabled = true;
                    if(LoginViewModel.currentUser.LecturerTypeID == 1) {
                        IsApprovingEnabled = IsRejectingEnabled = true; // enable approve & reject for admin
                    }
                } 
                else if(ListSelectedUsingExam.Count() > 1) {
                    SelectedExam = DataProvider.Ins.DB.EXAMs.Where(x => x.ExamID == SelectedUsingExam.ExamID).SingleOrDefault();

                    IsDeletingEnabled = true;
                    if(LoginViewModel.currentUser.LecturerTypeID == 1) {
                        IsApprovingEnabled = IsRejectingEnabled = true;
                    }
                    IsEditingEnabled = false;    // cannot edit when selecting many items
                }
            });
        }

        // CuteTN
        #region Input by Excel
        /// <summary>
        /// <para> parse a row raw information into EXAM and USINGEXAM </para>
        /// <para> WARNING: this function does NOT generate the Ids </para>
        /// </summary>
        /// <param name="template"></param>
        /// <param name="rawData"></param>
        private void ParseExcelRowFromTemplate(List<string> template, List<string> rawData, out EXAM outputExam, out USINGEXAM outputUsingExam)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            for (int i = 0; i < template.Count; i++)
            {
                dict.Add(template[i].ToUpper(), rawData[i]);
            }

            outputExam = new EXAM();
            outputUsingExam = new USINGEXAM();

            // CuteTN: using string directly here is way tooooo DIRTY. but I have no time lol
            outputExam.ClassID = dict["MÃ LỚP"];
            outputExam.LecturerID = dict["MÃ GIÁM THỊ"];
            outputExam.Population_ = int.Parse(dict["SĨ SỐ"]);

            outputUsingExam.Duration = TimeSpan.FromMinutes(int.Parse(dict["THỜI GIAN THI"]));
            outputUsingExam.Description_ = dict["GHI CHÚ"];

            // default fields
            outputUsingExam.RoomID = Utils.NullStringId;
            outputUsingExam.StartPeriod = Utils.NullIntId;
            outputUsingExam.Date_ = DateTime.Now;
            outputUsingExam.Status_ = (int)Enums.UsingStatus.Pending;
        }

        private void ReadFromExcel()
        {
            var dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.Filter = "Excel file (*.xlsx;*.xls)|*.xlsx;*.xls|All files (*.*)|*.*";
            var dlgRes = dlg.ShowDialog();

            if(dlgRes != System.Windows.Forms.DialogResult.OK)
                return;

            List<List<string>> importedData;

            try
            {
                importedData = MsExcelReader.Read(dlg.FileName);
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show($"Đã xảy ra lỗi trong quá trình đọc file, vui lòng kiểm tra định dạng.\n{e.Message}");
                return;
            }

            // var testStr = Utils.Convert2DListToString(importedData);
            // System.Windows.MessageBox.Show(testStr.ToUpper());

            EXAM parsedExam;
            USINGEXAM parsedUsingExam;
            List<int> errorLines = new List<int>();

            for (int i = 2; i < importedData.Count; i++)
            {

                try
                {
                    ParseExcelRowFromTemplate(importedData[1], importedData[i], out parsedExam, out parsedUsingExam);

                    parsedExam.ExamID = Utils.GenerateStringId(DataProvider.Ins.DB.EXAMs);
                    DataProvider.Ins.DB.EXAMs.AddOrUpdate(parsedExam); // using System.Data.Entity.Migrations

                    parsedUsingExam.UsingExamID = Utils.GenerateStringId(DataProvider.Ins.DB.USINGEXAMs);
                    parsedUsingExam.ExamID = parsedExam.ExamID;
                    DataProvider.Ins.DB.USINGEXAMs.Add(parsedUsingExam);

                    DataProvider.Ins.DB.SaveChanges();

                    // ListExam.Add(parsedExam);
                    ListUsingExam.Add(parsedUsingExam);
                }
                catch
                {
                    errorLines.Add(i);
                    continue;
                }
            }

            if (errorLines.Count == 0) { }
            else
                System.Windows.MessageBox.Show(
                    "Đã xảy ra lỗi ở các dòng sau: " +
                    errorLines.Select(x => (x + 1).ToString()).Aggregate((x, y) => x + "; " + y)
                    );

            ResetAll();
        }
        #endregion

        // CuteTN + HCT
        #region Auto Schedule
        private USINGEXAM AutoMakeSchedule(USINGEXAM ue, DateTime startDate, DateTime endDate)
        {
            EXAM exam = DataProvider.Ins.DB.EXAMs.Find(ue.ExamID);

            ROOM room = null;
            if (ue.RoomID != Utils.NullStringId)
                room = DataProvider.Ins.DB.ROOMs.Find(ue.RoomID);

            Nullable<int> sp = null;
            if (ue.StartPeriod != Utils.NullIntId)
                sp = ue.StartPeriod;

            return Utils.AutoMakeExam(ue, exam, startDate, endDate);
        }

        private void ScheduleAndAdd(USINGEXAM ue, DateTime startDate, DateTime endDate, bool enableShowError)
        {
            var ue_ = AutoMakeSchedule(ue, startDate, endDate);

            if (ue_ == null)
            {
                if (enableShowError)
                    System.Windows.MessageBox.Show("Không thể tự động sắp phòng thi");

                DataProvider.Ins.DB.USINGEXAMs.Add(ue);
                ListUsingExam.Add(ue);
            }
            else
            {
                DataProvider.Ins.DB.USINGEXAMs.Add(ue_);
                ListUsingExam.Add(ue_);
            }
        }
        #endregion
    }
}
