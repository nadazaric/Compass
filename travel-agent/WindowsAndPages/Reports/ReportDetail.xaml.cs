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
    /// Interaction logic for ReportDetail.xaml
    /// </summary>
    public partial class ReportDetail : Page
    {
        private new readonly MainWindow Parent;
        private Application App;
        private ReservationService ReservationService;
        public List<TableRow> Rows;
        public Arrangement Arrangement;
        public ReportDetail(MainWindow parent, Arrangement arrangement)
        {
            InitializeComponent();
            Parent = parent;
            App = Application.Current;
            DataContext = this;
            Arrangement = arrangement;
            ReservationService = ReservationService.Instance;
            StartDatePicker.SelectedDate = arrangement.Start;
            EndDatePicker.SelectedDate = arrangement.End;
            Rows = GenerateData(arrangement);
            tableDataGrid.ItemsSource = Rows;
        }

        private List<TableRow> GenerateData(Arrangement ar)
        {
            List<TableRow> data = new List<TableRow>();
            List<Reservation> res = ReservationService.GetReservationsForArrangment(ar);
            if (res == null) 
            {
                data.Add(new TableRow(ar.Name, 0, 0, 0, 0));
                return data; 
            }
            int numRes=0, numCanc=0, numPaid=0;
            decimal total = 0;
            foreach (Reservation reservation in res)
            {
                if (reservation.Status == Reservation.ReservationStatus.RESERVED) numRes++;
                else if (reservation.Status == Reservation.ReservationStatus.CANCELED) numCanc++;
                else if (reservation.Status == Reservation.ReservationStatus.PAID) { numRes++; numPaid++; total+= ar.Price; }
            }
            data.Add(new TableRow(ar.Name, numRes, numCanc, numPaid, total));
            return data;
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
                temp = Arrangement.Name + "_" + temp;
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
}
