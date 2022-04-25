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
    public PostSummaryResponse? Current { get; set; }

    /// <summary>
    /// The parent post.
    /// </summary>
    public PostSummaryResponse? Parent { get; set; }

    /// <summary>
    /// Sibling posts.
    /// </summary>
    public IEnumerable<PostSummaryResponse?>? Siblings { get; set; }

    /// <summary>
    /// Child posts.
    /// </summary>
    public IEnumerable<PostSummaryResponse?>? Children { get; set; }

    /// <summary>
    /// Set to true if the post has any relations.
    /// </summary>
    public bool HasRelations => Parent!=null || (Siblings!=null && Siblings.Any()) || (Children!=null && Children.Any());
}
