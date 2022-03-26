namespace dbarone_api.Extensions;
using System.ComponentModel;
using System.Reflection;

/// <summary>
/// Type extensions.
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Returns a default value for value types and reference types.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static object? Default(this Type t)
    {
        if (t.IsValueType)
        {
            return Activator.CreateInstance(t);
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Supports a parse method for nullable types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expression"></param>
    /// <param name="parseMethod"></param>
    /// <returns></returns>
    public static Nullable<T> NullableParse<T>(string expression, ParseDelegate<T> parseMethod) where T : struct
    {
        if (string.IsNullOrEmpty(expression))
            return null;
        else
        {
            try
            {
                MethodInfo? mi = typeof(T)?.GetMethod("Parse", new[] { typeof(string) });
                if (mi != null)
                {
                    var del = (ParseDelegate<T>)Delegate.CreateDelegate(typeof(ParseDelegate<T>), mi);
                    return del(expression);
                }
                else
                    throw new ApplicationException("Type does not support the Parse method.");
            }
            catch { return null; }
        }
    }

    /// <summary>
    /// Accesses a 'parse' function for nullable types
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public delegate T ParseDelegate<T>(string expression);


    /// <summary>
    /// Determines whether an object is a certain type.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="targetType"></param>
    /// <returns></returns>
    public static bool CanConvertTo(this object input, Type targetType)
    {
        try
        {
            TypeDescriptor.GetConverter(targetType).ConvertFrom(input);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Returns true if an object is a numeric type.
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public static bool IsNumeric(this object o)
    {
        switch (Type.GetTypeCode(o.GetType()))
        {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
                return true;
            default:
                return false;
        }
    }
}