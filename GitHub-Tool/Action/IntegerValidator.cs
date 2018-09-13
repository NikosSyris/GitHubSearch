using System;
using System.Windows.Controls;

namespace GitHub_Tool.Action
{
    class IntegerValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string stringValue = Convert.ToString(value);
            int maxLength = 999999999;
            int intValue = 0;
            bool canConvert = false;
            
            canConvert = int.TryParse(stringValue, out intValue);

            if (!canConvert)
            {
                return new ValidationResult(false, $"Input should be type of Int");
            }
            else
            {
                if (intValue < 0 || intValue > 999999999)
                {
                    return new ValidationResult(false, "Cannot be less than 0 and more than " + maxLength);
                }
                
            }

            return ValidationResult.ValidResult;
        }
    }
}