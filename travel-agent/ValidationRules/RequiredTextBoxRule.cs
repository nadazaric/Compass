namespace travel_agent.ValidationRules
{
    public class RequiredTextBoxRule : ValidationRule
    {
        public override bool Validate(string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return true;
        }
    }
}
