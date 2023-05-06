using System.Windows;
using travel_agent.Windows;

namespace travel_agent.WindowsAndPages
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Main.Content = new PlacesPage();
            PlacesNavbarButton.Focus();     
        }
            
        private void OnPlacesNavbarButtonClick(object sender, RoutedEventArgs e) => Main.Content = new PlacesPage(); 
        private void OnArrangementsNavbarBttonClick(object sender, RoutedEventArgs e) => Main.Content = new ArrangementsPage();
        private void OnLogoutButtonClick(object sender, RoutedEventArgs e)
        {
            var loginAndRegisterWindow = new LoginAndRegisterWindow();
            loginAndRegisterWindow.Show();
            Hide();
        }
    }
}