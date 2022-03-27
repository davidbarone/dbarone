namespace dbarone_api.Controllers;
using dbarone_api.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Linq;

public class LinksDictionaryKey
{
    public string Route { get; set; }
    public string HttpVerb { get; set; }
}


public class RestController : ControllerBase
{
    public IDictionary<LinksDictionaryKey, Link> Links { get; init; }

    protected LinkedResource<IEnumerable<T>> GetPaginationResult<T>(string controller, string action, IEnumerable<T> data, int pageSize, int page)
    {
        double rowCount = data.Count();
        int lastPage = (int)Math.Ceiling(rowCount / pageSize);
        if (page < 1) page = 1;
        if (page > lastPage) page = lastPage;

        var first = Url.Action(action, controller, new { pageSize = pageSize, page = 1 });
        var last = Url.Action(action, controller, new { pageSize = pageSize, page = lastPage });
        var previous = (page > 1) ? Url.Action(action, controller, new { pageSize = pageSize, page = page - 1 }) : null;
        var next = (page < lastPage) ? Url.Action(action, controller, new { pageSize = pageSize, page = page + 1 }) : null;

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