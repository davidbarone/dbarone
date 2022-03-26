namespace dbarone_api.Lib.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dbarone_api.Extensions;

public static class ValidationManager
{
    /// <summary>
    /// Validates an object. Throws an exception if not valid.
    /// </summary>
    /// <param name="target"></param>
    public static void Validate(object target)
    {
        var results = GetValidationResults(target);
        if (results.Any())
            throw new ValidationException(results);
    }

    /// <summary>
    /// Gets the validations results for an object. Does not throw an exception.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static IEnumerable<ValidationResult> GetValidationResults(object target)
    {
        var props = target.GetPropertiesDecoratedBy<ValidatorAttribute>();
        List<ValidationResult> results = new List<ValidationResult>();

        foreach (var prop in props)
        {
            // get the attribute for the property
            var attributes = (ValidatorAttribute[])prop.GetCustomAttributes(typeof(ValidatorAttribute), false);
            foreach (var attribute in attributes)
            {
                string key = prop.Name;
                attribute.DoValidate(target.Value(key), target, key, results);
            }
        }

        // method validators
        var methods = target.GetMethodsDecoratedBy<MethodValidatorAttribute>();
        foreach (var method in methods)
        {
            // get the attribute for the property
            var attributes = (MethodValidatorAttribute[])method.GetCustomAttributes(typeof(MethodValidatorAttribute), false);
            foreach (var attribute in attributes)
            {
                string key = method.Name;
                attribute.Method = method;
                attribute.DoValidate(null, target, key, results);
            }
        }

        return results;
    }
}
