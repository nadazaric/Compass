using System.Windows.Controls;
using travel_agent.Services;

namespace travel_agent.WindowsAndPages
{
    public partial class PlacesPage : Page
    {
        private new readonly MainWindow Parent;
        private PlaceService PlaceService { get; set; }
        public PlacesPage(MainWindow parent)
        {
            InitializeComponent();
            Parent = parent;
            PlaceService = PlaceService.Instance;
            //var place = PlaceService.GetPlace(1);
            //var imageBrush = new ImageBrush(place.Image);
            //test.Background = imageBrush;
        }

        private void OnAddNewPlaceClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Parent.MainFrame.Content = new AddPlacePage(Parent);
        }
    }
}
