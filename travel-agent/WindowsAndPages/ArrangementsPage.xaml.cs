using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
                if (arrangement.Name.ToLower().Contains(ArrangementSearchName.InputText.ToLower())) Arrangements.Add(arrangement);
			}
            if (Arrangements.Count == 0 && parent.User.Role != Role.AGENT) SetIfNoContentAfterSearch();
            if (Arrangements.Count > 0 && WasListCollapsed) SetIfHaveContentAfterSearch();
            ArrangementsItemsCotrol.ItemsSource = Arrangements;
		}

        private void OnSearchButtonClick(object sender, EventArgs e) => Search();

        private void OnEnterSearch(object sender, EventArgs e) => Search();

        private void OnTransportTypeCB(object sender, EventArgs e) => Search();

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

        private void SetIfNoContent()
		{
            if(parent.User.Role == Role.AGENT)
			{
                ArrangementsListView.Margin = new Thickness(20);
                ArrangementsSearch.Visibility = Visibility.Visible;
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
