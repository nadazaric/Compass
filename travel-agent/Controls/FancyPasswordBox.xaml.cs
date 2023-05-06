using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Rule = travel_agent.ValidationRules.ValidationRule;

namespace travel_agent.Controls
{
    public partial class FancyPasswordBox : UserControl
    {
        public FancyPasswordBox()
        {
            InitializeComponent();
            DataContext = this;
            ValidationRules = new List<Rule>();
        }

        #region ---[ Properties ]---

        // Placeholder property
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("PlaceholderPasswordBox", typeof(string), typeof(FancyPasswordBox), new PropertyMetadata(string.Empty));

        public string Placeholder
        {
            get => GetValue(PlaceholderProperty) as string;
            set => SetValue(PlaceholderProperty, value);
        }

        // Password property
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("PasswordPasswordBox", typeof(string), typeof(FancyPasswordBox), new PropertyMetadata(string.Empty));

        public string Password
        {
            get => (GetValue(PasswordProperty) as string).Trim();
            private set => SetValue(PasswordProperty, value);
        }

        // Validation rules property
        public static readonly DependencyProperty ValidationRulesProperty =
            DependencyProperty.Register("ValidationRulesPasswordBox", typeof(List<Rule>), typeof(FancyPasswordBox), new FrameworkPropertyMetadata(null));

        public List<Rule> ValidationRules
        {
            get => (List<Rule>)GetValue(ValidationRulesProperty);
            set => SetValue(ValidationRulesProperty, value);
        }
        #endregion

        #region ---[ Methods ]---
        public void SetManuallyError(string message)
        {
            errorMessage.Content = message;
            errorMessage.Visibility = Visibility.Visible;
        }

        public void UnsetManuallyError()
        {
            errorMessage.Visibility = Visibility.Collapsed;
            errorMessage.Content = string.Empty;
        }

        public bool IsValid()
        {
            bool isValid = true;
            foreach (Rule rule in ValidationRules)
            {
                passwordBox.Password = passwordBox.Password.Trim();
                if (!rule.Validate(passwordBox.Password))
                {
                    isValid = false;
                    errorMessage.Content = rule.ErrorMessage;
                    errorMessage.Visibility = Visibility.Visible;
                    break;
                }
            }
            if (isValid) UnsetManuallyError();
            return isValid;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (!string.IsNullOrEmpty(passwordBox.Password)) Password = passwordBox.Password;
        }

        public void RestartState()
        {
            passwordBox.Password = string.Empty;
            UnsetManuallyError();
        }

        #endregion
    }
}
