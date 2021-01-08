using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuanLyGiangDuong.Model;

namespace QuanLyGiangDuong.View
{
    /// <summary>
    /// Interaction logic for RoomPage.xaml
    /// </summary>
    public partial class RoomPage : Page
    {
        public RoomPage()
        {
            InitializeComponent();
        }

        private void dataGrid_QueryCoveredRange(object sender, Syncfusion.UI.Xaml.Grid.GridQueryCoveredRangeEventArgs e)
        {
            var range = GetRange(e.GridColumn, e.RowColumnIndex.RowIndex, e.RowColumnIndex.ColumnIndex, e.Record);

            if (range == null)
                return;

            e.Range = range;
            e.Handled = true;
        }

        private CoveredCellInfo GetRange(GridColumn column, int rowIndex, int columnIndex, object rowData)
        {
            IPropertyAccessProvider reflector = null;

            reflector = dataGrid.View.GetPropertyAccessProvider();

            var range = new CoveredCellInfo(columnIndex, columnIndex, rowIndex, rowIndex);
            object data = reflector.GetFormattedValue(rowData, column.MappingName);

            GridColumn leftColumn = null;
            GridColumn rightColumn = null;

            // total rows count.
            int recordsCount = this.dataGrid.GroupColumnDescriptions.Count != 0 ?
            (this.dataGrid.View.TopLevelGroup.DisplayElements.Count + this.dataGrid.TableSummaryRows.Count + this.dataGrid.UnBoundRows.Count + (this.dataGrid.AddNewRowPosition == AddNewRowPosition.Top ? +1 : 0)) :
            (this.dataGrid.View.Records.Count + this.dataGrid.TableSummaryRows.Count + this.dataGrid.UnBoundRows.Count + (this.dataGrid.AddNewRowPosition == AddNewRowPosition.Top ? +1 : 0));

            // Merge Horizontally

            // compare right column               

            for (int i = dataGrid.Columns.IndexOf(column); i < this.dataGrid.Columns.Count - 1; i++)
            {
                var compareData = reflector.GetFormattedValue(rowData, dataGrid.Columns[i + 1].MappingName);

                if (compareData == null)
                    break;

                if (!compareData.Equals(data))
                    break;
                rightColumn = dataGrid.Columns[i + 1];
            }

            // compare left column.

            for (int i = dataGrid.Columns.IndexOf(column); i > 0; i--)
            {
                var compareData = reflector.GetFormattedValue(rowData, dataGrid.Columns[i - 1].MappingName);

                if (compareData == null)
                    break;

                if (!compareData.Equals(data))
                    break;
                leftColumn = dataGrid.Columns[i - 1];
            }

            if (leftColumn != null || rightColumn != null)
            {

                // set left index

                if (leftColumn != null)
                {
                    var leftColumnIndex = this.dataGrid.ResolveToScrollColumnIndex(this.dataGrid.Columns.IndexOf(leftColumn));
                    range = new CoveredCellInfo(leftColumnIndex, range.Right, range.Top, range.Bottom);
                }

                // set right index

                if (rightColumn != null)
                {
                    var rightColumnIndex = this.dataGrid.ResolveToScrollColumnIndex(this.dataGrid.Columns.IndexOf(rightColumn));
                    range = new CoveredCellInfo(range.Left, rightColumnIndex, range.Top, range.Bottom);
                }
                return range;
            }

            // Merge Vertically from the row index.

            int previousRowIndex = -1;
            int nextRowIndex = -1;

            // Get previous row data.                
            var startIndex = dataGrid.ResolveStartIndexBasedOnPosition();

            for (int i = rowIndex - 1; i >= startIndex; i--)
            {
                var previousData = this.dataGrid.GetRecordEntryAtRowIndex(i);

                if (previousData == null || !previousData.IsRecords)
                    break;
                var compareData = reflector.GetFormattedValue((previousData as RecordEntry).Data, column.MappingName);

                if (compareData == null)
                    break;

                if (!compareData.Equals(data))
                    break;
                previousRowIndex = i;
            }

            // get next row data.

            for (int i = rowIndex + 1; i < recordsCount + 1; i++)
            {
                var nextData = this.dataGrid.GetRecordEntryAtRowIndex(i);

                if (nextData == null || !nextData.IsRecords)
                    break;
                var compareData = reflector.GetFormattedValue((nextData as RecordEntry).Data, column.MappingName);

                if (compareData == null)
                    break;

                if (!compareData.Equals(data))
                    break;
                nextRowIndex = i;
            }

            if (previousRowIndex != -1 || nextRowIndex != -1)
            {

                if (previousRowIndex != -1)
                    range = new CoveredCellInfo(range.Left, range.Right, previousRowIndex, range.Bottom);

                if (nextRowIndex != -1)
                    range = new CoveredCellInfo(range.Left, range.Right, range.Top, nextRowIndex);
                return range;
            }
            return null;
        }
    }

    public class UsingToColorBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value;

            if (data == null)
            {
                return new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));
            }
            else if (data is USINGCLASS)
            {
                return new SolidColorBrush(Color.FromArgb(255, 120, 226, 154));
            }
            else if(data is USINGEVENT)
            {
                return new SolidColorBrush(Color.FromArgb(255, 235, 232, 147));
            }
            else if(data is USINGEXAM)
            {
                return new SolidColorBrush(Color.FromArgb(255, 237, 189, 177));
            }

            return new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class UsingToColorBorderBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value;

            if (data == null)
            {
                return new SolidColorBrush(Color.FromArgb(200, 130, 130, 130));
            }
            else if (data is USINGCLASS)
            {
                return new SolidColorBrush(Color.FromArgb(255, 44, 206, 46));
            }
            else if (data is USINGEVENT)
            {
                return new SolidColorBrush(Color.FromArgb(255, 215, 210, 40));
            }
            else if (data is USINGEXAM)
            {
                return new SolidColorBrush(Color.FromArgb(255, 217, 74, 74));
            }

            return new SolidColorBrush(Color.FromArgb(200, 130, 130, 130));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
