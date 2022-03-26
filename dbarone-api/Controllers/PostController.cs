using Microsoft.AspNetCore.Mvc;
using dbarone_api.Entities;
using dbarone_api.Services;
using dbarone_api.Authorization;
using dbarone_api.Lib.ObjectMapper;
using dbarone_api.Models;
using dbarone_api.Models.Response;

namespace dbarone_api.Controllers;

/// <summary>
/// Services for posts.
/// </summary>
[ApiController]
[Route("[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class PostController : RestController
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
    [HttpGet("/posts")]
    public ActionResult<ResponseEnvelope<IEnumerable<PostSummaryResponse>>> GetPosts(int pageSize = 10, int page = 1, bool includeChildren = false)
    {
        var url = Url.Action("GetPosts", "Post");
        if (pageSize > 1000) pageSize = 1000;
        if (pageSize < 1) pageSize = 1;
        if (page < 1) page = 1;
        var mapper = ObjectMapper<Post, PostSummaryResponse>.Create();
        var posts = mapper.MapMany(_dataService.GetPosts().Where(p => !p.IsChild || includeChildren));
        var paginationResult = GetPaginationResult("Post", "GetPosts", posts, pageSize, page);
        return Ok(this.CreateResponseEnvelope<IEnumerable<PostSummaryResponse>>(paginationResult.Page, null, null, paginationResult.Links));
    }

    [HttpPost("/posts")]
    [Authorize]
    public ActionResult<Post> CreatePost(PostRequest post)
    {

        return null;
    }

    [HttpPut("/posts/{id}")]
    [Authorize]
    public ActionResult UpdatePost(int id, [FromBody] Post post)
    {
        return null;
    }

    [HttpDelete("/posts/{id}")]
    [Authorize]
    public ActionResult DeletePost(int id)
    {
        return null;
    }

    [HttpGet("/posts/{id}")]
    public ActionResult<Post> GetPost(int id)
    {
        return Ok(_dataService.GetPost(id));
    }

    [HttpGet("/{slug}")]
    public ActionResult<Post> GetPostBySlug(string slug)
    {
        return Ok(_dataService.GetPostBySlug(slug));
    }

    [HttpGet("/posts/related/{id}")]
    public ActionResult<RelatedPostResponse> GetRelatedPosts(int id)
    {
        var mapper = ObjectMapper<Post, PostSummaryResponse>.Create();

        return Ok(new RelatedPostResponse
        {
            Parent = mapper.MapOne(_dataService.GetPostParent(id)),
            Siblings = mapper.MapMany(_dataService.GetPostSiblings(id)),
            Children = mapper.MapMany(_dataService.GetPostChildren(id))
        });
    }
}
