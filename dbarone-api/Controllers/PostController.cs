using Microsoft.AspNetCore.Mvc;
using dbarone_api.Entities;
using dbarone_api.Services;

namespace dbarone_api.Controllers;

/// <summary>
/// Services for posts.
/// </summary>
[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly IDataService _dataService;

    /// <summary>
    /// Constructor for post controller.
    /// </summary>
    /// <param name="dataService"></param>
    public PostController(IDataService dataService)
    {
        this._dataService = dataService;
    }

    /// <summary>
    /// Returns a set of posts.
    /// </summary>
    /// <param name="includeChildren">If set to true, child posts are included in the result.</param>
    /// <returns>A set of posts.</returns>
    [HttpGet("posts")]
    public ActionResult<IEnumerable<Post>> GetPosts(bool includeChildren = false)
    {
        var companies = _dataService.GetPosts().Where(p => !p.IsChild || includeChildren);
        return Ok(companies);
    }

    [HttpPost("posts")]
    public ActionResult<Post> CreatePost(Post post)
    {
        return null;
    }

    [HttpPut("posts/{id}")]
    public ActionResult UpdatePost(int id, [FromBody] Post post)
    {
        return null;
    }

    [HttpDelete("posts/{id}")]
    public ActionResult DeletePost(int id)
    {
        return null;
    }

    [HttpGet("posts/{id}")]
    public ActionResult<Post> GetPost(int id)
    {
        return Ok(_dataService.GetPost(id));
    }

    [HttpGet("posts/related/{id}")]
    public ActionResult<RelatedPostResponse> GetRelatedPosts(int id)
    {
        return Ok(new RelatedPostResponse
        {
            Parent = _dataService.GetPostParent(id),
            Siblings = _dataService.GetPostSiblings(id),
            Children = _dataService.GetPostChildren(id)
        });
    }
}
