namespace dbarone_api.Controllers;
using Microsoft.AspNetCore.Mvc;
using dbarone_api.Entities;
using dbarone_api.Services;
using dbarone_api.Models.Response;
using dbarone_api.Extensions;
using dbarone_api.Authorization;

/// <summary>
/// Controller for handling resource requests.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ResourceController : RestController
{
    private readonly IDataService _dataService;

    /// <summary>
    /// Constructor for resource controller.
    /// </summary>
    /// <param name="dataService"></param>
    public ResourceController(IDataService dataService)
    {
        this._dataService = dataService;
    }

    /// <summary>
    /// Returns a list of all resources.
    /// </summary>
    /// <returns>List of resources.</returns>
    [HttpGet("/resources")]
    public ActionResult<ResponseEnvelope<LinkedResource<IEnumerable<LinkedResource<Resource>>>>> GetResources(int pageSize = 10, int page = 1)
    {
        if (pageSize > 1000) pageSize = 1000;
        if (pageSize < 1) pageSize = 1;
        if (page < 1) page = 1;
        var resources = _dataService.Context.Read<Resource>();
        var linkedResources = resources.Select(g => g.ToLinkedResource(new Link[] {
            Url.GetLink("View", this.GetResource, new { id = g.Id })
        }));
        var linkedPage = linkedResources.ToLinkedPaginatedResource(Url, this.GetResources, pageSize, page);
        return Ok(linkedPage.ToResponseEnvelope());
    }

    /// <summary>
    /// Downloads a resource by id.
    /// </summary>
    /// <param name="id">The resource id.</param>
    /// <returns>Downloaded resource file.</returns>
    [HttpGet("/resources/{id}")]
    public FileContentResult GetResource(int id)
    {
        var resource = _dataService.Context.Find<Resource>(id);
        return File(
            resource.Data, resource.ContentType, resource.Filename
        );
    }

    /// <summary>
    /// Downloads a resouce by filename.
    /// </summary>
    /// <param name="filename">The resource filename</param>
    /// <returns>Downloaded resource file.</returns>
    [HttpGet("/static/{filename}")]
    public FileContentResult GetResourceByName(string filename)
    {
        var resource = _dataService.Context.Single<Resource>(new { Filename = filename });
        return File(
            resource.Data, resource.ContentType, resource.Filename
        );
    }

    /// <summary>
    /// Uploads a resource.
    /// </summary>
    /// <param name="file">File to upload.</param>
    /// <returns></returns>
    [HttpPost("/resources/")]
    [Authorize]
    public ActionResult<ResponseEnvelope<LinkedResource<Post>>> UploadResource(IFormFile file)
    {
        var resource = _dataService.Context.Create<Resource>();
        resource.ContentType = file.ContentType;
        resource.Filename = file.FileName;
        var fileSize = (int)file.Length;
        resource.Data = new byte[fileSize];
        file.OpenReadStream().Read(resource.Data, 0, fileSize);
        var keys = _dataService.Context.Insert<Resource>(resource);
        resource = _dataService.Context.Find<Resource>(keys);
        return Ok(resource.ToLinkedResource(null).ToResponseEnvelope());
    }

    /// <summary>
    /// Deletes a resource.
    /// </summary>
    /// <param name="id">The id of the resource to delete.</param>
    /// <returns></returns>
    [HttpDelete("/resources/{id}")]
    [Authorize]
    public ActionResult<ResponseEnvelope<LinkedResource<object?>>> DeleteResource(int id)
    {
        _dataService.Context.Delete<Resource>(id);
        var obj = ((object?)null).ToLinkedResource<object>(new Link[] {
            Url.GetLink("Parent", this.GetResources, null)
        });
        return Ok(obj.ToResponseEnvelope().AddMessage(new ResponseMessage() { Message = $"Resource {id} successfully deleted." }));
    }
}
