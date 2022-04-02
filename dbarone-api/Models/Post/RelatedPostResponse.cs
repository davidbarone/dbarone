namespace dbarone_api.Models.Post;
using dbarone_api.Models.Response;
using dbarone_api.Entities;

/// <summary>
/// Related posts model.
/// </summary>
public class RelatedPostResponse
{
    /// <summary>
    /// The current post.
    /// </summary>
    public LinkedResource<PostSummaryResponse?>? Current { get; set; }

    /// <summary>
    /// The parent post.
    /// </summary>
    public LinkedResource<PostSummaryResponse?>? Parent { get; set; }

    /// <summary>
    /// Sibling posts.
    /// </summary>
    public IEnumerable<LinkedResource<PostSummaryResponse?>?>? Siblings { get; set; } = default!;

    /// <summary>
    /// Child posts.
    /// </summary>
    public IEnumerable<LinkedResource<PostSummaryResponse?>?>? Children { get; set; } = default!;
}
