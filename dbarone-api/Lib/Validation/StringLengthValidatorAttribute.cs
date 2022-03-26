namespace dbarone_api.Lib.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// String length validator.
/// </summary>
public class StringLengthValidatorAttribute : ValidatorAttribute
{
    /// <summary>
    /// Minimum length of the string.
    /// </summary>
    public int Min { get; set; }

    /// <summary>
    /// Maximum length of the string.
    /// </summary>
    public int Max { get; set; }

    private string ErrorMessage
    {
        get
        {
            if (Min == 0)
                return "Maximum length of {0} is {1}";
            else
                return "{0} must be between {2} and {1} characters";
        }
    }

    /// <summary>
    /// DoValidate method.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="target"></param>
    /// <param name="key"></param>
    /// <param name="results"></param>
    public override void DoValidate(object value, object target, string key, IList<ValidationResult> results)
    {
        // note that string length ONLY runs if value not null.
        // to test for nulls, use NotNull validator as well.
        if (value != null)
        {
            if (value.GetType() != typeof(string))
                results.Add(new ValidationResult { Key = key, Target = target, Message = "StringLengthValidator must operate on a string type" });

            if (Max < Min)
                results.Add(new ValidationResult { Key = key, Target = target, Message = "MaxLength is less than MinLength" });

            if (Max < 0)
                results.Add(new ValidationResult { Key = key, Target = target, Message = "MaxLength cannot be negative" });

            if (Min < 0)
                results.Add(new ValidationResult { Key = key, Target = target, Message = "MinLength cannot be negative" });

            if (value?.ToString()?.Length < Min || value?.ToString()?.Length > Max)
                results.Add(new ValidationResult { Key = key, Target = target, Message = string.Format(ErrorMessage, key, Max, Min) });
        }
    }
}
