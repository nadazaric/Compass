using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Rule = travel_agent.ValidationRules.ValidationRule;

namespace travel_agent.Controls
{
    /// <summary>
    /// Interaction logic for FancyTextBox.xaml
    /// </summary>
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
            DependencyProperty.Register("Placeholder", typeof(string), typeof(FancyTextBox), new PropertyMetadata(null));

        public string Placeholder
        {
            get => GetValue(PlaceholderProperty) as string; 
            set => SetValue(PlaceholderProperty, value);
        }

        // InputText Property
        public static DependencyProperty InputTextProperty =
            DependencyProperty.Register("InputText", typeof(string), typeof(FancyTextBox), new PropertyMetadata(null));

        public string InputText
        {
            get => GetValue(InputTextProperty) as string;
            set => SetValue(InputTextProperty, value);
        }

        // Validation rules property
        public static readonly DependencyProperty ValidationRulesProperty =
            DependencyProperty.Register("ValidationRules", typeof(List<Rule>), typeof(FancyTextBox), new FrameworkPropertyMetadata(null));

        public List<Rule> ValidationRules
        {
            get => (List<Rule>)GetValue(ValidationRulesProperty);
            set => SetValue(ValidationRulesProperty, value);
        }

        #endregion

        public bool IsValid()
        {
            bool isValid = true;
            foreach (Rule rule in ValidationRules)
            {
                if(!rule.Validate(textBox.Text))
                {
                    isValid = false;
                    errorMessage.Content = rule.ErrorMessage;
                    errorMessage.Visibility = Visibility.Visible;
                    break;
                }
            }
            if (isValid)
            {
                errorMessage.Visibility = Visibility.Collapsed;
                errorMessage.Content = string.Empty;
            }
            return isValid;
        }
    }
}
