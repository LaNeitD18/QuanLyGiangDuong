using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace QuanLyGiangDuong.ViewModel
{
    public class HomePage_ViewModel : BaseViewModel
    {
        #region Variables
        private string _CurrentDateTime;

        public string CurrentDateTime
        {
            get { return _CurrentDateTime; }
            set {
                if (_CurrentDateTime == value)
                    return;
                _CurrentDateTime = value; OnPropertyChanged(); 
            }
        }

        public DispatcherTimer _timer;
        #endregion

        #region Functions
        private void GetTimeNow()
        {
            CurrentDateTime = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
            // Copy from https://stackoverflow.com/questions/31160201/time-ticking-in-c-sharp-wpf-mvvm
            _timer = new DispatcherTimer(DispatcherPriority.Render);
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (sender, args) =>
            {
                //Khởi tạo tài khoản cho toàn bộ chương trình
                //InitTaiKhoan();
                CurrentDateTime = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
            };
            _timer.Start();
        }

        private void InitTaiKhoan()
        {
            
        }
        #endregion

        public HomePage_ViewModel()
        {
            GetTimeNow();
        }
    }
}
