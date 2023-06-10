using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travel_agent.ValidationRules
{
	public class PriceTextBoxRule : ValidationRule
	{
		public override bool Validate(string value)
		{
			if (value == ".00") return false;
			return true;
		}
	}
}
