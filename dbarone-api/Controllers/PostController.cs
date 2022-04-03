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
    /// Returns a set of posts. Child posts are NOT included in the results.
    /// </summary>
    /// <returns>A set of posts.</returns>
    [HttpGet("/posts")]
    public ActionResult<ResponseEnvelope<LinkedResource<IEnumerable<LinkedResource<PostSummaryResponse>>>>> GetPosts(int pageSize = 10, int page = 1)
    {
        if (pageSize > 1000) pageSize = 1000;
        if (pageSize < 1) pageSize = 1;
        if (page < 1) page = 1;
        var mapper = ObjectMapper<Post, PostSummaryResponse>.Create();
        var posts = mapper.MapMany(_dataService.Context.Read<Post>().Where(p => !p.IsChild));
        var linkedPosts = posts.Select(g => g.ToLinkedResource(new Link[] {
            Url.GetLink("View", this.GetPost, new { id = g.Id })
        }));
        var linkedPage = linkedPosts.ToLinkedPaginatedResource(Url, this.GetPosts, pageSize, page);
        return Ok(linkedPage.ToResponseEnvelope());
    }

    /// <summary>
    /// Gets a post by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("/posts/{id}")]
    public ActionResult<ResponseEnvelope<LinkedResource<Post>>> GetPost(int id)
    {
        var post = _dataService.Context.Find<Post>(id);
        var linkedPost = post.ToLinkedResource(new Link[] {
            Url.GetLink("Related", this.GetRelatedPosts, new { id = id }),
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
    public ActionResult<ResponseEnvelope<LinkedResource<Post>>> GetPostBySlug(string slug)
    {
        var entity = _dataService.Context.Single<Post>(new { Slug = slug });
        return GetPost(entity.Id);
    }

    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="post">The post to create.</param>
    /// <returns></returns>
    [HttpPost("/posts")]
    public ActionResult<ResponseEnvelope<LinkedResource<Post>>> CreatePost(PostRequest post)
    {
        var p = ObjectMapper<PostRequest, Post>.Create().MapOne(post)!;
        p.Validate();
        var keys = _dataService.Context.Insert<Post>(p);
        var createdPost = _dataService.Context.Find<Post>(keys);

        // Links
        List<Link> links = new();
        links.Add(Url.GetLink("self", this.GetPost, new { id = createdPost.Id }));
        var linkedPost = createdPost.ToLinkedResource<Post>(links);
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
        if (id != post.Id)
        {
            throw new InvalidDataException($"Id {id} does not match resource id {post.Id}.");
        }
        var p = ObjectMapper<PostRequest, Post>.Create().MapOne(post)!;
        p.Validate();
        var keys = _dataService.Context.Update<Post>(p);
        var updatedPost = _dataService.Context.Find<Post>(keys);

        var linkedPost = updatedPost.ToLinkedResource(new Link[]
        {
            Url.GetLink("self", this.GetPost, new { id = updatedPost.Id })
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
    public ActionResult<ResponseEnvelope<LinkedResource<object?>>> DeletePost(int id)
    {
        _dataService.Context.Delete<Post>(id);
        var obj = ((object?)null).ToLinkedResource<object>(new Link[] {
            Url.GetLink("Parent", this.GetPosts, null)
        });
        return Ok(obj.ToResponseEnvelope());
    }

    /// <summary>
    /// Gets the related posts.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("/posts/{id}/related")]
    public ActionResult<ResponseEnvelope<RelatedPostResponse>> GetRelatedPosts(int id)
    {
        var mapper = ObjectMapper<Post, PostSummaryResponse>.Create();
        Post current = _dataService.Context.Single<Post>(new object[] { id });
        var parent = _dataService.Context.Find<Post>(new object[] { current.ParentId! });
        var siblings = _dataService.Context.Read<Post>(new { ParentId = current.ParentId });
        var children = _dataService.Context.Read<Post>(new { ParentId = current.Id });

        return Ok(new RelatedPostResponse
        {
            Current = mapper.MapOne(current).ToLinkedResource(new Link[] { Url.GetLink("self", this.GetPost, new { id = current.Id }) }),
            Parent = mapper.MapOne(parent).ToLinkedResource(new Link[] { Url.GetLink("self", this.GetPost, new { id = parent != null ? parent.Id : (int?)null }) }),
            Siblings = mapper.MapMany(siblings).Select(s => s.ToLinkedResource(new Link[] { Url.GetLink("self", this.GetPost, new { id = s.Id }) })),
            Children = mapper.MapMany(children).Select(s => s.ToLinkedResource(new Link[] { Url.GetLink("self", this.GetPost, new { id = s.Id }) }))
        }.ToResponseEnvelope());
    }
}
