namespace dbarone_api.Lib.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dbarone_api.Extensions;

/// <summary>
/// Field is required.
/// </summary>
public class RequiredValidatorAttribute : ValidatorAttribute
{
    public override void DoValidate(object value, object target, string key, IList<ValidationResult> results)
    {
        if (value == null || (value != null && value.GetType().Default() == value))
        {
            results.Add(new ValidationResult { Key = key, Target = target, Message = string.Format("{0} is required.", key) });
        }
    }
}
