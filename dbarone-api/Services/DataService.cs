namespace dbarone_api.Services;
using Dapper;
using dbarone_api.Entities;
using dbarone_api.Data;
using dbarone_api.Models.Users;

public interface IDataService
{
    public IEnumerable<Post> GetPosts();
    public IEnumerable<User> GetUsers();
    public void AddRefreshToken(User user, RefreshToken refreshToken, int refreshTokenTtlDays);
    public void UpdateRefreshToken(int refreshTokenId, RefreshToken refreshToken);

    #region Users
    public User CreateUser(UserModel userModel);

    #endregion

    #region Refresh Tokens

    public IEnumerable<RefreshToken> GetRefreshTokens(int? userId);

    #endregion
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

    public IEnumerable<RefreshToken> GetRefreshTokens(int? userId)
    {
        var items = _context.Query<RefreshToken>("SELECT * FROM [RefreshToken] WHERE UserId = @UserId OR @UserId IS NULL", new { UserId = userId });
        return items ?? new List<RefreshToken>();
    }

    public void UpdateRefreshToken(int refreshTokenId, RefreshToken refreshToken)
    {
        if (refreshTokenId != refreshToken.Id)
        {
            throw new Exception("Invalid refresh token id");
        }
        _context.Execute(@"
UPDATE RefreshToken
SET
    Revoked = @Revoked,
    RevokedByIp = @RevokedByIp,
    ReasonRevoked = @ReasonRevoked,
    ReplacedByToken = @ReplacedByToken
WHERE
    Id = @Id", new
        {
            Id = refreshTokenId,
            Revoked = refreshToken.Revoked,
            RevokedByIp = refreshToken.RevokedByIp,
            ReasonRevoked = refreshToken.ReasonRevoked,
            ReplacedByToken = refreshToken.ReplacedByToken
        });
    }

    /// <summary>
    /// Adds new refresh token, and removes old refresh tokens
    /// </summary>
    /// <param name="user"></param>
    /// <param name=""></param>
    public void AddRefreshToken(User user, RefreshToken refreshToken, int refreshTokenTtlDays)
    {
        _context.Execute(@"
DECLARE @Id INT
SELECT @Id = NEXT VALUE FOR UserSeq

INSERT INTO RefreshToken (
    [Id], [UserId], [Token], [Expires], [Created], [CreatedByIp], [Revoked], [RevokedByIp], [ReplacedByToken], [ReasonRevoked]
) VALUES (
    @Id, @UserId, @Token, @Expires, @Created, @CreatedByIp, @Revoked, @RevokedByIp, @ReplacedByToken, @ReasonRevoked
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
            TtlDays = -refreshTokenTtlDays
        });
    }


    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param name="userModel"></param>
    /// <returns></returns>
    public User CreateUser(UserModel userModel)
    {

        var now = DateTime.Now;
        var systemUser = "system";
        var hash = BCrypt.Net.BCrypt.HashPassword(userModel.Password);

        var user = _context.Query<User>(@"

DECLARE @Id INT
SELECT @Id = NEXT VALUE FOR UserSeq

DELETE FROM [User];

INSERT INTO [User] (
    [Id], [Username], [Hash], [Firstname], [Lastname], [Email], [Status], [CreatedDt], [CreatedBy], [UpdatedDt], [UpdatedBy]
) VALUES (
    @Id, @Username, @Hash, @Firstname, @Lastname, @Email, @Status, @CreatedDt, @CreatedBy, @UpdatedDt, @UpdatedBy
);

-- Return new user
SELECT * FROM [User] WHERE Id = @Id;", new
        {
            Username = userModel.Username,
            Hash = hash,
            Firstname = userModel.FirstName,
            Lastname = userModel.LastName,
            Email = userModel.Email,
            Status = "P",       // published
            CreatedDt = now,
            CreatedBy = systemUser,
            UpdatedDt = now,
            UpdatedBy = systemUser
        }).First();

        return user;
    }
}