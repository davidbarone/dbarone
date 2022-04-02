namespace dbarone_api.Controllers;
using Microsoft.AspNetCore.Mvc;
using dbarone_api.Entities;
using dbarone_api.Services;
using dbarone_api.Authorization;
using dbarone_api.Models.Response;
using dbarone_api.Extensions;

/// <summary>
/// Services for posts.
/// </summary>
[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class GroupController : RestController
{
    private readonly IDataService _dataService;

    /// <summary>
    /// Constructor for post controller.
    /// </summary>
    /// <param name="dataService"></param>
    public GroupController(IDataService dataService)
    {
        this._dataService = dataService;
    }

    /// <summary>
    /// Gets a list of groups.
    /// </summary>
    /// <param name="pageSize"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    [HttpGet("/groups")]
    public ActionResult<ResponseEnvelope<LinkedResource<IEnumerable<LinkedResource<Group>>>>> GetGroups(int pageSize = 10, int page = 1)
    {
        if (pageSize > 1000) pageSize = 1000;
        if (pageSize < 1) pageSize = 1;
        if (page < 1) page = 1;
        var groups = _dataService.Context.Read<Group>().Select(g => g.ToLinkedResource(new Link[] {
            Url.GetLink("Update", this.UpdateGroup, new { id = g.Id }),
            Url.GetLink("Delete", this.DeleteGroup, new { id = g.Id })
        }));
        var linkedPage = groups.ToLinkedPaginatedResource(Url, this.GetGroups, pageSize, page);
        return Ok(linkedPage.ToResponseEnvelope());
    }

    /// <summary>
    /// Gets a group by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("/groups/{id}")]
    public ActionResult<ResponseEnvelope<LinkedResource<Group>>> GetGroup(int id)
    {
        var group = _dataService.Context.Single<Group>(new object[] { id });
        var linkedGroup = group.ToLinkedResource(new Link[] {
            Url.GetLink("Update", this.UpdateGroup, new { id = id }),
            Url.GetLink("Delete", this.DeleteGroup, new { id = id })
        });
        return Ok(linkedGroup.ToResponseEnvelope());
    }

    /// <summary>
    /// Creates a new group.
    /// </summary>
    /// <param name="group">The group to create.</param>
    /// <returns></returns>
    [HttpPost("/groups")]
    public ActionResult<ResponseEnvelope<LinkedResource<Group>>> CreateGroup(Group group)
    {
        group.Validate();
        var keys = _dataService.Context.Insert<Group>(group);
        var newItem = _dataService.Context.Single<Group>(keys);

        // Links
        List<Link> links = new();
        links.Add(Url.GetLink("self", this.GetGroup, new { id = newItem.Id }));
        var linkedGroup = newItem.ToLinkedResource(links);
        return Ok(linkedGroup.ToResponseEnvelope());
    }

    /// <summary>
    /// Updates an existing group.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="group"></param>
    /// <returns></returns>
    [HttpPut("/groups/{id}")]
    public ActionResult<ResponseEnvelope<LinkedResource<Group>>> UpdateGroup(int id, [FromBody] Group group)
    {
        group.Validate();
        if (id != group.Id)
        {
            throw new InvalidDataException($"Id {id} does not match resource id {group.Id}.");
        }
        var keys = _dataService.Context.Update<Group>(group);
        var newGroup = _dataService.Context.Single<Group>(keys);

        var linkedPost = newGroup.ToLinkedResource(new Link[]
        {
            Url.GetLink("self", this.GetGroup, new { id = newGroup.Id })
        });
        return Ok(linkedPost.ToResponseEnvelope());
    }

    /// <summary>
    /// Deletes an existing group.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("/groups/{id}")]
    public ActionResult<ResponseEnvelope<LinkedResource<object?>>> DeleteGroup(int id)
    {
        var keys = new object[] { id };
        _dataService.Context.Delete<Group>(keys);
        var obj = ((object?)null).ToLinkedResource<object>(new Link[] {
            Url.GetLink("Parent", this.GetGroups, null)
        });
        return Ok(obj.ToResponseEnvelope());
    }
}