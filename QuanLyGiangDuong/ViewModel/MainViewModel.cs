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
        public ICommand Menu_Click_Command { get; set; }
        public ICommand Canvas_MouseLeave_Command { get; set; }


        #endregion

        #region Functions
        private void ChangePage(Page currentPage, BaseViewModel currentViewModel)
        {
            FrameContent = currentPage;
            FrameContent.DataContext = currentViewModel;
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
                    if (LoginViewModel.TaiKhoanSuDung != 0)
                    {
                        //Init_Button_User(LoginViewModel.TaiKhoanSuDung);

                        _timer.Stop();
                    }
                };
                _timer.Start();

                FrameContent = new HomePage();

            });

            // CuteTN: Initialize database for the first use of local DB Server, because there are some fake-Null-entries
            Utils.InitDatabase();

            IsVisibleCanvas = Visibility.Hidden;
            FrameColumn = 1;

            // initial page
            FrameContent = new HomePage();

            // commands
            Home_Page_SelectedCommand = new RelayCommand((p) => {
                FrameContent = new HomePage();
            });

            TimeTable_Page_SelectedCommand = new RelayCommand((p) => {
                FrameContent = new TimeTablePage();
            });

            Room_Page_SelectedCommand = new RelayCommand((p) => {
                FrameContent = new RoomPage();
            });

            TimeTableInput_Page_SelectedCommand = new RelayCommand((p) => {
                FrameContent = new TimeTableInputPage();
            });

            EventInput_Page_SelectedCommand = new RelayCommand((p) => {
                FrameContent = new EventInputPage();
            });

            ExamInput_Page_SelectedCommand = new RelayCommand((p) => {
                FrameContent = new ExamInputPage();
            });

            RoomManagement_Page_SelectedCommand = new RelayCommand((p) => {
                FrameContent = new RoomManagementPage();
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
