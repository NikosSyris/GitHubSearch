using System.Windows.Controls;

namespace GitHubSearch.Action.Validation
{
    class StringValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int maxLength = 400;

            if (value.ToString().Length > maxLength)
            {
                return new ValidationResult(false, "Name cannot be more than " + maxLength +" characters long.");
            }
            
            return ValidationResult.ValidResult;
        }
    }
}