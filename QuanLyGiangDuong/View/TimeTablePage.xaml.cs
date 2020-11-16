using System;
using System.Collections.Generic;
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
using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Helpers;
using QuanLyGiangDuong.ViewModel;
using System.Globalization;

namespace QuanLyGiangDuong.View
{
    /// <summary>
    /// Interaction logic for TimeTablePage.xaml
    /// </summary>
    public partial class TimeTablePage : Page
    {
        public TimeTablePage()
        {
            InitializeComponent();
        }


        private void sfDataGrid1_QueryCoveredRange(object sender, GridQueryCoveredRangeEventArgs e)
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int rowindex = 0;
            //Get selectd cells data
            var selectedcells = this.dataGrid.GetSelectedCells();
            //calculate the total number of selected cell in dataGrid
            int count = selectedcells.Count;
            for (int i = 0; i < count; i++)
            {
                GridCellInfo cellinfo = selectedcells[i];
                //Get selected cell row index
                if (!cellinfo.IsDataRowCell)
                    continue;
                rowindex = this.dataGrid.ResolveToRowIndex(cellinfo.RowData);
                var gridColumnIndex = this.dataGrid.Columns.IndexOf(cellinfo.Column);
                //Resolve the column index
                var visibleColumnIndex = this.dataGrid.ResolveToGridVisibleColumnIndex(gridColumnIndex);
                //Get the SelectedCell values
                var propertyCollection = this.dataGrid.View.GetPropertyAccessProvider();
                var cellvalue = propertyCollection.GetValue(cellinfo.RowData, cellinfo.Column.MappingName);
            }
        }

        private void BtnGetTimetable_Click(object sender, RoutedEventArgs args)
        {
            TimeTableViewModel vm = DataContext as TimeTableViewModel;

            try
            {
                vm.GetTimeTable();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }

    public class ValueToColorBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Tuple<string, string> data = value as Tuple<string, string>;

            if(data == null)
            {
                return new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));
            }
            else
            {
                return new SolidColorBrush(Colors.White);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ValueToColorBorderBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Tuple<string, string> data = value as Tuple<string, string>;

            if (data != null)
            {
                return new SolidColorBrush(Colors.LightGreen);
            }
            else
            {
                return new SolidColorBrush(Colors.LightGreen);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
