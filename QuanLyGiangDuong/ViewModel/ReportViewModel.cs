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
        private void LoadData() {
            // pie chart
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

            // bar chart
            int classRooms = DataProvider.Ins.DB.USINGCLASSes.Count();
            int eventRooms = DataProvider.Ins.DB.USINGEVENTs.Count();
            int examRooms = DataProvider.Ins.DB.USINGEXAMs.Count();
            PointLabel = chartPoint => string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            ColumnSeries = new SeriesCollection {
                new ColumnSeries {
                    Title = "Mục đích sử dụng phòng",
                    Values = new ChartValues<int>{classRooms, eventRooms, examRooms},
                    LabelPoint = PointLabel
                }
            };
        }
        #endregion

        #region ICommand

        #endregion

        public ReportViewModel()
        {
            LoadData();
        }
    }
}
