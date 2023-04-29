using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using travel_agent.Controls;

namespace travel_agent.Windows
{ 
    public partial class LoginWindow : Window
    {
        public LoginWindow() => InitializeComponent();

        private void OnSingInClick(object sender, RoutedEventArgs e)
        {
            LoginEmailInput.IsValid();
            LoginPasswordInput.IsValid();
            Console.WriteLine(LoginPasswordInput.Password);
        }

        private void OnSwichToRegisterViewClick(object sender, MouseButtonEventArgs e)
        {
            LoginView.Visibility = Visibility.Collapsed;
            RegisterView.Visibility = Visibility.Visible;
            RestartLoginState();
        }

        private void OnRegisterClick(object sender, RoutedEventArgs e) => CheckIfRegisterInputsValid();

        private void OnSwichToLoginViewClick(object sender, MouseButtonEventArgs e)
        {
            RegisterView.Visibility = Visibility.Collapsed;
            LoginView.Visibility = Visibility.Visible;
            RestartRegisterState();
        }

        private void RestartLoginState()
        {
            LoginEmailInput.RestartTextBoxState();
            LoginPasswordInput.RestartPasswordBoxState();
        }

        private void RestartRegisterState()
        {
            StackPanel registerView = FindName("RegisterView") as StackPanel;
            foreach (var child in registerView.Children)
            {
                if (child is FancyTextBox) (child as FancyTextBox).RestartTextBoxState();
                else if (child is FancyPasswordBox) (child as FancyPasswordBox).RestartPasswordBoxState();
            }
        }

        private void CheckIfRegisterInputsValid()
        {
            StackPanel registerView = FindName("RegisterView") as StackPanel;
            foreach(var child in registerView.Children)
            {
                if (child is FancyTextBox) (child as FancyTextBox).IsValid();
                else if (child is FancyPasswordBox) (child as FancyPasswordBox).IsValid();
            }
        }
    }
}
