namespace dbarone_api.Models.Post;

/// <summary>
/// Post model used in requests (creating and updating posts)
/// </summary>
public class PostSummaryResponse
{
    /// <summary>
    /// Primary key for post.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Post title.
    /// </summary>
    public string Title { get; set; } = default!;

    /// <summary>
    /// Optional slug used for post.
    /// </summary>
    public string? Slug { get; set; }

    /// <summary>
    /// Short text for the post.
    /// </summary>
    public string? Teaser { get; set; }

    /// <summary>
    /// Created date for the post.
    /// </summary>
    public DateTime CreatedDt { get; set; }

    /// <summary>
    /// Created by for the post.
    /// </summary>
    public string? CreatedBy { get; set; }
}