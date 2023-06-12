using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
using travel_agent.Models;
using travel_agent.Services;

namespace travel_agent.WindowsAndPages.Reports
{
    /// <summary>
    /// Interaction logic for MonthReport.xaml
    /// </summary>
    public partial class MonthReport : Page
    {
        private new readonly MainWindow Parent;
        private Application App;
        private ArrangementService ArrangementService;
        private ReservationService ReservationService;
        public List<TableRow> Rows;
        public MonthReport(MainWindow parent)
        {
            InitializeComponent();
            Parent = parent;
            App = Application.Current;
            DataContext = this;
            ArrangementService = ArrangementService.Instance;
            ReservationService = ReservationService.Instance;
            StartDatePicker.SelectedDate = DateTime.Now.AddDays(-30);
            EndDatePicker.SelectedDate = DateTime.Now;
            //Rows = GenerateTable(DateTime.Now.AddDays(-30), DateTime.Now);
            Rows = GenerateTable(DateTime.Now, DateTime.Now.AddDays(30));
            tableDataGrid.ItemsSource = Rows; 

        }

        private List<TableRow> GenerateTable(DateTime start, DateTime end)
        {
            List<TableRow> rows = new List<TableRow>();
            List<Arrangement> arrangements = new List<Arrangement>();
            arrangements = ArrangementService.GetAll();
            foreach (Arrangement ar in arrangements)
            {
                int numRes=0, numCancel=0, numDel=0;
                decimal total = 0;

                List<Reservation> reservations = new List<Reservation>();
                // reservations = ReservationService.GetReservationsForMontForArrangement(DateTime.Now.AddDays(-30), DateTime.Now, ar);
                reservations = ReservationService.GetReservationsForMontForArrangement(start, end, ar);
                if (reservations == null) return null;
                foreach(Reservation reservation in reservations)
                {
                    if (reservation.Status == Reservation.ReservationStatus.RESERVED) numRes++;
                    else if (reservation.Status == Reservation.ReservationStatus.CANCELED) numCancel++;
                    else if (reservation.Status == Reservation.ReservationStatus.PAID) { total += ar.Price; numDel++; numRes++; }
                    else { numDel++; }
                }
                rows.Add(new TableRow(ar.Name, numRes, numCancel, numDel, total));  
            }
            return rows;
        }

        public void GenerateReportButton(object sender, EventArgs e)
        {
            Rows = GenerateTable((DateTime)StartDatePicker.SelectedDate, (DateTime)EndDatePicker.SelectedDate);
            foreach (TableRow r in Rows) { Console.WriteLine(r.ToString()); }
            tableDataGrid.ItemsSource = Rows;
        }

        public void DownloadReportData(Object sender, RoutedEventArgs e)
        {
            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Set the column headers
                for (int i = 0; i < tableDataGrid.Columns.Count; i++)
                {
                    var columnHeader = tableDataGrid.Columns[i].Header.ToString();
                    worksheet.Cells[1, i + 1].Value = columnHeader;
                }
                var itemsSource = (IEnumerable)tableDataGrid.ItemsSource;
                var row = 2;
                var col = 1;
                foreach (var item in itemsSource)
                {
                    foreach (var property in item.GetType().GetProperties())
                    {
                        var value = property.GetValue(item);
                        worksheet.Cells[row, col].Value = value;
                        col++;
                    }
                    row++;
                    col = 1;
                }
                // Save the Excel file
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                string temp = DateTime.Now.ToString().Replace(':', '_').Replace('/', '_').Replace(' ', '_');
                saveFileDialog.FileName = "Report_" + temp + ".xlsx";
                saveFileDialog.DefaultExt = ".xlsx";
                saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                var result = saveFileDialog.ShowDialog();
                if (result == true)
                {
                    var filePath = saveFileDialog.FileName;
                    package.SaveAs(new FileInfo(filePath));
                }
            }
        }

        public void OnBackClick(object sender, EventArgs e) => Parent.MainFrame.Content = new Statistics(Parent);
    }

    public class TableRow
    {
        public string Name { get; set; }
        public int NumberReserved { get; set; }
        public int NumberCanceled { get; set; }
        public int NumberDeleted { get; set; }
        public decimal TotalPrice { get; set; }
        public TableRow(string name, int numberReserved, int numberCanceled, int numDel, decimal totalPrice)
        {
            Name = name;
            NumberReserved = numberReserved;
            NumberCanceled = numberCanceled;
            NumberDeleted = numDel;
            TotalPrice = totalPrice;
        }
        public override string ToString()
        {
            return Name + " " + NumberReserved + " " + NumberCanceled + " " + NumberDeleted;
        }
    }
}
