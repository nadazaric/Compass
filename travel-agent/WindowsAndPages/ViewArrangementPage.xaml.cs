using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
	/// Interaction logic for ViewArrangementPage.xaml
	/// </summary>
	public partial class ViewArrangementPage : Page
	{

		private Arrangement Arrangement;
		private Reservation Reservation;
		private bool isFromTrips = true;
		private MainWindow parent;
		private ReservationService reservationService;
		private Application App;

		public ViewArrangementPage(MainWindow parent, Arrangement arrangement, Reservation reservation = null)
		{
			InitializeComponent();
			this.parent = parent;
			this.Arrangement = arrangement;
			this.Reservation = reservation;
			DataContext = this.Arrangement;
			reservationService = ReservationService.Instance;
			App = Application.Current;

			if(reservation == null)
			{
				CheckUserReservations();
				isFromTrips=false;
			}
			SetUpButtons();
			SetUpMap();
		}

		

		private void CheckUserReservations()
		{
			Reservation = reservationService.GetUserReservation(parent.User, Arrangement);
		}

		private void SetUpMap()
		{
			foreach (var step in Arrangement.Steps)
			{
				Map.DrawPinForRoute(step.StartPlace);
			}
			Place last = Arrangement.Steps[Arrangement.Steps.Count - 1].EndPlace;
			Map.DrawPinForRoute(last);
			Map.DrawRouteAsync(Arrangement.Steps);
			Map.DisableDoubleClick();
		}

		private void SetUpButtons()
		{
			if(Reservation == null)
			{
				TimeSpan t = Arrangement.Start - DateTime.Now;
				double days = t.TotalDays;
				MakeReservationButton.Visibility = Visibility.Visible;
				PayTripButton.Visibility = Visibility.Collapsed;
				CancelReservationButton.Visibility= Visibility.Collapsed;
				if (days >= 3)
				{
					MakeReservationButton.IsEnabled = true;
				}else if(days < 0)
				{
					MakeReservationButton.Visibility = Visibility.Collapsed;
				}
				else
				{
					MakeReservationButton.IsEnabled = false;
				}
				StatusPanel.Visibility = Visibility.Collapsed;
			}
			else if(Reservation.Status == Reservation.ReservationStatus.RESERVED)
			{
				MakeReservationButton.Visibility= Visibility.Collapsed;
				PayTripButton.Visibility = Visibility.Visible;
				CancelReservationButton.Visibility = Visibility.Visible;
				ReservationStatusLabel.Content = "Rezervisano";
			}
			else if((Reservation.Status == Reservation.ReservationStatus.CANCELED || Reservation.Status == Reservation.ReservationStatus.DELETED) && DateTime.Compare(Arrangement.Start, DateTime.Now) > 0)
			{
				TimeSpan t = Arrangement.Start - DateTime.Now;
				double days = t.TotalDays;
				if (days >= 3)
				{
					MakeReservationButton.Visibility = Visibility.Visible;
				}
				PayTripButton.Visibility = Visibility.Collapsed;
				CancelReservationButton.Visibility = Visibility.Collapsed;
				ReservationStatusLabel.Content = Reservation.Status == Reservation.ReservationStatus.CANCELED ? "Otkazano" : "Istekla rezervacija";
			}
			else if(Reservation.Status == Reservation.ReservationStatus.PAID)
			{
				MakeReservationButton.Visibility= Visibility.Collapsed;
				PayTripButton.Visibility = Visibility.Collapsed;
				CancelReservationButton.Visibility = Visibility.Collapsed;
				
				ReservationStatusLabel.Content = "Plaćeno";
			}
				

			
		}

		private void OnBackClick(object sender, RoutedEventArgs e)  {
			if(!isFromTrips) parent.MainFrame.Content = new ArrangementsPage(parent);
			else parent.MainFrame.Content = new MyTripsPage(parent);
		}

		private void MakeReservationButton_Click(object sender, RoutedEventArgs e)
		{
			var result = MessageBox.Show(App.Resources["String.MakeReservationMessage"] as string, App.Resources["String.AppName"] as string, MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.No) return;
			if (Reservation == null) reservationService.CreateReservation(parent.User, Arrangement);
			else reservationService.RecreateReservation(Reservation);
			Reservation.Status = Reservation.ReservationStatus.RESERVED;
			SetUpButtons();
		}

		private void CancelReservationButton_Click(object sender, RoutedEventArgs e)
		{
			var result = MessageBox.Show(App.Resources["String.CancelReservationMessage"] as string, App.Resources["String.AppName"] as string, MessageBoxButton.YesNo, MessageBoxImage.Question);
			if(result == MessageBoxResult.No) return;
			reservationService.CancelReservationForUser(Reservation);
			Reservation.Status = Reservation.ReservationStatus.CANCELED;
			SetUpButtons();
		}

		private void PayTripButton_Click(object sender, RoutedEventArgs e)
		{
			string message = "Plati putovanje?" + Environment.NewLine + "Nakon izvršenog plaćanja otkazivanje više nije moguće";
			var result = MessageBox.Show(message, App.Resources["String.AppName"] as string, MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.No) return;
			reservationService.PayReservation(Reservation);
			Reservation.Status = Reservation.ReservationStatus.PAID;
			SetUpButtons();
		}
	}
}
