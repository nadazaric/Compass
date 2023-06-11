using System.Windows;
using System.Windows.Controls;
using travel_agent.Models;
using travel_agent.Windows;

namespace travel_agent.WindowsAndPages
{
    public partial class MainWindow : Window
    {
        public User User { get; }
        public Frame MainFrame { get; }
        private Application App;

        public Visibility IsPassenger = Visibility.Collapsed;
        public MainWindow(User user)
        {
            User = user;
            InitializeComponent();
            App = Application.Current;
            Main.Content = new PlacesPage(this);
            SetFocusStyle(PlacesNavbarButton);
            MainFrame = Main;
            if (user.Role == Role.PASSENGER) 
            {
                MyTripsButton.Visibility = Visibility.Visible;
            } 
        }

        public void SetFocusStyle(Button button) => button.Style = App.Resources["SelectedNavbarButtonStyle"] as Style;

        public void SetUnfocusStyle()
        {
            foreach (var child in NavbarButtons.Children)
                if (child is Button) (child as Button).Style = App.Resources["NavbarButtonStyle"] as Style;
        }

        private void OnMyTripsNavbarButtonClick(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new MyTripsPage(this));
            SetUnfocusStyle();
            SetFocusStyle(sender as Button);
        }

        private void OnPlacesNavbarButtonClick(object sender, RoutedEventArgs e)
        {
            Main.Navigate(new PlacesPage(this));
            SetUnfocusStyle();
            SetFocusStyle(sender as Button);
        }
        private void OnArrangementsNavbarBttonClick(object sender, RoutedEventArgs e)
        {
            Main.Content = new ArrangementsPage(this);
            SetUnfocusStyle();
            SetFocusStyle(sender as Button);
        }

        private void OnLogoutButtonClick(object sender, RoutedEventArgs e)
        {
            var loginAndRegisterWindow = new LoginAndRegisterWindow();
            loginAndRegisterWindow.Show();
            Close();    
        }
    }
}