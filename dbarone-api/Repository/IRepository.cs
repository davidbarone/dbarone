namespace dbarone_api.Repository;

using dbarone_api.Models;

public interface IRepository
{
    public Task<IEnumerable<PostInfo>> GetPosts();
}