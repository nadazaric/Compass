using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using travel_agent.Models;
using travel_agent.Services;

namespace travel_agent.WindowsAndPages
{
    public partial class PlacesPage : Page
    {
        private new readonly MainWindow Parent;
        private Application App;
        private PlaceService PlaceService;
        private List<Place> FullPlacesList;
        public ObservableCollection<Place> Places { get; set; }
        private bool IsPopupOpen = false;
        private bool WasListColapsed = true;
        public PlacesPage(MainWindow parent)
        {
            InitializeComponent();
            Parent = parent;
            App = Application.Current;
            PlaceService = PlaceService.Instance;
            SetPlacesList();
            DataContext = this;
            if (Places.Count <= 1) SetupIfInitiallyNoContent();
            PlaceAllRadioBtn.Checked += OnPlaceTypeRadioBtnClick;
        }

        #region ---[ Logic ]---
        private void SetPlacesList()
        {
            Places = new ObservableCollection<Place>();
            FullPlacesList = PlaceService.GetAll();
            foreach (Place p in FullPlacesList) Places.Add(p);
            Places = new ObservableCollection<Place>(Places.Reverse());
            if (Parent.User.Role == Role.AGENT) Places.Insert(0, null);
		}

        private void Search()
        {
            Places = new ObservableCollection<Place>();
            if (Parent.User.Role == Role.AGENT) Places.Add(null);
            foreach (var place in FullPlacesList)
            {
                if (place.Name.ToUpper().Contains(PlaceSearchName.InputText.ToUpper()) &&
                    IsTypeCorrect(place) == true &&
                    place.Address.ToUpper().Contains(PlaceSearchAddress.InputText.ToUpper())) Places.Add(place);
            }
            if (Places.Count == 0 && Parent.User.Role != Role.AGENT) SetIfNoContentAfterSearch();
            if (Places.Count > 0 && WasListColapsed) SetIfHaveContentAfterSearch();
            PlacesItemsControl.ItemsSource = Places;
        }

        private bool IsTypeCorrect(Place place)
        {
            if (GetTypeFromRadioButton() == null) return true;
            return GetTypeFromRadioButton() == place.Type;
        }
        #endregion

        #region ---[ Handlers ]---

        private void OnHandlePopupClick(object sender, RoutedEventArgs e)
        {
            if (IsPopupOpen)
            {
                HandlePopupButton.Content = App.Resources["String.DownButton"] as string;
                IsPopupOpen = false;
                AdvancedSearchPopup.IsOpen = false;
            }
            else
            {
                HandlePopupButton.Content = App.Resources["String.UpButton"] as string;
                IsPopupOpen = true;
                AdvancedSearchPopup.IsOpen = true;
            }
        }

        private void OnSearchButtonClick(object sender, RoutedEventArgs e) => Search();

        private async void WhenPopupClosed(object sender, EventArgs e)
        {
            HandlePopupButton.Content = App.Resources["String.DownButton"] as string;
            await Task.Delay(200);
            IsPopupOpen = false;
        }

        private void SearchInputOnEnterPressed(object sender, EventArgs e) => Search();

        private void OnPlaceTypeRadioBtnClick(object sender, RoutedEventArgs e) => Search();

        private void OnReturnToDefaultClick(object sender, RoutedEventArgs e)
        {
            SetPlacesList();
            if (Places.Count > 1 && WasListColapsed) SetIfHaveContentAfterSearch();
            PlacesItemsControl.ItemsSource = Places;
            PlaceSearchName.RestartState();
            PlaceSearchAddress.RestartState();
            PlaceAllRadioBtn.IsChecked = true;
        }

        private void OnAddNewPlaceClick(object sender, System.Windows.RoutedEventArgs e) => Parent.MainFrame.Content = new AddAndModifyPlacePage(Parent);

        private void OnPlaceItemClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            object data = (sender as Grid).DataContext;
            if (Parent.User.Role == Role.AGENT) Parent.MainFrame.Content = new AddAndModifyPlacePage(Parent, data as Place);
            else Parent.MainFrame.Content = new ViewPlacePage(data as Place, Parent);
        }
        #endregion

        #region ---[ Helpers ] ---
        private void SetupIfInitiallyNoContent()
        {
            if (Parent.User.Role == Role.AGENT)
            {
                PlacesList.Margin = new Thickness(20);
                AdvanceSearch.Visibility = Visibility.Collapsed;
            }
            else
            {
                AdvanceSearch.Visibility = Visibility.Collapsed;
                PlacesList.Visibility = Visibility.Collapsed;
                NoContent.Visibility = Visibility.Visible;
            }
        }

        private Place.PlaceType? GetTypeFromRadioButton()
        {
            if (PlaceAllRadioBtn.IsChecked == true) return null;
            else if (PlaceAtractionRadioBtn.IsChecked == true) return Place.PlaceType.ATRACTION;
            else if (PlaceRestaurantRadioBtn.IsChecked == true) return Place.PlaceType.RESTAURANT;
            else return Place.PlaceType.ACCOMMODATION;
        }

        private void SetIfNoContentAfterSearch()
        {
            NoContent.Visibility = Visibility.Visible;
            PlacesList.Visibility = Visibility.Collapsed;
            WasListColapsed = true;
        }

        private void SetIfHaveContentAfterSearch()
        {
            NoContent.Visibility = Visibility.Collapsed;
            PlacesList.Visibility = Visibility.Visible;
            WasListColapsed = false;
        }

        #endregion
    }

    public class FirstItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var itemsControl = ItemsControl.ItemsControlFromItemContainer(container);
            var place = itemsControl.ItemContainerGenerator.ItemFromContainer(container);
            if (place == null) return (DataTemplate)itemsControl.FindResource("FirstItemTemplate");
            else return (DataTemplate)itemsControl.FindResource("DefaultItemTemplate");
        }
    }
}
