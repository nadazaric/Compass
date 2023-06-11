using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using travel_agent.Models;
using travel_agent.Services;

namespace travel_agent.WindowsAndPages
{
	/// <summary>
	/// Interaction logic for MyTripsPage.xaml
	/// </summary>
	public partial class MyTripsPage : Page
	{

		private readonly MainWindow parent;
		private Application app;
		private ReservationService ReservationService;
		private ArrangementService ArrangementService;
		public ObservableCollection<Reservation> Future { get; set; }
		private List<Reservation> futureList;
		public ObservableCollection<Reservation> History { get; set; }
		private List<Reservation> historyList;


		public MyTripsPage(MainWindow parent)
		{
			InitializeComponent();
			app = Application.Current;
			this.parent = parent;
			ArrangementService = ArrangementService.Instance;
			ReservationService = ReservationService.Instance;
			DataContext = this;

			SetUpReservations();
			SetIfNoContent();
		}

		private void SetUpReservations()
		{
			
			futureList = ReservationService.GetAllForUser(parent.User, true).OrderBy(o => o.Arrangement.Start).ToList();
			historyList = ReservationService.GetAllForUser(parent.User, false).OrderByDescending(o => o.Arrangement.Start).ToList();
			Future = new ObservableCollection<Reservation>(futureList);
			History = new ObservableCollection<Reservation>(historyList);

		}

		private void SetIfNoContent()
		{
			if(Future.Count == 0)
			{
				FutureList.Visibility = Visibility.Collapsed;
				NoContentFuture.Visibility = Visibility.Visible;
			}if(History.Count == 0)
			{
				HistoryList.Visibility = Visibility.Collapsed;
				NoContentHistory.Visibility = Visibility.Visible;
			}
		}

		private void SetStatus(object sender, RoutedEventArgs e)
		{
			
		}

		private void OnTripItemClick(object sender, EventArgs e) { }

		
	}

	public class EnumTranslator : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Reservation.ReservationStatus enumValue = (Reservation.ReservationStatus)value;
			switch(enumValue)
			{
				case Reservation.ReservationStatus.RESERVED:
					return "Rezervisano";
				case Reservation.ReservationStatus.PAID:
					return "Plaćeno";
				case Reservation.ReservationStatus.CANCELED:
					return "Otkazano";
				default:
					return "Istekla rezervacija";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string stringValue = (string)value;
			switch (stringValue)
			{
				case "Rezervisano":
					return Reservation.ReservationStatus.RESERVED;
				case "Plaćeno":
					return Reservation.ReservationStatus.PAID;
				case "Otkazano":
					return Reservation.ReservationStatus.CANCELED;
				default:
					return Reservation.ReservationStatus.DELETED;
			}
		}
	}
}
