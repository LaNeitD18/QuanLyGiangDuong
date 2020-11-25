using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows;

namespace QuanLyGiangDuong.Utilities
{
    static class MsExcelReader
    {
        /// <summary>
        /// source: copy and adjust from http://csharp.net-informations.com/excel/csharp-read-excel.htm
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name=""></param>
        /// <returns> returns a 2D list of string that contains the data in the excel file </returns>
        static public List<List<string>> Read(string filepath)
        {
            var result = new List<List<string>>();

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Range range;

            string str;
            int rCnt;
            int cCnt;
            int rw = 0;
            int cl = 0;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(filepath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            range = xlWorkSheet.UsedRange;
            rw = range.Rows.Count;
            cl = range.Columns.Count;

            for (rCnt = 1; rCnt <= rw; rCnt++)
            {
                List<string> rowTempData = new List<string>();

                for (cCnt = 1; cCnt <= cl; cCnt++)
                {
                    str = ( (range.Cells[rCnt, cCnt] as Excel.Range).Value2 ).ToString();
                    str = str.Trim();
                    rowTempData.Add(string.IsNullOrEmpty(str)? "":str);
                }

                result.Add(rowTempData);
            }

            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

            return result;
        }
    }
}
