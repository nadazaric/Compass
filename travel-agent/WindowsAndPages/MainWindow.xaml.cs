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
        public MainWindow(User user)
        {
            User = user;
            InitializeComponent();
            App = Application.Current;
            Main.Content = new PlacesPage(this);
            SetFocusStyle(PlacesNavbarButton);
            MainFrame = Main;

        }

        private void SetFocusStyle(Button button) => button.Style = App.Resources["SelectedNavbarButtonStyle"] as Style;

        private void SetUnfocusStyle()
        {
            foreach (var child in NavbarButtons.Children)
                if (child is Button) (child as Button).Style = App.Resources["NavbarButtonStyle"] as Style;
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