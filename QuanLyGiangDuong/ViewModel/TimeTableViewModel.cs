using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyGiangDuong.ViewModel
{
    public class table
    {
        private string _phong;
        public string phong
        {
            get
            {
                return _phong;
            }
            set { _phong = value; }
        }

        private List<string> _tiet;
        
        public List<string> tiet
        {
            get
            {
                if (_tiet == null)
                {
                    _tiet = new List<string>();

                    for (int i = 0; i < 10; i++)
                        _tiet.Add(i.ToString());
                }

                return _tiet;
            }

            set { _tiet = value; }
        }
    }

    class TimeTableViewModel
    {
        private ObservableCollection<table> _tb;
        public ObservableCollection<table> tb
        {
            get
            {
                if (_tb == null)
                {
                    _tb = new ObservableCollection<table>();

                    table t = new table();
                    t.phong = "B1.01";

                    var x = t.tiet;

                    _tb.Add(t);
                }

                return _tb;
            }

            set { _tb = value; }
        }
    }
}
