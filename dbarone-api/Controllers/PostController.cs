namespace dbarone_api.Controllers;
using Microsoft.AspNetCore.Mvc;
using dbarone_api.Entities;
using dbarone_api.Services;
using dbarone_api.Authorization;
using dbarone_api.Lib.ObjectMapper;
using dbarone_api.Models.Post;
using dbarone_api.Models.Response;
using dbarone_api.Extensions;

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
        var linkedPage = posts.ToLinkedPaginatedResource(Url, this.GetPosts, pageSize, page);
        return Ok(linkedPage.ToResponseEnvelope());
    }

    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="post">The post to create.</param>
    /// <returns></returns>
    [HttpPost("/posts")]
    public ActionResult<ResponseEnvelope<LinkedResource<Post>>> CreatePost(PostRequest post)
    {
        post.Validate();
        var createdPost = _dataService.CreatePost(post);

        // Links
        List<Link> links = new();
        links.Add(Url.GetLink("self", this.GetPost, new { id = createdPost.Id }));
        var linkedPost = createdPost.ToLinkedResource(links);
        return Ok(linkedPost.ToResponseEnvelope());
    }

    /// <summary>
    /// Updates an existing post.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="post"></param>
    /// <returns></returns>
    [HttpPut("/posts/{id}")]
    public ActionResult<ResponseEnvelope<LinkedResource<Post>>> UpdatePost(int id, [FromBody] PostRequest post)
    {
        post.Validate();
        if (id != post.Id)
        {
            throw new InvalidDataException($"Id {id} does not match resource id {post.Id}.");
        }
        var createdPost = _dataService.UpdatePost(id, post);

        var linkedPost = createdPost.ToLinkedResource(new Link[]
        {
            Url.GetLink("self", this.GetPost, new { id = createdPost.Id })
        });
        return Ok(linkedPost.ToResponseEnvelope());
    }

    /// <summary>
    /// Deletes an existing post.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("/posts/{id}")]
    [Authorize]
    public ActionResult DeletePost(int id)
    {
        return null;
    }

    /// <summary>
    /// Gets a post by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("/posts/{id}")]
    public ActionResult<Post> GetPost(int id)
    {
        var post = _dataService.GetPost(id);
        if (post == null)
        {
            throw new KeyNotFoundException($"Post {id} not found.");
        }

        var linkedPost = post.ToLinkedResource(new Link[] {
            Url.GetLink("Update", this.UpdatePost, new { id = id }),
            Url.GetLink("Delete", this.DeletePost, new { id = id })
        });
        return Ok(linkedPost.ToResponseEnvelope());
    }

    /// <summary>
    /// Gets a post by slug.
    /// </summary>
    /// <param name="slug"></param>
    /// <returns></returns>
    [HttpGet("/{slug}")]
    public ActionResult<Post> GetPostBySlug(string slug)
    {
        return Ok(_dataService.GetPostBySlug(slug));
    }

    /// <summary>
    /// Gets the related posts.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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
