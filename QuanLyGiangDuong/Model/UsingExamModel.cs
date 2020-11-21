using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QuanLyGiangDuong.Model
{
    public partial class USINGEXAM : IUsing
    {
        private string _displayName;
        public string DisplayName
        {
            get
            {
                _displayName = GetDisplayString();
                return _displayName;
            }

            set
            {
                _displayName = value;
            }
        }

        public string GetDisplayString()
        {
            return ExamID;
        }
    }
}