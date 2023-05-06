namespace travel_agent.ValidationRules
{
    public abstract class ValidationRule
    {
        public string ErrorMessage { get; set; }
        public abstract bool Validate(string value);
    }
}
