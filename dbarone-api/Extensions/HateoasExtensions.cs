namespace dbarone_api.Extensions;
using dbarone_api.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

/// <summary>
/// Extensions that simplify Hateoas support.
/// </summary>
public static class HateoasExtensions
{
    /// <summary>
    /// Converts a resource to a linked resource
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resource"></param>
    /// <param name="links"></param>
    /// <returns></returns>
    public static LinkedResource<T?> ToLinkedResource<T>(this T? resource, IEnumerable<Link> links)
    {
        return LinkedResource<T>.Create(resource, links);
    }

    public static ResponseEnvelope<T?> ToResponseEnvelope<T>(this T? resource)
    {
        return ResponseEnvelope<T>.Create(resource);
    }

    /// <summary>
    /// Creates a response envelope from a list of items. Creates a page with pagination links.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static ResponseEnvelope<IEnumerable<T>?>? ToResponseEnvelope<T>(this IEnumerable<T> list, IUrlHelper urlHelper, Delegate getMember, int pageSize, int page) {
        var linkedPaginatedResource = list.ToLinkedPaginatedResource<T>(urlHelper, getMember, pageSize, page);
        var envelope = linkedPaginatedResource.Resource.ToResponseEnvelope();
        envelope.AddLinks(linkedPaginatedResource.Links);
        return envelope;
    }

    /// <summary>
    /// Returns a Link object for a Controller action and parameters.
    /// </summary>
    /// <param name="action"></param>
    /// <param name=""></param>
    /// <param name=""></param>
    /// <returns></returns>
    public static Link GetLink(this IUrlHelper helper, string rel, Delegate method, object? values = null, string? title = null)
    {
        Dictionary<Type, HttpVerb> HttpVerbs = new Dictionary<Type, HttpVerb>();
        HttpVerbs.Add(typeof(HttpGetAttribute), HttpVerb.GET);
        HttpVerbs.Add(typeof(HttpPutAttribute), HttpVerb.PUT);
        HttpVerbs.Add(typeof(HttpPatchAttribute), HttpVerb.PATCH);
        HttpVerbs.Add(typeof(HttpDeleteAttribute), HttpVerb.DELETE);
        HttpVerbs.Add(typeof(HttpPostAttribute), HttpVerb.POST);
        HttpVerbs.Add(typeof(HttpHeadAttribute), HttpVerb.HEAD);
        HttpVerbs.Add(typeof(HttpOptionsAttribute), HttpVerb.OPTIONS);

        HttpVerb verb = HttpVerb.GET;
        foreach (var key in HttpVerbs.Keys)
        {
            var attr = method.Method.GetCustomAttribute(key);
            if (attr != null)
            {
                verb = HttpVerbs[key];
            }
        }

        var controller = method.Method?.DeclaringType?.Name.Replace("controller", "", true, System.Globalization.CultureInfo.CurrentCulture);
        var action = method.Method?.Name;
        var uri = helper.Action(action, controller, values);
        return new Link(rel, uri, verb, title);
    }

    public static LinkedResource<IEnumerable<T>> ToLinkedPaginatedResource<T>(this IEnumerable<T> data, IUrlHelper urlHelper, Delegate getMember, int pageSize, int page)
    {
        var controller = getMember.Method?.DeclaringType?.Name.Replace("controller", "", true, System.Globalization.CultureInfo.CurrentCulture);
        var action = getMember.Method?.Name;

        double rowCount = data.Count();
        int lastPage = (int)Math.Ceiling(rowCount / pageSize);
        if (page < 1) page = 1;
        if (page > lastPage) page = lastPage;

        var first = urlHelper.Action(action, controller, new { pageSize = pageSize, page = 1 });
        var last = urlHelper.Action(action, controller, new { pageSize = pageSize, page = lastPage });
        var previous = (page > 1) ? urlHelper.Action(action, controller, new { pageSize = pageSize, page = page - 1 }) : null;
        var next = (page < lastPage) ? urlHelper.Action(action, controller, new { pageSize = pageSize, page = page + 1 }) : null;

        var pagedData = data.Skip((page - 1) * pageSize).Take(pageSize);
        var links = (lastPage > 1) ? new List<Link> {
                new Link("first", first),
                new Link("previous", previous),
                new Link("next", next),
                new Link("last", last)
            }.Where(l => !string.IsNullOrEmpty(l.Uri)).ToList() : null;

        LinkedResource<IEnumerable<T>> results = new LinkedResource<IEnumerable<T>>(pagedData, links);
        return results;
    }
}