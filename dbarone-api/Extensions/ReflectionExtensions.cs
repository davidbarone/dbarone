namespace dbarone_api.Extensions;
using System.Reflection;
using System.Linq.Expressions;

/// <summary>
/// Reflection extention methods
/// </summary>
public static class ReflectionExtensions
{
    /// <summary>
    /// Returns a list of properties decorated by the specified attribute.
    /// </summary>
    /// <typeparam name="T">The specified attribute.</typeparam>
    /// <param name="obj"></param>
    /// <param name="inherit"></param>
    /// <returns></returns>
    public static PropertyInfo[] GetPropertiesDecoratedBy<T>(this object obj, bool inherit = false) where T : Attribute
    {
        return obj.GetType()
            .GetProperties()
            .Where(pi => Attribute.IsDefined(pi, typeof(T), inherit))
            .ToArray();
    }

    /// <summary>
    /// Returns a list of properties decorated by a specific attribute.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="inherit"></param>
    /// <returns></returns>
    public static PropertyInfo[] GetPropertiesDecoratedBy<T>(this Type t, bool inherit = false)
    {
        return t
            .GetProperties()
            .Where(pi => Attribute.IsDefined(pi, typeof(T), inherit))
            .ToArray();
    }

    /// <summary>
    /// Returns a list of members decorated by a specific attribute.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static MemberInfo[] GetMembersDecoratedBy<T>(this object obj) where T : Attribute
    {
        return obj.GetType()
            .GetMembers()
            .Where(pi => Attribute.IsDefined(pi, typeof(T), false))
            .ToArray();
    }

    /// <summary>
    /// Returns a list of methods decorated by the specified attribute.
    /// </summary>
    /// <typeparam name="T">The specified attribute.</typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static MethodInfo[] GetMethodsDecoratedBy<T>(this object obj) where T : Attribute
    {
        return obj.GetType()
            .GetMethods()
            .Where(pi => Attribute.IsDefined(pi, typeof(T), false))
            .ToArray();
    }

    /// <summary>
    /// Gets the value of a property using reflection.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static object? Value(this object obj, string propertyName)
    {
        Type t = obj.GetType();
        return t.GetProperty(propertyName)?.GetValue(obj, null);
    }

    /// <summary>
    /// Returns a dictionary of concrete types that implement a base type within
    /// an app domain. Typically used as AppDomain.CurrentDomain.GetTypesImplementing().
    /// </summary>
    /// <param name="domain"></param>
    /// <param name="baseType"></param>
    /// <returns></returns>
    public static IList<Type> GetTypesImplementing(this AppDomain domain, Type baseType)
    {
        Dictionary<string, Type> types = new Dictionary<string, Type>();
        // Get all the command types:
        foreach (var assembly in domain.GetAssemblies())
        {
            try
            {
                foreach (var type in assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract))
                {
                    if (!types.ContainsKey(type.Name))
                        types.Add(type.Name.ToLower(), type);
                }
            }
            catch (ReflectionTypeLoadException)
            {
            }
        }
        return types.Values.ToList();
    }

    /// <summary>
    /// Get subclasses of a given type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="domain"></param>
    /// <returns></returns>
    public static IEnumerable<Type> GetSubclassTypesOf<T>(this AppDomain domain)
    {
        List<Type> types = new List<Type>();

        // Get all the command types:
        foreach (var assembly in domain.GetAssemblies())
        {
            try
            {
                foreach (var type in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(T))))
                    types.Add(type);
            }
            catch (ReflectionTypeLoadException)
            {
            }
        }
        return types;
    }
}