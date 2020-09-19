using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

using QuanLyGiangDuong.ViewModel;
using QuanLyGiangDuong.View;

namespace QuanLyGiangDuong.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        #region Variables
        private Page _FrameContent;

        public Page FrameContent
        {
            get { return _FrameContent; }
            set { _FrameContent = value; OnPropertyChanged(); }
        }
        #endregion

        #region ICommand
        public ICommand Home_Page_SelectedCommand { get; set; }
        public ICommand TimeTableInput_Page_SelectedCommand { get; set; }
        public ICommand EventInput_Page_SelectedCommand { get; set; }
        public ICommand Room_Page_SelectedCommand { get; set; }
        public ICommand TimeTable_Page_SelectedCommand { get; set; }


        #endregion

        public MainViewModel()
        {
            Home_Page_SelectedCommand = new RelayCommand((p) => {
                FrameContent = new HomePage();
                FrameContent.DataContext = new HomePage_ViewModel();
            });

            TimeTable_Page_SelectedCommand = new RelayCommand((p) => {
                FrameContent = new TimeTablePage();
                FrameContent.DataContext = new TimeTableViewModel();
            });

            Room_Page_SelectedCommand = new RelayCommand((p) => {
                FrameContent = new RoomPage();
                FrameContent.DataContext = new RoomViewModel();
            });

            TimeTableInput_Page_SelectedCommand = new RelayCommand((p) =>
            {
                FrameContent = new TimeTableInputPage();
                FrameContent.DataContext = new TimeTableInputViewModel();
            });

            EventInput_Page_SelectedCommand = new RelayCommand((p) => {
                FrameContent = new EventInputPage();
                FrameContent.DataContext = new EventInputViewModel();
            });
        }
    }
}
