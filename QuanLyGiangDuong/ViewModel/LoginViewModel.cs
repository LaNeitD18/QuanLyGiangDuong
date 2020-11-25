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
    public class LoginViewModel : BaseViewModel
    {
        static public LECTURER currentUser; // tao bien static nguoi dung

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

            LoginCommand = new RelayCommand((p) =>
            {
                Window loginWindow = p as Window;
                if (UserName == null || Password == null)
                    MessageBox.Show("Mời nhập tài khoản!");

                ObservableCollection<LECTURER> ListUser = new ObservableCollection<LECTURER>(DataProvider.Ins.DB.LECTURERs);

                foreach (var user in ListUser)
                {
                    if (user.LecturerID == UserName && user.Password_ == Password)
                    {
                        //Gan static TaiKhoanSuDung
                        currentUser = user;
                        //MessageBox.Show("Đăng nhập thành công");
                        loginWindow.Close();
                        return;
                    }
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
