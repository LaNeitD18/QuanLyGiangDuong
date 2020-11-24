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
using LiveCharts;
using LiveCharts.Wpf;

namespace QuanLyGiangDuong.ViewModel
{
    class ReportViewModel : BaseViewModel
    {
        #region Variables
        private ObservableCollection<int> _ListMonth;
        public ObservableCollection<int> ListMonth
        {
            get { return _ListMonth; }
            set { _ListMonth = value; OnPropertyChanged(); }
        }

        private int _SelectedMonth;
        public int SelectedMonth
        {
            get { return _SelectedMonth; }
            set { _SelectedMonth = value; OnPropertyChanged(); }
        }

        private ObservableCollection<int> _ListYear;
        public ObservableCollection<int> ListYear
        {
            get { return _ListYear; }
            set { _ListYear = value; OnPropertyChanged(); }
        }

        private int _SelectedYear;
        public int SelectedYear
        {
            get { return _SelectedYear; }
            set { _SelectedYear = value; OnPropertyChanged(); }
        }

        private int _UnusedRooms;
        public int UnusedRooms
        {
            get { return _UnusedRooms; }
            set { _UnusedRooms = value; OnPropertyChanged(); }
        }

        private int _EmptyRooms;
        public int EmptyRooms
        {
            get { return _EmptyRooms; }
            set { _EmptyRooms = value; OnPropertyChanged(); }
        }

        private int _UsingRooms;
        public int UsingRooms
        {
            get { return _UsingRooms; }
            set { _UsingRooms = value; OnPropertyChanged(); }
        }

        private SeriesCollection _PieSeries;
        public SeriesCollection PieSeries
        {
            get { return _PieSeries; }
            set { _PieSeries = value; OnPropertyChanged(); }
        }

        private SeriesCollection _ColumnSeries;
        public SeriesCollection ColumnSeries
        {
            get { return _ColumnSeries; }
            set { _ColumnSeries = value; OnPropertyChanged(); }
        }

        private Func<ChartPoint, string> _PointLabel;
        public Func<ChartPoint, string> PointLabel
        {
            get { return _PointLabel; }
            set { _PointLabel = value; OnPropertyChanged(); }
        }
        #endregion

        #region Function
        // load full ko load theo thang, nam
        private void LoadPieChartInput() {
            UnusedRooms = DataProvider.Ins.DB.ROOMs.Where(x => x.Status_ == "Không còn sử dụng").Count();
            EmptyRooms = DataProvider.Ins.DB.ROOMs.Where(x => x.Status_ == "Còn sử dụng").Count();
            UsingRooms = DataProvider.Ins.DB.ROOMs.Where(x => x.Status_ == "Đang sử dụng").Count();

            PieSeries = new SeriesCollection {
                new PieSeries {
                    Title = "Phòng không còn sử dụng",
                    Values = new ChartValues<int>{ UnusedRooms }
                },
                new PieSeries {
                    Title = "Phòng còn sử dụng",
                    Values = new ChartValues<int>{ EmptyRooms }
                },
                new PieSeries {
                    Title = "Phòng đang sử dụng",
                    Values = new ChartValues<int>{ UsingRooms }
                }
            };
        }

        private void LoadBarChartInput() {
            int classRooms = DataProvider.Ins.DB.USINGCLASSes.Count();
            int eventRooms = DataProvider.Ins.DB.USINGEVENTs.Where(x => x.Date_.Month == SelectedMonth && x.Date_.Year == SelectedYear).Count();
            int examRooms = DataProvider.Ins.DB.USINGEXAMs.Where(x => x.Date_.Month == SelectedMonth && x.Date_.Year == SelectedYear).Count();
            PointLabel = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            ColumnSeries = new SeriesCollection {
                new ColumnSeries {
                    Title = "Mục đích sử dụng phòng",
                    Values = new ChartValues<int>{classRooms, eventRooms, examRooms},
                    LabelPoint = PointLabel
                }
            };
        }

        private void LoadData() {
            // combobox select month & year
            ListMonth = new ObservableCollection<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};
            SelectedMonth = DateTime.Now.Month;

            ListYear = new ObservableCollection<int>();
            for(int i=DateTime.Now.Year; i>=2000; i--) {
                ListYear.Add(i);
            }
            SelectedYear = DateTime.Now.Year;

            // pie chart
            LoadPieChartInput();

            // bar chart
            LoadBarChartInput();
        }
        #endregion

        #region ICommand
        public ICommand CreateReportCommand { get; set; }
        #endregion

        public ReportViewModel()
        {
            LoadData();

            CreateReportCommand = new RelayCommand((p) => {
                LoadBarChartInput();
            });
        }
    }
}
