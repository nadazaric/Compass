using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

using Rule = travel_agent.ValidationRules.ValidationRule;

namespace travel_agent.Controls
{
	/// <summary>
	/// Interaction logic for FancyDatePicker.xaml
	/// </summary>
	public partial class FancyDatePicker : UserControl
	{
		public FancyDatePicker()
		{
			InitializeComponent();
            DataContext = this;
		}

		public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register("PlaceholderDatePicker", typeof(string), typeof(FancyDatePicker), new PropertyMetadata(null));

		public string Placeholder
		{
			get => GetValue(PlaceholderProperty) as string;
			set => SetValue(PlaceholderProperty, value);
		}

        public static DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(FancyDatePicker), new PropertyMetadata(null));

        public DateTime? SelectedDate
        {
            get => (DateTime?)GetValue(SelectedDateProperty);
            set => SetValue(SelectedDateProperty, value);
        }

	}
}
