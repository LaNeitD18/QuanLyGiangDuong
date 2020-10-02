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

        private List<Tuple<string, string>> _tiet;
        
        public List<Tuple<string, string>> tiet
        {
            get
            {
                if (_tiet == null)
                {
                    _tiet = new List<Tuple<string, string>>();

                    for (int i = 0; i < 10; i++)
                        _tiet.Add(new Tuple<string, string>("a" + (i/3).ToString(), (i/3).ToString()));

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

        private table _selectedTB;

        public table selectedTB
        {
            get
            {
                return _selectedTB;
            }

            set 
            { 
                _selectedTB = value; 
            }
        }

        private int _si;

        public int si
        {
            get { return _si; }
            set 
            { 
                _si = value; 
            }
        }
    }
}
