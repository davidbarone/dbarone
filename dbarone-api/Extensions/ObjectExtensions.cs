namespace dbarone_api.Extensions;
using dbarone_api.Lib.Validation;
using System.Collections;

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

    /// <summary>
    /// Converts an object to a Hashtable.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Hashtable? ToHashTable(this object? obj)
    {
        if (obj == null)
        {
            return null;
        }
        else
        {
            Hashtable ht = new Hashtable();
            var props = obj.GetType().GetProperties();
            foreach (var prop in props)
            {
                ht.Add(prop.Name, prop.GetValue(obj));
            }
            return ht;
        }
    }
}