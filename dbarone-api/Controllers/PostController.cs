using Microsoft.AspNetCore.Mvc;
using dbarone_api.Models;
using dbarone_api.Repository;

namespace dbarone_api.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly IRepository _repository;

    public PostController(IRepository repository)
    {
        this._repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostInfo>>> GetPosts()
    {
        var companies = await _repository.GetPosts();
        return Ok(companies);
    }
}
