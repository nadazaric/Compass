using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Rule = travel_agent.ValidationRules.ValidationRule;

namespace travel_agent.Controls
{
    public partial class FancyTextBox : UserControl
    {
        public FancyTextBox()
        {
            InitializeComponent();
            DataContext = this;
            ValidationRules = new List<Rule>();
        }

        #region ---[ Properties ]---

        // Placeholder property
        public static readonly DependencyProperty PlaceholderProperty = 
            DependencyProperty.Register("PlaceholderTextBox", typeof(string), typeof(FancyTextBox), new PropertyMetadata(null));

        public string Placeholder
        {
            get => GetValue(PlaceholderProperty) as string; 
            set => SetValue(PlaceholderProperty, value);
        }

        // InputText Property
        public static DependencyProperty InputTextProperty =
            DependencyProperty.Register("InputText", typeof(string), typeof(FancyTextBox), new PropertyMetadata(string.Empty));

        public string InputText
        {
            get => (GetValue(InputTextProperty) as string).Trim();
            set => SetValue(InputTextProperty, value);
        }


        // Validation rules property
        public static readonly DependencyProperty ValidationRulesProperty =
            DependencyProperty.Register("ValidationRulesTextBox", typeof(List<Rule>), typeof(FancyTextBox), new FrameworkPropertyMetadata(null));

        public List<Rule> ValidationRules
        {
            get => (List<Rule>)GetValue(ValidationRulesProperty);
            set => SetValue(ValidationRulesProperty, value);
        }

        // Multiline property
        public static readonly DependencyProperty MultilineProperty =
        DependencyProperty.Register("MultilineTextBox", typeof(bool), typeof(FancyTextBox), new PropertyMetadata(false));

        public bool Multiline
        {
            get => (bool)GetValue(MultilineProperty);
            set => SetValue(MultilineProperty, value); 
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
                textBox.Text = textBox.Text.Trim();
                if(!rule.Validate(textBox.Text))
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

        public void RestartState()
        {
            textBox.Text = string.Empty;
            UnsetManuallyError();
        }

        #endregion
    }
}
