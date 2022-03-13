namespace dbarone_api.Repository;
using Dapper;
using dbarone_api.Models;
using dbarone_api.Data;

public class DbaroneRepository : IRepository
{
    private readonly DapperContext _context;
    public DbaroneRepository(DapperContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<PostInfo>> GetPosts()
    {
        // C# 8.0 implied using declarations
        using var db = _context.CreateConnection();
        var posts = await db.QueryAsync<PostInfo>("SELECT * FROM Post");
        return posts;
    }
}