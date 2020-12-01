using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using QuanLyGiangDuong.Model;

namespace QuanLyGiangDuong.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        static public int TaiKhoanSuDung = 0; // tao bien static nguoi dung

        public ICommand CloseWindowCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }

        private string _UserName;
        public string UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }
        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }

        public LoginViewModel()
        {
            //DatabaseCheck.Ins.Check();
            UserName = "";
            Password = "";

            #region Demo Auto schedule using class
            /*
            CLASS newClass = new CLASS();
            newClass.ClassID = "abc";
            newClass.StartDate = new DateTime(2020, 11, 2);
            newClass.EndDate = new DateTime(2021, 1, 31);
            newClass.Population_ = 90;
            newClass.LecturerID = "1";

            USINGCLASS newUsingClass = new USINGCLASS();
            newUsingClass.UsingClassID = "xyz";
            newUsingClass.ClassID = newClass.ClassID;
            newUsingClass.Duration = new TimeSpan(1, 30, 0);
            newUsingClass.RepeatCycle = 1;
            newUsingClass.Day_ = 1;
            newUsingClass.StartDate = new DateTime(2020, 11, 2);
            newUsingClass.EndDate = new DateTime(2021, 1, 31);

            ROOM ChoosenRoom = null;
            ChoosenRoom = DataProvider.Ins.DB.ROOMs.Find("B6.06");

            var result = Utilities.Utils.AutoMakeSchedule(newUsingClass, newClass, ChoosenRoom, null, 2);
            */
            #endregion

            #region Demo Auto schedule using exam

            //EXAM newExam = new EXAM();
            //newExam.Population_ = 120;

            //USINGEXAM newUsingExam = new USINGEXAM();
            //newUsingExam.Duration = new TimeSpan(1, 30, 0);

            //DateTime startDate = new DateTime(2020, 11, 23);
            //DateTime endDate = new DateTime(2020, 11, 29);

            //var result = Utilities.Utils.AutoMakeExam(newUsingExam, newExam, startDate, endDate);

            #endregion

            LoginCommand = new RelayCommand((p) =>
            {
                Window loginWindow = p as Window;
                if (UserName == null || Password == null)
                    MessageBox.Show("Mời nhập tài khoản!");

                //ObservableCollection<NGUOIDUNG> Account = new ObservableCollection<NGUOIDUNG>(DataProvider.Ins.DB.NGUOIDUNGs);

                //foreach (var item in Account)
                //{
                //    if (item.TenDangNhap == UserName && item.MatKhau == Password)
                //    {
                //        //Gan static TaiKhoanSuDung
                //        TaiKhoanSuDung = item;
                //        //MessageBox.Show("Đăng nhập thành công");
                //        p.Close();
                //        return;
                //    }
                //}

                if (UserName == "admin" && Password == "admin")
                {
                    //MessageBox.Show("Đăng nhập thành công");
                    TaiKhoanSuDung = 1;
                    loginWindow.Close();
                    return;
                }
                MessageBox.Show("Tài khoản không hợp lệ!");

                //CheckLogin(p);
            });

            CloseWindowCommand = new RelayCommand((p) => {
                Window loginWindow = p as Window;

                loginWindow.Close();
                System.Environment.Exit(1);
            });

            PasswordChangedCommand = new RelayCommand((p) =>
            {
                PasswordBox passwordBox = p as PasswordBox;
                Password = passwordBox.Password;
            });
        }
    }
}
