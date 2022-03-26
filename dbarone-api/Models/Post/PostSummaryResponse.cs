namespace dbarone_api.Models.Post;

/// <summary>
/// Post model used in requests (creating and updating posts)
/// </summary>
public class PostSummaryResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Teaser { get; set; }
}