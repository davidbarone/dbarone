namespace dbarone_api.Services;
using Dapper;
using dbarone_api.Entities;
using dbarone_api.Data;
using dbarone_api.Models.Users;
using dbarone_api.Models.Post;

public interface IDataService
{
    public IEnumerable<User> GetUsers();
    public void AddRefreshToken(User user, RefreshToken refreshToken, int refreshTokenTtlDays);
    public void UpdateRefreshToken(int refreshTokenId, RefreshToken refreshToken);

    #region Posts
    public IEnumerable<Post> GetPosts();
    public Post? GetPost(int id);
    public Post? GetPostBySlug(string slug);
    public Post? GetPostParent(int id);
    public IEnumerable<Post> GetPostSiblings(int id);
    public IEnumerable<Post> GetPostChildren(int id);
    public Post CreatePost(PostRequest post);

    #endregion

    #region Users
    public User CreateUser(UserRequest userRequest);

    #endregion

    #region Refresh Tokens

    public IEnumerable<RefreshToken> GetRefreshTokens(int? userId);

    #endregion

    #region Resources

    public Resource CreateResource(string filename, string contentType, byte[] data);
    public IEnumerable<Resource> GetResources();
    public Resource GetResource(int id);
    public Resource GetResourceByFilename(string filename);
    public void DeleteResource(int id);

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

    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="post">Post to create.</param>
    /// <returns></returns>
    public Post CreatePost(PostRequest post)
    {
        var now = DateTime.Now;

        var newPost = _context.Query<Post>(@"
DECLARE @Id INT
SELECT @Id = NEXT VALUE FOR PostSeq

INSERT INTO Post (
    [Id], [Title], [Slug], [Teaser], [Content], [Code], [Style], [Head], [PostType], [ParentId], [CreatedDt], [CreatedBy], [UpdatedDt], [UpdatedBy]
)
SELECT
    @Id, @Title, @Slug, @Teaser, @Content, @Code, @Style, @Head, @PostType, @ParentId, @CreatedDt, @CreatedBy, @UpdatedDt, @UpdatedBy;

SELECT * FROM Post WHERE Id = @Id;", new
        {
            Title = post.Title,
            Slug = post.Slug,
            Teaser = post.Teaser,
            Content = post.Content,
            Code = post.Code,
            Style = post.Style,
            Head = post.Head,
            PostType = post.PostType,
            ParentId = post.ParentId,
            CreatedDt = now,
            CreatedBy = "system",
            UpdatedDt = now,
            UpdatedBy = "system"
        }).First();
        return newPost;
    }

    public Post? GetPost(int id)
    {
        var post = _context.Query<Post>("SELECT * FROM Post WHERE Id = @Id", new { Id = id }).FirstOrDefault();
        return post;
    }

    public Post? GetPostBySlug(string slug)
    {
        var post = _context.Query<Post>("SELECT * FROM Post WHERE Slug = @Slug", new { Slug = slug }).FirstOrDefault();
        return post;
    }

    public Post? GetPostParent(int id)
    {
        var post = GetPost(id);
        if (post == null)
            return null;

        var parent = _context.Query<Post>("SELECT * FROM Post WHERE Id = @Id", new { Id = post.ParentId }).FirstOrDefault();
        return parent;
    }

    public IEnumerable<Post> GetPostSiblings(int id)
    {
        var parent = GetPostParent(id);

        if (parent == null)
            return new List<Post>();

        var siblings = _context.Query<Post>("SELECT * FROM Post WHERE ParentId = @ParentId AND Id <> @Id", new { ParentId = parent.Id, Id = id });
        return siblings;
    }

    public IEnumerable<Post> GetPostChildren(int id)
    {
        var children = _context.Query<Post>("SELECT * FROM Post WHERE ParentId = @ParentId", new { ParentId = id });
        return children;
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