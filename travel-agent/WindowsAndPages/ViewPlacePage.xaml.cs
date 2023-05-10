using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static travel_agent.Models.Place;

namespace travel_agent.WindowsAndPages
{
    public partial class ViewPlacePage : Page
    {
        private new readonly MainWindow Parent;
        private Place Place;
        public ViewPlacePage(MainWindow parent, Place place)
        {
            InitializeComponent();
            Parent = parent;
            Place = place;
            DataContext = Place;
            Loaded += SetType;
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
