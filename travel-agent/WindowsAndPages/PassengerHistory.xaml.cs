using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace travel_agent.WindowsAndPages
{
    /// <summary>
    /// Interaction logic for PassengerHistory.xaml
    /// </summary>
    public partial class PassengerHistory : Page
    {
        private readonly MainWindow parent;
        private Application app;
        private ReservationService reservationService;
        public PassengerHistory() { }
        public ObservableCollection<Reservation> Reservations{ get; set; }
        private List<Reservation> ReservationsList {  get; set; }
        private List<Arrangement> ArrangementsList { get; set; }
        public PassengerHistory(MainWindow parentWindow)
        {
            InitializeComponent();
            parent = parentWindow;
            reservationService = ReservationService.Instance;
            DataContext = this;
            app = Application.Current;
            SetUpReservations();
        }

        private void SetUpReservations()
        {
            Reservations = new ObservableCollection<Reservation>();
            ReservationsList = reservationService.GetAllForUser(parent.User);
            foreach (Reservation reservation in Reservations) ArrangementsList.Add(reservation.Arrangement);

        }
    }
}
