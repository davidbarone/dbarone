namespace dbarone_api.Models.Post;
using dbarone_api.Models.Response;
using dbarone_api.Entities;

public class RelatedPostResponse
{
    public LinkedResource<PostSummaryResponse> Current { get; set; }
    public LinkedResource<PostSummaryResponse?> Parent { get; set; }
    public IEnumerable<LinkedResource<PostSummaryResponse>> Siblings { get; set; } = default!;
    public IEnumerable<LinkedResource<PostSummaryResponse>> Children { get; set; } = default!;
}
