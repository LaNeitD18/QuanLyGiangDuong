using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

using QuanLyGiangDuong.ViewModel;
using QuanLyGiangDuong.View;
using Syncfusion.UI.Xaml.Grid;
using System.Windows;

using QuanLyGiangDuong.Utilities;
using System.Windows.Threading;
using QuanLyGiangDuong.Model;
using System.Collections.ObjectModel;

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

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { _Title = value; OnPropertyChanged(); }
        }

        private Visibility _IsVisibleCanvas;
        public Visibility IsVisibleCanvas
        {
            get { return _IsVisibleCanvas; }
            set { _IsVisibleCanvas = value; OnPropertyChanged(); }
        }

        private int _FrameColumn;
        public int FrameColumn
        {
            get { return _FrameColumn; }
            set { _FrameColumn = value; OnPropertyChanged(); }
        }

        private bool _IsHomeEnabled;
        public bool IsHomeEnabled
        {
            get { return _IsHomeEnabled; }
            set { _IsHomeEnabled = value; OnPropertyChanged(); }
        }

        private Visibility _IsHomeVisible;
        public Visibility IsHomeVisible
        {
            get { return _IsHomeVisible; }
            set { _IsHomeVisible = value; OnPropertyChanged(); }
        }

        private bool _IsTimetableEnabled;
        public bool IsTimetableEnabled
        {
            get { return _IsTimetableEnabled; }
            set { _IsTimetableEnabled = value; OnPropertyChanged(); }
        }

        private Visibility _IsTimetableVisible;
        public Visibility IsTimetableVisible
        {
            get { return _IsTimetableVisible; }
            set { _IsTimetableVisible = value; OnPropertyChanged(); }
        }

        private bool _IsRoomEnabled;
        public bool IsRoomEnabled
        {
            get { return _IsRoomEnabled; }
            set { _IsRoomEnabled = value; OnPropertyChanged(); }
        }

        private Visibility _IsRoomVisible;
        public Visibility IsRoomVisible
        {
            get { return _IsRoomVisible; }
            set { _IsRoomVisible = value; OnPropertyChanged(); }
        }

        private bool _IsTimetableInputEnabled;
        public bool IsTimetableInputEnabled
        {
            get { return _IsTimetableInputEnabled; }
            set { _IsTimetableInputEnabled = value; OnPropertyChanged(); }
        }

        private Visibility _IsTimetableInputVisible;
        public Visibility IsTimetableInputVisible
        {
            get { return _IsTimetableInputVisible; }
            set { _IsTimetableInputVisible = value; OnPropertyChanged(); }
        }

        private bool _IsEventInputEnabled;
        public bool IsEventInputEnabled
        {
            get { return _IsEventInputEnabled; }
            set { _IsEventInputEnabled = value; OnPropertyChanged(); }
        }

        private Visibility _IsEventInputVisible;
        public Visibility IsEventInputVisible
        {
            get { return _IsEventInputVisible; }
            set { _IsEventInputVisible = value; OnPropertyChanged(); }
        }

        private bool _IsExamInputEnabled;
        public bool IsExamInputEnabled
        {
            get { return _IsExamInputEnabled; }
            set { _IsExamInputEnabled = value; OnPropertyChanged(); }
        }

        private Visibility _IsExamInputVisible;
        public Visibility IsExamInputVisible
        {
            get { return _IsExamInputVisible; }
            set { _IsExamInputVisible = value; OnPropertyChanged(); }
        }

        private bool _IsRoomManagementEnabled;
        public bool IsRoomManagementEnabled
        {
            get { return _IsRoomManagementEnabled; }
            set { _IsRoomManagementEnabled = value; OnPropertyChanged(); }
        }

        private Visibility _IsRoomManagementVisible;
        public Visibility IsRoomManagementVisible
        {
            get { return _IsRoomManagementVisible; }
            set { _IsRoomManagementVisible = value; OnPropertyChanged(); }
        }

        private bool _IsReportEnabled;
        public bool IsReportEnabled
        {
            get { return _IsReportEnabled; }
            set { _IsReportEnabled = value; OnPropertyChanged(); }
        }

        private Visibility _IsReportVisible;
        public Visibility IsReportVisible
        {
            get { return _IsReportVisible; }
            set { _IsReportVisible = value; OnPropertyChanged(); }
        }

        static public DispatcherTimer _timer;

        public bool isLoaded = false;
        #endregion

        #region ICommand
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand Home_Page_SelectedCommand { get; set; }
        public ICommand TimeTable_Page_SelectedCommand { get; set; }
        public ICommand Room_Page_SelectedCommand { get; set; }
        public ICommand TimeTableInput_Page_SelectedCommand { get; set; }
        public ICommand EventInput_Page_SelectedCommand { get; set; }
        public ICommand ExamInput_Page_SelectedCommand { get; set; }
        public ICommand RoomManagement_Page_SelectedCommand { get; set; }
        public ICommand Report_Page_SelectedCommand { get; set; }
        public ICommand Menu_Click_Command { get; set; }
        public ICommand Canvas_MouseLeave_Command { get; set; }


        #endregion

        #region Functions
        private void DisableButtons()
        {
            // disable and hidden all buttons
            IsHomeEnabled = IsTimetableEnabled = IsRoomEnabled = IsTimetableInputEnabled = IsEventInputEnabled = 
                IsExamInputEnabled = IsRoomManagementEnabled = IsReportEnabled = false;
            IsHomeVisible = IsTimetableVisible = IsRoomVisible = IsTimetableInputVisible = IsEventInputVisible =
                IsExamInputVisible = IsRoomManagementVisible = IsReportVisible = Visibility.Hidden;
        }

        private void InitButton(int featureID)
        {
            switch (featureID)
            {
                case 1:
                    IsHomeEnabled = true;
                    IsHomeVisible = Visibility.Visible;
                    break;
                case 2:
                    IsTimetableEnabled = true;
                    IsTimetableVisible = Visibility.Visible;
                    break;
                case 3:
                    IsRoomEnabled = true;
                    IsRoomVisible = Visibility.Visible;
                    break;
                case 4:
                    IsTimetableInputEnabled = true;
                    IsTimetableInputVisible = Visibility.Visible;
                    break;
                case 5:
                    IsEventInputEnabled = true;
                    IsEventInputVisible = Visibility.Visible;
                    break;
                case 6:
                    IsExamInputEnabled = true;
                    IsExamInputVisible = Visibility.Visible;
                    break;
                case 7:
                    IsRoomManagementEnabled = true;
                    IsRoomManagementVisible = Visibility.Visible;
                    break;
                case 8:
                    IsReportEnabled = true;
                    IsReportVisible = Visibility.Visible;
                    break;
            }
        }

        private void InitButtonsForUsing(LECTURER user)
        {
            DisableButtons();
            ObservableCollection<PERMISSION> listPermission = new ObservableCollection<PERMISSION>(DataProvider.Ins.DB.PERMISSIONs);
            foreach (var item in listPermission)
            {
                if (item.LectureTypeID == user.LecturerTypeID)
                {
                    InitButton(item.FeatureID);
                }
            }
        }
        #endregion

        public MainViewModel()
        {
            LoadedWindowCommand = new RelayCommand((p) => {
                if (p == null) 
                    MessageBox.Show(p.GetType().Name);
                Window mainWindow = p as Window;
                mainWindow.Hide(); // main view hide in login window
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
                isLoaded = true;

                //if (loginWindow.DataContext == null) return;
                //var loginVM = loginWindow.DataContext as LoginViewModel;
                //if (loginVM.isLogin)
                //{
                mainWindow.Show();
                //    LoadRemainsData(); // show remain table
                //}
                //else
                //{

                //}

                _timer = new DispatcherTimer(DispatcherPriority.Render);
                _timer.Interval = TimeSpan.FromSeconds(1);
                _timer.Tick += (sender, args) =>
                {
                    if (LoginViewModel.currentUser != null)
                    {
                        InitButtonsForUsing(LoginViewModel.currentUser);

                        _timer.Stop();
                    }
                };
                _timer.Start();

                FrameContent = new HomePage();

            });

            IsVisibleCanvas = Visibility.Hidden;
            FrameColumn = 1;

            // initial page
            Title = "Trang chủ";
            FrameContent = new HomePage();

            // commands
            Home_Page_SelectedCommand = new RelayCommand((p) => {
                Title = "Trang chủ";
                FrameContent = new HomePage();
                FrameContent.DataContext = new HomePage_ViewModel();
            });

            TimeTable_Page_SelectedCommand = new RelayCommand((p) => {
                Title = "Lịch dạng TKB";
                FrameContent = new TimeTablePage();
                FrameContent.DataContext = new TimeTableViewModel();
            });

            Room_Page_SelectedCommand = new RelayCommand((p) => {
                Title = "Lịch dạng phòng";
                FrameContent = new RoomPage();
                FrameContent.DataContext = new RoomViewModel();
            });

            TimeTableInput_Page_SelectedCommand = new RelayCommand((p) => {
                Title = "Nhập phòng học";
                FrameContent = new TimeTableInputPage();
                FrameContent.DataContext = new TimeTableInputViewModel();
            });

            EventInput_Page_SelectedCommand = new RelayCommand((p) => {
                Title = "Nhập sự kiện";
                FrameContent = new EventInputPage();
                FrameContent.DataContext = new EventInputViewModel();
            });

            ExamInput_Page_SelectedCommand = new RelayCommand((p) => {
                Title = "Nhập phòng thi";
                FrameContent = new ExamInputPage();
                FrameContent.DataContext = new ExamInputViewModel();
            });

            RoomManagement_Page_SelectedCommand = new RelayCommand((p) => {
                Title = "Quản lý phòng";
                FrameContent = new RoomManagementPage();
                FrameContent.DataContext = new RoomManagementViewModel();
            });

            Report_Page_SelectedCommand = new RelayCommand((p) => {
                Title = "Lập báo cáo";
                FrameContent = new ReportPage();
                FrameContent.DataContext = new ReportViewModel();
            });

            Menu_Click_Command = new RelayCommand((p) => {
                IsVisibleCanvas = Visibility.Visible;
                FrameColumn = 6;
            });

            Canvas_MouseLeave_Command = new RelayCommand((p) => {
                IsVisibleCanvas = Visibility.Hidden;
                FrameColumn = 1;
            });
        }
    }
}
