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

namespace travel_agent.WindowsAndPages
{
	/// <summary>
	/// Interaction logic for ViewArrangementPage.xaml
	/// </summary>
	public partial class ViewArrangementPage : Page
	{

		private Arrangement arrangement;
		private MainWindow parent;
		private ReservationService reservationService;

		public ViewArrangementPage(MainWindow parent, Arrangement arrangement)
		{
			InitializeComponent();
			this.parent = parent;
			this.arrangement = arrangement;
			DataContext = this.arrangement;
			reservationService = ReservationService.Instance;
		}

		private void OnBackClick(object sender, RoutedEventArgs e) => parent.MainFrame.Content = new ArrangementsPage(parent);

		private void MakeReservationButton_Click(object sender, RoutedEventArgs e)
		{
			reservationService.CreateReservation(parent.User, arrangement);
		}

		private void CancelReservationButton_Click(object sender, RoutedEventArgs e)
		{

		}

		private void PayTripButton_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
