using Microsoft.AspNetCore.Mvc;
using dbarone_api.Entities;
using dbarone_api.Services;

namespace dbarone_api.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly IDataService _dataService;

    public PostController(IDataService dataService)
    {
        this._dataService = dataService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Post>> GetPosts()
    {
        var companies = _dataService.GetPosts();
        return Ok(companies);
    }
}
