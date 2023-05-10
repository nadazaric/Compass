using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            else Parent.MainFrame.Content = new ViewPlacePage(Parent, data as Place);
        }

        private void OnShowPopupClick(object sender, RoutedEventArgs e)
        {
            ShowPopupButton.Visibility = Visibility.Collapsed;
            HidePopupButton.Visibility = Visibility.Visible;
            AdvancedSearchPopup.IsOpen = true;
        }

        private void OnHidePopupClick(object sender, RoutedEventArgs e)
        {
            ShowPopupButton.Visibility = Visibility.Visible;
            HidePopupButton.Visibility = Visibility.Collapsed;
            AdvancedSearchPopup.IsOpen = false;
        }

        private void Search()
        {
            string name = PlaceSearchName.InputText;
            string address = PlaceSearchAddress.InputText;

            Places = new ObservableCollection<Place> { null };
            foreach (var place in FullPlacesList)
            {
                Console.WriteLine(PlaceSearchAddress.InputText);
                if ((name != "" && place.Name.ToUpper().Contains(name.ToUpper())) ||
                    (address != "" && place.Address.ToUpper().Contains(address.ToUpper()))) Places.Add(place);
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
