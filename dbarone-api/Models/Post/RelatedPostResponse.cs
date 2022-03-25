namespace dbarone_api.Models;

using dbarone_api.Entities;

public class RelatedPostResponse
{
    public PostSummaryResponse? Parent { get; set; }
    public IEnumerable<PostSummaryResponse> Siblings { get; set; } = default!;
    public IEnumerable<PostSummaryResponse> Children { get; set; } = default!;
}
