using System.Text.RegularExpressions;

namespace travel_agent.ValidationRules
{
    internal class EmailPatternRule : ValidationRule
    {
        public override bool Validate(string value)
        {
            string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            if (!Regex.IsMatch(value, emailPattern)) return false;
            return true;
        }
    }
}
