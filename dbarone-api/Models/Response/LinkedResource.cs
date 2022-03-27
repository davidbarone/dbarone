namespace dbarone_api.Models.Response;
using System.Text.Json.Serialization;

/// <summary>
/// Represents a resource with links. Links allow clients to discover functionality. This is at core of HATEOAS.
/// </summary>
/// <typeparam name="T"></typeparam>
public class LinkedResource<T>
{
    public static LinkedResource<T> Create(T resource, IEnumerable<Link> links)
    {
        return new LinkedResource<T>(resource, links);
    }

    public LinkedResource(T resource, IEnumerable<Link> links)
    {
        this.Resource = resource;
        this.Links = links;
    }

    public T Resource { get; set; }

    public IEnumerable<Link> Links { get; set; }
}