using QuanLyGiangDuong.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLyGiangDuong.ViewModel
{
    class ExamInputViewModel : BaseViewModel
    {
        #region Variables
        private ObservableCollection<TRAINING_PROGRAM> _ListTrainingProgram;
        public ObservableCollection<TRAINING_PROGRAM> ListTrainingProgram
        {
            get { return _ListTrainingProgram; }
            set { _ListTrainingProgram = value; OnPropertyChanged(); }
        }

        private TRAINING_PROGRAM _SelectedTrainingProgram;
        public TRAINING_PROGRAM SelectedTrainingProgram
        {
            get { return _SelectedTrainingProgram; }
            set { _SelectedTrainingProgram = value; OnPropertyChanged(); }
        }

        private ObservableCollection<FACAULTY> _ListFacaulty;
        public ObservableCollection<FACAULTY> ListFacaulty
        {
            get { return _ListFacaulty; }
            set { _ListFacaulty = value; OnPropertyChanged(); }
        }

        private FACAULTY _SelectedFacaulty;
        public FACAULTY SelectedFacaulty
        {
            get { return _SelectedFacaulty; }
            set { _SelectedFacaulty = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> _ListSemester;
        public ObservableCollection<string> ListSemester
        {
            get { return _ListSemester; }
            set { _ListSemester = value; OnPropertyChanged(); }
        }

        private ObservableCollection<int> _ListYear;
        public ObservableCollection<int> ListYear
        {
            get { return _ListYear; }
            set { _ListYear = value; OnPropertyChanged(); }
        }

        private string _ExamSubject;
        public string ExamSubject
        {
            get { return _ExamSubject; }
            set { _ExamSubject = value; OnPropertyChanged(); }
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
        #endregion

        #region Function
        private void LoadData()
        {
            var listTrainingProgram = DataProvider.Ins.DB.TRAINING_PROGRAM;
            ListTrainingProgram = new ObservableCollection<TRAINING_PROGRAM>(listTrainingProgram);

            var listFacaulty = DataProvider.Ins.DB.FACAULTies;
            ListFacaulty = new ObservableCollection<FACAULTY>(listFacaulty);

            ListSemester = new ObservableCollection<string>();
            ListSemester.Add("Học kỳ I");
            ListSemester.Add("Học kỳ II");
            ListSemester.Add("Học kỳ hè");

            ListYear = new ObservableCollection<int>();
            for(int i=2000; i<=DateTime.Now.Year; i++)
            {
                ListYear.Add(i);
            }
        }
        #endregion

        #region ICommand

        #endregion

        public ExamInputViewModel()
        {
            LoadData();

            
        }
    }
}
