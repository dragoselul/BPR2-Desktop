using System.Windows.Controls;

namespace BPR2_Desktop.ViewModels.Validators;

public class ValidateDoubleNumbers: ValidationRule
{
    public int Min { get; set; }
    public int Max { get; set; }
    public override ValidationResult Validate(object? value, CultureInfo cultureInfo)
    {
        {
            double number = 0;

            try
            {
                var numberOfDots = ((string)value).Count(c => c == '.');
                if (numberOfDots > 1)
                {
                    return new ValidationResult(false, "Illegal characters or double dots");
                }
                if (((string)value).EndsWith("."))
                {
                    number = double.Parse(((String)value) + ".0");
                    return ValidationResult.ValidResult;
                }
                if (((string)value).Length > 0)
                    number = double.Parse((String)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"Illegal characters or {e.Message}");
            }

            if ((number < Min) || (number > Max))
            {
                return new ValidationResult(false,
                    $"Please enter an number in the range: {Min}-{Max}.");
            }
            return ValidationResult.ValidResult;
        }
    }
}