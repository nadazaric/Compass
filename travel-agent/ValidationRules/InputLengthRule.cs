namespace travel_agent.ValidationRules
{
    public class InputLengthRule : ValidationRule
    {
        public int MinLength { get; set; } = int.MinValue;
        public int MaxLength { get; set; } = int.MaxValue;

        public override bool Validate(string value)
        {
            if (value.Length < MinLength || value.Length > MaxLength) return false;
            return true;
        }
    }
}
