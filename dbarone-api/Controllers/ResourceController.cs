using Microsoft.AspNetCore.Mvc;
using dbarone_api.Entities;
using dbarone_api.Services;

namespace dbarone_api.Controllers;

[ApiController]
[Route("[controller]")]
public class ResourceController : ControllerBase
{
    private readonly IDataService _dataService;

    public ResourceController(IDataService dataService)
    {
        this._dataService = dataService;
    }

    /// <summary>
    /// Deletes a resource.
    /// </summary>
    /// <param name="id">The id of the resource to delete.</param>
    /// <returns></returns>
    [HttpDelete("/resources/{id}")]
    public ActionResult DeleteResource(int id)
    {
        _dataService.DeleteResource(id);
        return Ok(new { message = $"Resource {id} successfully deleted." });
    }

    /// <summary>
    /// Uploads a resource.
    /// </summary>
    /// <param name="file">File to upload.</param>
    /// <returns></returns>
    [HttpPost("/resources/")]
    public ActionResult<Resource> UploadResource(IFormFile file)
    {
        var contentType = file.ContentType;
        var filename = file.FileName;
        var fileSize = (int)file.Length;
        byte[] data = new byte[fileSize];
        file.OpenReadStream().Read(data, 0, fileSize);
        var resource = _dataService.CreateResource(filename, contentType, data);
        return Ok(resource);
    }

    /// <summary>
    /// Returns a list of all resources.
    /// </summary>
    /// <returns>List of resources.</returns>
    [HttpGet("/resources")]
    public ActionResult<IEnumerable<Resource>> GetResources()
    {
        return Ok(_dataService.GetResources());
    }

    /// <summary>
    /// Downloads a resource by id.
    /// </summary>
    /// <param name="id">The resource id.</param>
    /// <returns>Downloaded resource file.</returns>
    [HttpGet("/resources/{id}")]
    public ActionResult<Resource> GetResource(int id)
    {
        var resource = _dataService.GetResource(id);
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
    public ActionResult<Resource> GetResourceByName(string filename)
    {
        var resource = _dataService.GetResourceByFilename(filename);
        return File(
            resource.Data, resource.ContentType, resource.Filename
        );
    }

}
