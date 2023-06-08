using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using travel_agent.ValidationRules;
using ValidationRule = travel_agent.ValidationRules.ValidationRule;

namespace travel_agent.Controls
{
	/// <summary>
	/// Interaction logic for FancyCurrencyTextBox.xaml
	/// </summary>
	public partial class FancyCurrencyTextBox : UserControl
	{
		public FancyCurrencyTextBox()
		{
			InitializeComponent();
			DataContext = this;
			ValidationRules = new List<ValidationRule>();
		}

		// InputPrice Property
		public static DependencyProperty InputPriceProperty =
			DependencyProperty.Register("InputPrice", typeof(decimal), typeof(FancyCurrencyTextBox), new PropertyMetadata(null));

		public decimal InputPrice
		{
			get => (decimal)GetValue(InputPriceProperty);
			set => SetValue(InputPriceProperty, value);
		}


		// Validation rules property
		public static readonly DependencyProperty ValidationRulesProperty =
			DependencyProperty.Register("ValidationRulesCurrencyTextBox", typeof(List<ValidationRule>), typeof(FancyCurrencyTextBox), new FrameworkPropertyMetadata(null));

		public List<ValidationRule> ValidationRules
		{
			get => (List<ValidationRule>)GetValue(ValidationRulesProperty);
			set => SetValue(ValidationRulesProperty, value);
		}

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
			foreach (ValidationRule rule in ValidationRules)
			{
				textBox.Text = textBox.Text.Trim();
				if (!rule.Validate(textBox.Text))
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

		private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9\\s]+");
			e.Handled = regex.IsMatch(e.Text);
		}	
		
	}
}
