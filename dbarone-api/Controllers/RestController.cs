namespace dbarone_api.Controllers;
using dbarone_api.Models.Response;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Linq;

public class RestController : ControllerBase
{
    /// <summary>
    /// Creates a response standard envelope that implements HATEOAS.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="e"></param>
    /// <param name="messages"></param>
    /// <param name="links"></param>
    /// <returns></returns>
    protected ResponseEnvelope<T> CreateResponseEnvelope<T>(T data, Exception? e = null, IEnumerable<ResponseMessage>? messages = null, IList<ResponseLink>? links = null)
    {
        return new ResponseEnvelope<T>
        {
            Status = new ResponseStatus
            {
                Success = (e == null) ? true : false,
                Message = e?.Message ?? ""
            },
            Data = data,
            Messages = messages!,
            Links = links!
        };
    }

    /// <summary>
    /// Pagination result structure
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginationResult<T>
    {
        /// <summary>
        /// The selected page of data
        /// </summary>
        public IEnumerable<T> Page { get; set; }
        /// <summary>
        /// REST links
        /// </summary>
        public IList<ResponseLink>? Links { get; set; }
    }

    protected PaginationResult<T> GetPaginationResult<T>(string controller, string action, IEnumerable<T> data, int pageSize, int page)
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

        return new PaginationResult<T>
        {
            Page = pagedData,
            Links = (lastPage > 1) ? new List<ResponseLink> {
                new ResponseLink{ Rel = "first", Uri = first},
                new ResponseLink{ Rel = "previous", Uri = previous},
                new ResponseLink{ Rel = "next", Uri = next},
                new ResponseLink{ Rel = "last", Uri = last}
            }.Where(l => !string.IsNullOrEmpty(l.Uri)).ToList() : null
        };
    }
}