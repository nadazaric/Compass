using System.Windows;
using System.Windows.Input;

namespace travel_agent.Windows
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void SingIn(object sender, RoutedEventArgs e)
        {
            LoginEmailInput.IsValid();
            LoginPassworInput.IsValid();
        }

        private void OnSingUp(object sender, MouseButtonEventArgs e)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.Show();
            Hide();
        }
    }
}
