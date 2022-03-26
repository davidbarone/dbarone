namespace dbarone_api.Lib.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dbarone_api.Extensions;

public static class ValidationManager
{
    /// <summary>
    /// Tests the validity of an object. If not valid, throws an exception.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="resultType"></param>
    public static void AssertValidity(object target, ValidationResultType resultType)
    {
        var results = Validate(target, resultType);
        if (results.Any())
            throw new ValidationException(results);
    }

    /// <summary>
    /// Tests the validity of an object. If not valid, throws an exception.
    /// </summary>
    /// <param name="target"></param>
    public static void AssertValidity(object target)
    {
        var results = Validate(target);
        if (results.Any())
            throw new ValidationException(results);
    }

    public static IEnumerable<ValidationResult> Validate(object target, ValidationResultType resultType)
    {
        return Validate(target).Where(r => (resultType & r.ResultType) == r.ResultType);
    }

    public static IEnumerable<ValidationResult> Validate(object target)
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
