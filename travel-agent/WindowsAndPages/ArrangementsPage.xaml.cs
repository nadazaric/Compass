using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
 
			if (Arrangements.Count <= (parent.User.Role == Role.AGENT ? 1 : 0)) SetIfNoContent();
            StartDatePicker.DateChanged += StartDatePicker_DateChanged;
            EndDatePicker.DateChanged += EndDatePicker_DateChanged;
              
        }

        private void SetUpArrangements()
		{
            Arrangements = new ObservableCollection<Arrangement>();
            if (parent.User.Role == Role.AGENT)
            {
                ArrangementsList = arrangementService.GetAll();
            }
            else ArrangementsList = arrangementService.GetFuture().Where(a => !a.IsDeleted).ToList();

            foreach (Arrangement item in ArrangementsList) Arrangements.Add(item);
            Arrangements = new ObservableCollection<Arrangement>(Arrangements.Reverse());
			if (parent.User.Role == Role.AGENT) Arrangements.Insert(0,null);
		}

		private void Search()
		{
			Arrangements = new ObservableCollection<Arrangement>();
			if (parent.User.Role == Role.AGENT) Arrangements.Add(null);
            foreach (var arrangement in ArrangementsList)
			{
                if (arrangement.Name.ToLower().Contains(ArrangementSearchName.InputText.ToLower())
                    && CheckTransportType(arrangement) && CheckFilter(arrangement) && CheckDateRange(arrangement) && CheckBudget(arrangement))
                    Arrangements.Add(arrangement);
			}
            if (Arrangements.Count == 0 && parent.User.Role != Role.AGENT) SetIfNoContentAfterSearch();
            if (Arrangements.Count > 0 && WasListCollapsed) SetIfHaveContentAfterSearch();
            ArrangementsItemsCotrol.ItemsSource = Arrangements;
		}

        private bool CheckBudget(Arrangement arrangement)
        {
            bool price0 = (bool)PriceCB0.IsChecked ? true : false;
			bool price5 = (bool)PriceCB5.IsChecked ? true : false;
			bool price10 = (bool)PriceCB10.IsChecked ? true : false;
			bool price15 = (bool)PriceCB15.IsChecked ? true : false;
			bool price20 = (bool)PriceCB20.IsChecked ? true : false;

			if (!price0 && !price10 && !price5 && !price15 && !price20) return true;
			if (price0 && arrangement.Price <= 5000) return true;
            if (price5 && arrangement.Price > 5000 && arrangement.Price <= 10000) return true;
            if (price10 && arrangement.Price > 10000 && arrangement.Price <= 15000) return true;
            if(price15 && arrangement.Price > 15000 && arrangement.Price <= 20000) return true;
            if(price20 && arrangement.Price > 20000) return true;
            return false;

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

        // TODO Check logic for filter
        private bool CheckTransportType(Arrangement arrangement)
		{
            bool bus = (bool)TransportBusCB.IsChecked ? false : true;
            bool plane = (bool)TransportPlaneCB.IsChecked ? false : true;
            bool train = (bool)TransportTrainCB.IsChecked ? false : true;
            bool foot = (bool)TransportSelfCB.IsChecked ? false : true;

            if(bus && plane && train && foot) return true;
            foreach(var step in arrangement.Steps)
            {
                switch (step.TransportationType)
                {
                    case ArrangementStep.TransportType.PLANE: plane= true; break;
                    case ArrangementStep.TransportType.TRAIN: train= true; break;
                    case ArrangementStep.TransportType.BUS: bus= true; break;
                    case ArrangementStep.TransportType.FOOT: foot = true; break;   
                }
            }

            return bus && plane && train && foot;
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

            StartDatePicker.SelectedDate = null;
            EndDatePicker.SelectedDate = null;

            ArrangementSearchPlace.RestartState();

            FilterRestaurantCB.IsChecked = false;
            FilterAccomodationCB.IsChecked= false;

            PriceCB0.IsChecked = false;
            PriceCB5.IsChecked = false;
            PriceCB10.IsChecked = false;
            PriceCB15.IsChecked = false;
            PriceCB20.IsChecked = false;

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
            else parent.MainFrame.Content = new ViewArrangementPage(parent, data as Arrangement);
		}

		private void StartDatePicker_DateChanged(object sender, SelectionChangedEventArgs e)
		{
			DateTime selectedDate = (DateTime)((FancyDatePicker)sender).SelectedDate;
			if(EndDatePicker.SelectedDate == null || DateTime.Compare(selectedDate, (DateTime)EndDatePicker.SelectedDate) > 0)
            {
                selectedDate = selectedDate.AddDays(1);
                EndDatePicker.SelectedDate = selectedDate;
            }
		}

        private void EndDatePicker_DateChanged(object sender, SelectionChangedEventArgs args)
        {
			DateTime selectedDate = (DateTime)((FancyDatePicker)sender).SelectedDate;
			if (StartDatePicker.SelectedDate == null || DateTime.Compare(selectedDate, (DateTime)StartDatePicker.SelectedDate) < 0)
			{
				selectedDate = selectedDate.AddDays(-1);
				StartDatePicker.SelectedDate = selectedDate;
			}
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
