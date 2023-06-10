using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace travel_agent.Controls
{
	public class CurrencyValueConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.Length >= 2 && values[0] is decimal inputPrice && values[1] is string text)
			{
				var formattedValue = $"{inputPrice:0.00} RSD";
				return formattedValue;
			}

			return string.Empty;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			if (value is string stringValue && decimal.TryParse(stringValue, NumberStyles.Currency, culture, out decimal decimalValue))
			{
				return new object[] { decimalValue, stringValue };
			}

			return new object[] { };
		}
	}
}
