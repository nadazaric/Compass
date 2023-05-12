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
        public ViewPlacePage(Place place)
        {
            InitializeComponent();
            Place = place;
            DataContext = Place;
            Loaded += SetType;
            Map.DisableDoubleClick();
            Map.DrawPin(new Location(Place.Latitude, Place.Longitude));
        }

        private void SetType(object sender, RoutedEventArgs e)
        {
            if (Place.Type == PlaceType.ATRACTION) PlaceTypeAtractionLabel.Visibility = Visibility.Visible;
            else if (Place.Type == PlaceType.RESTAURANT) PlaceTypeRestaurantLabel.Visibility = Visibility.Visible;
            else PlaceTypeAccommodationLabel.Visibility = Visibility.Visible;
            Loaded -= SetType;
        }
    }
}
