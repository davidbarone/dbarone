using dbarone_api.Models.Response;
using System.Text.Json.Serialization;

/// <summary>
/// Represents a link in a HATEOAS compliant REST interface.
/// </summary>
public class Link
{
    public Link(string rel, string uri, HttpVerb method = HttpVerb.GET, string? title = null)
    {
        this.Rel = rel;
        this.Uri = uri;
        this.Method = method;
        this.Title = title;
    }

    /// <summary>
    /// The relation of the link to the current resource, for example 'parent', 'self'.
    /// </summary>
    public string Rel { get; set; }

    /// <summary>
    /// The Uri for the resource.
    /// </summary>
    public string Uri { get; set; }

    /// <summary>
    /// The Http verb to access the related resource.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public HttpVerb Method { get; set; }

    /// <summary>
    /// An optional title for the related resource.
    /// </summary>
    public string? Title { get; set; }
}