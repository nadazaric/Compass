using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using travel_agent.Controls;
using travel_agent.Models;
using travel_agent.Services;

namespace travel_agent.WindowsAndPages
{
    public partial class PlacesPage : Page
    {
        private new readonly MainWindow Parent;
        private PlaceService PlaceService { get; set; }
        private List<Place> FullPlacesList { get; set; }
        public ObservableCollection<Place> Places { get; set; }
        public PlacesPage(MainWindow parent)
        {
            InitializeComponent();
            Parent = parent;
            PlaceService = PlaceService.Instance;
            SetPlacesList();
            DataContext = this;
            if (Places.Count <= 1) SetupIfListEmpty();
            if (Parent.User.Role != Role.AGENT) PlacesItemsControl.Loaded += CollapseFirstItem;
        }

        private void CollapseFirstItem(object sender, RoutedEventArgs e)
        {
            var firstItemContainer = PlacesItemsControl.ItemContainerGenerator.ContainerFromIndex(0) as UIElement;
            if (firstItemContainer != null) firstItemContainer.Visibility = Visibility.Collapsed;
            PlacesItemsControl.Loaded -= CollapseFirstItem;
        }

        private void SetPlacesList()
        {
            Places = new ObservableCollection<Place> {null};
            FullPlacesList = PlaceService.GetAll();
            foreach (Place p in FullPlacesList) Places.Add(p);
        }

        private void SetupIfListEmpty()
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
                EmptyPlaceList.Visibility = Visibility.Visible;
            }
        }

        private void OnAddNewPlaceClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Parent.MainFrame.Content = new AddAndModifyPlacePage(Parent);
        }

        private void OnPlaceClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            object data = (sender as Grid).DataContext;
            if (Parent.User.Role == Role.AGENT) Parent.MainFrame.Content = new AddAndModifyPlacePage(Parent, data as Place);
            else Parent.MainFrame.Content = new ViewPlacePage(data as Place);
        }

        bool IsPopupOpen = false;
        private void OnHandlePopupClick(object sender, RoutedEventArgs e)
        {
            if (IsPopupOpen)
            {
                HandlePopupButton.Content = "▼";
                IsPopupOpen = false;
                AdvancedSearchPopup.IsOpen = false;
            }
            else
            {
                HandlePopupButton.Content = "▲";
                IsPopupOpen = true;
                AdvancedSearchPopup.IsOpen = true;
            }
        }

        private void Search()
        {
            Places = new ObservableCollection<Place> { null };
            foreach (var place in FullPlacesList)
            {
                if (place.Name.ToUpper().Contains(PlaceSearchName.InputText.ToUpper()) &&
                    IsTypeCorrect(place) &&
                    place.Address.ToUpper().Contains(PlaceSearchAddress.InputText.ToUpper())) Places.Add(place);
            }
            
            PlacesItemsControl.ItemsSource = Places;
        }

        private void OnSearchButtonClick(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void OnReturnToDefaultClick(object sender, RoutedEventArgs e)
        {
            SetPlacesList();
            PlacesItemsControl.ItemsSource = Places;
            PlaceSearchName.RestartState();
            PlaceSearchAddress.RestartState();
            PlaceAllRadioBtn.IsChecked = true;
        }

        private bool IsTypeCorrect(Place place)
        {
            if (GetType() == null) return true;
            return GetType() == place.Type;
        }

        private Place.PlaceType? GetType()
        {
            if (PlaceAllRadioBtn.IsChecked == true) return null;
            else if (PlaceAtractionRadioBtn.IsChecked == true) return Place.PlaceType.ATRACTION;
            else if (PlaceRestaurantRadioBtn.IsChecked == true) return Place.PlaceType.RESTAURANT;
            else return Place.PlaceType.ACCOMMODATION;
        }

        private async void AdvancedSearchPopup_Closed(object sender, EventArgs e)
        {
            HandlePopupButton.Content = "▼";
            await Task.Delay(100);
            IsPopupOpen = false;
        }

        private void OnEnterFancInputClick(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter) Search();
        }
    }

    public class FirstItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var itemsControl = ItemsControl.ItemsControlFromItemContainer(container);
            var index = itemsControl.ItemContainerGenerator.IndexFromContainer(container);
            if (index == 0) return (DataTemplate)itemsControl.FindResource("FirstItemTemplate");
            else return (DataTemplate)itemsControl.FindResource("DefaultItemTemplate");
        }
    }
}
