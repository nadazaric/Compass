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
		}

		private void SetUpReservations()
		{
			
			futureList = ReservationService.GetAllForUser(parent.User, true).OrderBy(o => o.Arrangement.Start).ToList();
			historyList = ReservationService.GetAllForUser(parent.User, false).OrderByDescending(o => o.Arrangement.Start).ToList();
			Future = new ObservableCollection<Reservation>(futureList);
			History = new ObservableCollection<Reservation>(historyList);

		}

		private void OnTripItemClick(object sender, EventArgs e) { }

		
	}
}
