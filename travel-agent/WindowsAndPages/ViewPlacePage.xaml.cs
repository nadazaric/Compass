using System.Windows;
using System.Windows.Controls;
using travel_agent.Models;
using static travel_agent.Models.Place;
using Microsoft.Maps.MapControl.WPF;

namespace travel_agent.WindowsAndPages
{
    public partial class ViewPlacePage : Page
    {
        private Place Place;
        private MainWindow Parent;
        private Arrangement Arrangement;
        public ViewPlacePage(Place place, MainWindow parent, Arrangement arrangement = null)
        {
            InitializeComponent();
            Place = place;
            DataContext = Place;
            Loaded += SetType;
            Map.DisableDoubleClick();
            Map.DrawPin(new Location(Place.Latitude, Place.Longitude));
            Parent = parent;
            Arrangement = arrangement;
        }

        private void SetType(object sender, RoutedEventArgs e)
        {
            if (Place.Type == PlaceType.ATRACTION) PlaceTypeAtractionLabel.Visibility = Visibility.Visible;
            else if (Place.Type == PlaceType.RESTAURANT) PlaceTypeRestaurantLabel.Visibility = Visibility.Visible;
            else PlaceTypeAccommodationLabel.Visibility = Visibility.Visible;
            Loaded -= SetType;
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            if(Arrangement == null) Parent.MainFrame.Content = new PlacesPage(Parent);
            else Parent.MainFrame.Content = new ViewArrangementPage(Parent, Arrangement);
        }
    }
}
