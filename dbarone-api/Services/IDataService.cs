namespace dbarone_api.Services;
using dbarone_api.Data;
using dbarone_api.Entities;
using dbarone_api.Models.Users;
using dbarone_api.Models.Post;

public interface IDataService
{
    IDataContext Context { get; }
    public IEnumerable<User> GetUsers();
    public void AddRefreshToken(User user, RefreshToken refreshToken, int refreshTokenTtlDays);
    public void UpdateRefreshToken(int refreshTokenId, RefreshToken refreshToken);

    #region Users
    public User CreateUser(UserRequest userRequest);

    #endregion

    #region Refresh Tokens

    public IEnumerable<RefreshToken> GetRefreshTokens(int? userId);

    #endregion
}
