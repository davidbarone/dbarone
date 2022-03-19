namespace dbarone_api.Models;

/// <summary>
/// Post model used in requests (creating and updating posts)
/// </summary>
public class PostRequest
{
    public int? Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Teaser { get; set; }
    public string Content { get; set; }
    public string Code { get; set; }
    public string Style { get; set; }
    public string Head { get; set; }
    public string PostType { get; set; }
    public int? ParentId { get; set; }
}