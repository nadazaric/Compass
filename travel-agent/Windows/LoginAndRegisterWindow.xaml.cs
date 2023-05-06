using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using travel_agent.Controls;
using travel_agent.Models;
using travel_agent.Services;

namespace travel_agent.Windows
{ 
    public partial class LoginWindow : Window
    {
        UserService userService;
        Application app;
        public LoginWindow()
        {
            InitializeComponent();
            userService = UserService.Instance;
            app = Application.Current;
        }

        private void OnSingInClick(object sender, RoutedEventArgs e)
        {
            if (!IsLoginInputsValid()) return;
            bool isLogged = userService.TryLogin(LoginEmailInput.InputText, LoginPasswordInput.Password);
            if(!isLogged)
            {
                LoginEmailInput.RestartState();
                LoginPasswordInput.RestartState();
                LoginEmailInput.SetManuallyError(app.Resources["String.EmailOrPassworNotCorrectError"] as string);
                LoginPasswordInput.SetManuallyError(app.Resources["String.EmailOrPassworNotCorrectError"] as string);
            }
            // TODO: Go to next window
        }

        private void OnSwichToRegisterViewClick(object sender, MouseButtonEventArgs e)
        {
            LoginView.Visibility = Visibility.Collapsed;
            RegisterView.Visibility = Visibility.Visible;
            RestartLoginState();
        }

        private void OnRegisterClick(object sender, RoutedEventArgs e)
        {
            if (!IsRegisterInputsValid()) return;
            User newUser = new User
            {
                Name = RegisterNameInput.InputText,
                LastName = RegisterLastnameInput.InputText,
                Email = RegisterEmailInput.InputText,
                Password = RegisterPasswordInput.Password,
                Role = Role.PASSENGER
            };
            userService.Create(newUser);
            MessageBox.Show(app.Resources["String.RegisterSuccessMessage"] as string , app.Resources["String.AppName"] as string, MessageBoxButton.OK, MessageBoxImage.Information);
            RegisterView.Visibility = Visibility.Collapsed;
            LoginView.Visibility = Visibility.Visible;
        }

        private void OnSwichToLoginViewClick(object sender, MouseButtonEventArgs e)
        {
            RegisterView.Visibility = Visibility.Collapsed;
            LoginView.Visibility = Visibility.Visible;
            RestartRegisterState();
        }

        private void RestartLoginState()
        {
            LoginEmailInput.RestartState();
            LoginPasswordInput.RestartState();
        }

        private void RestartRegisterState()
        {
            StackPanel registerView = FindName("RegisterView") as StackPanel;
            foreach (var child in registerView.Children)
            {
                if (child is FancyTextBox) (child as FancyTextBox).RestartState();
                else if (child is FancyPasswordBox) (child as FancyPasswordBox).RestartState();
            }
        }

        private bool IsLoginInputsValid()
        {
            bool isAllValid = true;
            if (!LoginEmailInput.IsValid()) isAllValid = false;
            if (!LoginPasswordInput.IsValid()) isAllValid = false;
            return isAllValid;
        }

        private bool IsRegisterInputsValid()
        {
            bool isAllValid = true;
            if(!RegisterNameInput.IsValid()) isAllValid = false;
            if(!RegisterLastnameInput.IsValid()) isAllValid = false;
            if(!RegisterEmailInput.IsValid() || IsEmailAlreadyUsed()) isAllValid = false;
            if(!RegisterPasswordInput.IsValid()) isAllValid = false;
            if(!RegisterConfirmePasswordInput.IsValid() || !IsConfirmePasswordCorrect()) isAllValid = false;
            return isAllValid;
        }

        private bool IsConfirmePasswordCorrect()
        {
            if (RegisterPasswordInput.Password == RegisterConfirmePasswordInput.Password)
            {
                RegisterConfirmePasswordInput.UnsetManuallyError();
                return true;
            }
            RegisterConfirmePasswordInput.SetManuallyError(app.Resources["String.ConfirmePasswordNotMatchError"] as string);
            return false;
        }

        private bool IsEmailAlreadyUsed()
        {
            if (userService.IsEmailAlreadyUsed(RegisterEmailInput.InputText))
            {
                RegisterEmailInput.SetManuallyError(app.Resources["String.AlreadyUseEmailError"] as string);
                return true;
            }
            RegisterEmailInput.UnsetManuallyError();
            return false;
        }
    }
}
