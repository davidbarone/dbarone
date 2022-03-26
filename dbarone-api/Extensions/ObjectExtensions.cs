namespace dbarone_api.Extensions;
using dbarone_api.Lib.Validation;

/// <summary>
/// Object extensions.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Validates an object. Throws an exception if validation fails.
    /// </summary>
    /// <param name="obj"></param>
    public static void Validate(this object? obj)
    {
        if (obj != null)
            ValidationManager.Validate(obj);    // may throw ValidationException

        return;
    }
}