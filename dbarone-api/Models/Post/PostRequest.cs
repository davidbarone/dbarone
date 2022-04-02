namespace dbarone_api.Models.Post;
using dbarone_api.Lib.Validation;

/// <summary>
/// Post model used in requests (creating and updating posts)
/// </summary>
public class PostRequest
{
    /// <summary>
    /// The post Id.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// The post title.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// The post slug.
    /// </summary>
    public string? Slug { get; set; }

    /// <summary>
    /// The post teaser content.
    /// </summary>
    public string? Teaser { get; set; }

    /// <summary>
    /// The post main body content.
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// The post code content.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// The post style content.
    /// </summary>
    public string? Style { get; set; }

    /// <summary>
    /// The post head content.
    /// </summary>
    public string? Head { get; set; }

    /// <summary>
    /// The post type.
    /// </summary>
    public string PostType { get; set; } = "HTML";

    /// <summary>
    /// The post parent id
    /// </summary>
    public int? ParentId { get; set; }
}