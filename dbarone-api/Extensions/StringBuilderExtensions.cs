namespace dbarone_api.Extensions;
using System.Text;

/// <summary>
/// StringBuilder extensions.
/// </summary>
public static class StringBuilderExtensions
{
    /// <summary>
    /// Removes characters from the left side of the string.
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static StringBuilder RemoveRight(this StringBuilder sb, int length)
    {
        return sb.Remove(sb.Length - length, length);
    }

    /// <summary>
    /// Removes characters from the right side of the string.
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static StringBuilder RemoveLeft(this StringBuilder sb, int length)
    {
        return sb.Remove(0, length);
    }

}