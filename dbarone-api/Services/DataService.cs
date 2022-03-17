namespace dbarone_api.Services;
using Dapper;
using dbarone_api.Entities;
using dbarone_api.Data;

public interface IDataService
{
    public IEnumerable<Post> GetPosts();
    public IEnumerable<User> GetUsers();
    public void AddRefreshToken(User user, RefreshToken refreshToken, int refreshTokenTtlDays);
}

public class DataService : IDataService
{
    private readonly DapperContext _context;
    public DataService(DapperContext context)
    {
        _context = context;
    }

    public IEnumerable<Post> GetPosts()
    {
        var items = _context.Query<Post>("SELECT * FROM Post");
        return items ?? new List<Post>();
    }

    public IEnumerable<User> GetUsers()
    {
        var items = _context.Query<User>("SELECT * FROM [User]");
        return items ?? new List<User>();
    }

    /// <summary>
    /// Adds new refresh token, and removes old refresh tokens
    /// </summary>
    /// <param name="user"></param>
    /// <param name=""></param>
    public void AddRefreshToken(User user, RefreshToken refreshToken, int refreshTokenTtlDays)
    {
        _context.Execute(@"
INSERT INTO RefreshToken (
    [UserId], [Token], [Expires], [Created], [CreatedByIp], [Revoked], [RevokedByIp], [ReplacedByToken], [ReasonRevoked]
) VALUES (
    @UserId, @Token, @Expires, @Created, @CreatedByIp, @Revoked, @RevokedByIp, @ReplacedByToken, @ReasonRevoked
);

DELETE FROM RefreshToken WHERE UserId = @UserId AND Created < DATEADD(dd, @TtlDays, GETDATE());
", new
        {
            UserId = user.Id,
            Token = refreshToken.Token,
            Expires = refreshToken.Expires,
            Created = refreshToken.Created,
            CreatedByIp = refreshToken.CreatedByIp,
            Revoked = refreshToken.Revoked,
            RevokedByIp = refreshToken.RevokedByIp,
            ReplacedByToken = refreshToken.ReplacedByToken,
            ReasonRevoked = refreshToken.ReasonRevoked,
            TtlDays = refreshTokenTtlDays
        });
    }


}