using System.Windows.Controls;

namespace travel_agent.WindowsAndPages
{
    public partial class PlacesPage : Page
    {
        private new readonly MainWindow Parent;
        public PlacesPage(MainWindow parent)
        {
            InitializeComponent();
            Parent = parent;
        }

        private void OnAddNewPlaceClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Parent.MainFrame.Content = new AddPlacePage(Parent);
        }
    }
}
