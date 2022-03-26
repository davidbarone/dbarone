namespace dbarone_api.Models;
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
    [StringLengthValidator(Min = 1, Max = 250)]
    public string? Title { get; set; }

    /// <summary>
    /// The post slug.
    /// </summary>
    [StringLengthValidator(Min = 1, Max = 250)]
    public string? Slug { get; set; }

    /// <summary>
    /// The post teaser content.
    /// </summary>
    [StringLengthValidator(Min = 1, Max = 1000)]
    public string? Teaser { get; set; }

    /// <summary>
    /// The post main body content.
    /// </summary>
    [StringLengthValidator(Min = 1, Max = Int16.MaxValue)]
    public string? Content { get; set; }

    /// <summary>
    /// The post code content.
    /// </summary>
    [StringLengthValidator(Min = 1, Max = Int16.MaxValue)]
    public string? Code { get; set; }

    /// <summary>
    /// The post style content.
    /// </summary>
    [StringLengthValidator(Min = 1, Max = Int16.MaxValue)]
    public string? Style { get; set; }

    /// <summary>
    /// The post head content.
    /// </summary>
    [StringLengthValidator(Min = 1, Max = Int16.MaxValue)]
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