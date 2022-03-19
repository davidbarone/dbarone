using dbarone_api.Entities;

public class RelatedPostResponse
{
    public Post? Parent { get; set; }
    public IEnumerable<Post> Siblings { get; set; } = default!;
    public IEnumerable<Post> Children { get; set; } = default!;
}
