using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using travel_agent.Controls;
using travel_agent.Models;
using travel_agent.Services;

namespace travel_agent.WindowsAndPages
{
    public partial class ArrangementsPage : Page
    {
        private readonly MainWindow parent;
        private Application app;
        private ArrangementService arrangementService;
        public ObservableCollection<Arrangement> Arrangements { get; set; }
        private List<Arrangement> ArrangementsList { get; set; }

        private bool IsPopupOpen = false;
        private bool WasListCollapsed = true;



        public ArrangementsPage(MainWindow parentWindow)
        {
            InitializeComponent();
            app = Application.Current;
            parent = parentWindow;
            arrangementService = ArrangementService.Instance;
            DataContext = this;
            SetUpArrangements(); 
            if (Arrangements.Count <= 1) SetIfNoContent();
              
        }

        private void SetUpArrangements()
		{
            Arrangements = new ObservableCollection<Arrangement>();
            if (parent.User.Role == Role.AGENT) Arrangements.Add(null);
            ArrangementsList = arrangementService.GetAll();
            foreach (Arrangement item in ArrangementsList) Arrangements.Add(item);
		}

		private void Search()
		{
			Arrangements = new ObservableCollection<Arrangement>();
			if (parent.User.Role == Role.AGENT) Arrangements.Add(null);
            foreach (var arrangement in ArrangementsList)
			{
                if (arrangement.Name.ToLower().Contains(ArrangementSearchName.InputText.ToLower())
                    && CheckTransportType(arrangement) && CheckFilter(arrangement) && CheckDateRange(arrangement))
                    Arrangements.Add(arrangement);
			}
            if (Arrangements.Count == 0 && parent.User.Role != Role.AGENT) SetIfNoContentAfterSearch();
            if (Arrangements.Count > 0 && WasListCollapsed) SetIfHaveContentAfterSearch();
            ArrangementsItemsCotrol.ItemsSource = Arrangements;
		}


        private bool CheckDateRange(Arrangement arrangement)
        {
			DateTime? pickedStart = StartDatePicker.SelectedDate;
            DateTime? pickedEnd = EndDatePicker.SelectedDate;

			// TODO raise on screen error
			if (pickedStart == null && pickedEnd != null) return false;
			if (pickedStart != null && pickedEnd  == null) return false;
			if (pickedStart == null && pickedEnd == null) return true;

            DateTime startTime = arrangement.Start;
            DateTime endTime = arrangement.End;

            if (DateTime.Compare(startTime, (DateTime)pickedStart) < 0 || DateTime.Compare((DateTime)pickedEnd, endTime) < 0) return false;
            else return true;

        }


        private bool CheckFilter(Arrangement arrangement)
		{
			bool hasAccomodation = (bool)FilterAccomodationCB.IsChecked ? false : true;
            bool hasRestaurant = (bool)FilterRestaurantCB.IsChecked ? false : true;

            if (hasAccomodation && hasRestaurant) return true;

            foreach(var place in arrangement.Places)
            {
                if(place.Type == Place.PlaceType.RESTAURANT) hasRestaurant = true;
                else if(place.Type == Place.PlaceType.ACCOMMODATION) hasAccomodation = true;
            }

            return hasRestaurant && hasAccomodation;

		}

        private bool CheckTransportType(Arrangement arrangement)
		{
            bool bus = (bool)TransportBusCB.IsChecked ? false : true;
            bool plane = (bool)TransportPlaneCB.IsChecked ? false : true;
            bool train = (bool)TransportTrainCB.IsChecked ? false : true;
            bool self = (bool)TransportSelfCB.IsChecked ? false : true;

            if(bus && plane && train && self) return true;
            foreach(var step in arrangement.Steps)
            {
                switch (step.Type)
                {
                    case ArrangementStep.TransportType.PLANE: plane= true; break;
                    case ArrangementStep.TransportType.TRAIN: train= true; break;
                    case ArrangementStep.TransportType.BUS: bus= true; break;
                    case ArrangementStep.TransportType.SELF: self= true; break;   
                }
            }

            return bus && plane && train && self;
		}

        private void OnSearchButtonClick(object sender, EventArgs e) => Search();

		private void OnEnterSearch(object sender, EventArgs e) => Search();

        private void OnCBClick(object sender, EventArgs e) => Search();

        private void OnHandlePopupClick(object sender, RoutedEventArgs e)
		{
			if (IsPopupOpen)
			{
                HandlePopupButton.Content = app.Resources["String.DownButton"] as string;
                IsPopupOpen = false;
                ArrangementsSearchPopup.IsOpen = false;
			}
			else
			{
                HandlePopupButton.Content = app.Resources["String.UpButton"] as string;
                IsPopupOpen = true;
                ArrangementsSearchPopup.IsOpen = true;
            }
		}

        private void OnReturnToDefaultClick(object sender, RoutedEventArgs e)
        {
            SetUpArrangements();
            if (Arrangements.Count > 1 && WasListCollapsed) SetIfHaveContentAfterSearch();
            ArrangementsItemsCotrol.ItemsSource = Arrangements;
            ArrangementSearchName.RestartState();

            TransportPlaneCB.IsChecked = false;
            TransportBusCB.IsChecked = false;
            TransportSelfCB.IsChecked = false;
            TransportTrainCB.IsChecked = false;

            Console.WriteLine("tu sam");
            Console.WriteLine(StartDatePicker.SelectedDate);

            StartDatePicker.SelectedDate = null;
            EndDatePicker.SelectedDate = null;

            ArrangementSearchPlace.RestartState();

            FilterRestaurantCB.IsChecked = false;
            FilterAccomodationCB.IsChecked= false;
		}

        private async void WhenPopupIsClosed(object sender, EventArgs e)
		{
            HandlePopupButton.Content = app.Resources["String.DownButton"] as string;
            await Task.Delay(200);
            IsPopupOpen = false;
		}

		private void OnAddNewArangementClick(object sender, System.Windows.RoutedEventArgs e) => parent.MainFrame.Content = new AddAndModifyArangementPage(parent);

        private void OnArrangementItemClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
            object data = (sender as Grid).DataContext;
            if (parent.User.Role == Models.Role.AGENT) parent.MainFrame.Content = new AddAndModifyArangementPage(parent, data as Arrangement);
		}

        // TODO izaci na kraj sa custom eventovima
        private void OnSelectedDateChanged(object sender, EventArgs e)
        {
			DateTime? selectedDate = ((FancyDatePicker)sender).SelectedDate;
            Console.WriteLine(selectedDate);
			Console.WriteLine("trigger");
            Console.WriteLine(StartDatePicker.SelectedDate);
        }

        private void SetIfNoContent()
		{
            if(parent.User.Role == Role.AGENT)
			{
                ArrangementsListView.Margin = new Thickness(20);
                ArrangementsSearch.Visibility = Visibility.Collapsed;
			}
			else
			{
                ArrangementsSearch.Visibility=Visibility.Collapsed;
                ArrangementsListView.Visibility = Visibility.Collapsed;
                NoContent.Visibility = Visibility.Visible;

            }
		}

		private void SetIfNoContentAfterSearch()
		{
			NoContent.Visibility = Visibility.Visible;
			ArrangementsListView.Visibility = Visibility.Collapsed;
            WasListCollapsed = true;
		}

		private void SetIfHaveContentAfterSearch()
		{
			NoContent.Visibility = Visibility.Collapsed;
			ArrangementsListView.Visibility = Visibility.Visible;
            WasListCollapsed = false;
        }


		private void SearchArrangementOnEnter(object sender, EventArgs e) => Search();

	}
}
