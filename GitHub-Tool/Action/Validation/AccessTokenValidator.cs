using System.Windows.Controls;

namespace GitHubSearch.Action.Validation
{
    class AccessTokenValidator : ValidationRule
    {

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {

            if ( value.ToString().Length == 0)
            {
                return new ValidationResult(false, "Access token cannot be null");
            }

            return ValidationResult.ValidResult;
        }
    }
}