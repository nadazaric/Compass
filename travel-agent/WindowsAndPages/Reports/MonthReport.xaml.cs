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
        public MonthReport(MainWindow parent)
        {
            InitializeComponent();
            Parent = parent;
            App = Application.Current;
            DataContext = this;
            ArrangementService = ArrangementService.Instance;
            ReservationService = ReservationService.Instance;
            tableDataGrid.ItemsSource = GenerateTable();
        }

        private List<TableRow> GenerateTable()
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
                reservations = ReservationService.GetReservationsForMontForArrangement(DateTime.Now, DateTime.Now.AddDays(30), ar);
                if (reservations == null) return null;
                foreach(Reservation reservation in reservations)
                {
                    if (reservation.Status == Reservation.ReservationStatus.RESERVED) numRes++;
                    else if (reservation.Status == Reservation.ReservationStatus.CANCELED) numCancel++;
                    else if (reservation.Status == Reservation.ReservationStatus.PAID) { total += ar.Price; numRes++; }
                    else { numDel++; }
                }
                rows.Add(new TableRow(ar.Name, numRes, numCancel, numDel, total));  
            }
            return rows;
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
    }
}
