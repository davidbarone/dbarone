namespace dbarone_api.Services;
using Dapper;
using dbarone_api.Entities;
using dbarone_api.Data;
using dbarone_api.Models.Users;
using dbarone_api.Models.Post;

public class DataService : IDataService
{
    private readonly IDataContext _context;
    public DataService(IDataContext context)
    {
        _context = context;
    }

    public IDataContext Context { get { return _context; } }

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
    public User CreateUser(UserRequest userRequest)
    {

        var now = DateTime.Now;
        var systemUser = "system";
        var hash = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);

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
            Username = userRequest.Username,
            Hash = hash,
            Firstname = userRequest.FirstName,
            Lastname = userRequest.LastName,
            Email = userRequest.Email,
            Status = "P",       // published
            CreatedDt = now,
            CreatedBy = systemUser,
            UpdatedDt = now,
            UpdatedBy = systemUser
        }).First();

        return user;
    }

    #region Resources

    public void DeleteResource(int id)
    {
        _context.Execute(@"DELETE FROM Resource WHERE Id = @Id", new { Id = id });
    }

    public IEnumerable<Resource> GetResources()
    {
        var resources = _context.Query<Resource>(@"SELECT * FROM Resource");
        return resources;
    }

    public Resource GetResource(int id)
    {
        var resource = _context.Query<Resource>(@"SELECT * FROM Resource WHERE Id = @Id", new { Id = id }).FirstOrDefault();
        return resource;
    }

    public Resource GetResourceByFilename(string filename)
    {
        var resource = _context.Query<Resource>(@"SELECT * FROM Resource WHERE Filename = @Filename", new { Filename = filename }).FirstOrDefault();
        return resource;
    }

    public Resource CreateResource(string filename, string contentType, byte[] data)
    {
        var resource = _context.Query<Resource>(@"
DECLARE @Id INT;
SELECT @Id = NEXT VALUE FOR ResourceSeq;

DELETE FROM
    [Resource]
WHERE
    Filename = @Filename;

INSERT INTO
    Resource (Id, Filename, Data, ContentType, Status, CreatedDt, CreatedBy)
SELECT
    @Id, @Filename, @Data, @ContentType, @Status, @CreatedDt, @CreatedBy;

SELECT * FROM [Resource] WHERE Id = @Id
    ", new
        {
            Filename = filename,
            Data = data,
            ContentType = contentType,
            Status = 'P',
            CreatedDt = DateTime.Now,
            CreatedBy = "system"
        }).First();

        return resource;
    }

    #endregion
}