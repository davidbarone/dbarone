namespace dbarone_api.Extensions;

public static class GuidExtensions
{
    /// <summary>
    /// Returns time part of Guid which should be relatively
    /// unique locally. Used to enable users to specify a
    /// shorter string to select a unique record.
    /// </summary>
    /// <param name="g"></param>
    /// <returns></returns>
    public static string TimeLow(this Guid g)
    {
        return g.ToString("N").Substring(0, 8);
    }
}