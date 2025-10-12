using Microsoft.Extensions.Options;

public class UserService : IUserService
{
    private IDataStore _dataStore;
    private IJwtUtils _jwtUtils;
    private readonly SecuritySettings _securitySettings;

    public UserService(
        IDataStore dataStore,
        IJwtUtils jwtUtils,
        IOptions<SecuritySettings> securitySettings)
    {
        _dataStore = dataStore;
        _jwtUtils = jwtUtils;
        _securitySettings = securitySettings.Value;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress)
    {
        var user = _dataStore.Get<User>()
        .SingleOrDefault(x => x.UserName == model.UserName);

        // validate
        if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            throw new SecurityException("Username or password is incorrect");

        // authentication successful so generate jwt and refresh tokens
        var jwtToken = _jwtUtils.GenerateJwtToken(user);
        var refreshToken = _jwtUtils.GenerateRefreshToken(user.Id, ipAddress);

        // remove old refresh tokens from user
        removeOldRefreshTokens(user);

        return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
    }

    public AuthenticateResponse RefreshToken(string token, string ipAddress)
    {
        var user = getUserByRefreshToken(token);
        var refreshToken = GetRefreshTokens(user.Id).Single(x => x.Token == token);

        if (refreshToken.IsRevoked)
        {
            // revoke all descendant tokens in case this token has been compromised
            revokeDescendantRefreshTokens(refreshToken, user, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
        }

        if (!refreshToken.IsActive)
            throw new SecurityException("Invalid token");

        // replace old refresh token with a new one (rotate token)
        var newRefreshToken = rotateRefreshToken(refreshToken, ipAddress);
        _dataStore.Insert<RefreshToken>(newRefreshToken);

        // remove old refresh tokens from user
        removeOldRefreshTokens(user);

        // generate new jwt
        var jwtToken = _jwtUtils.GenerateJwtToken(user);

        return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
    }

    public void RevokeToken(string token, string ipAddress)
    {
        var user = getUserByRefreshToken(token);
        var refreshToken = GetRefreshTokens(user.Id).Single(x => x.Token == token);

        if (!refreshToken.IsActive)
            throw new SecurityException("Invalid token");

        // revoke token and save
        revokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");
    }

    public IEnumerable<User> GetAll()
    {
        return _dataStore.Get<User>();
    }

    public User GetById(int id)
    {
        var user = GetAll().SingleOrDefault(u => u.Id == id);
        if (user == null) throw new SecurityException("User not found");
        return user;
    }

    // helper methods

    private User getUserByRefreshToken(string token)
    {
        var t = _dataStore.Get<RefreshToken>().SingleOrDefault(t => t.Token == token);
        if (t is not null)
        {
            var userId = t.UserId;
            var user = _dataStore.GetById<User>(userId);
            if (user == null)
                throw new SecurityException("Invalid token");
            return user;
        }
        throw new SecurityException("Invalid token");
    }

    private RefreshToken rotateRefreshToken(RefreshToken refreshToken, string ipAddress)
    {
        var user = getUserByRefreshToken(refreshToken.Token);
        var newRefreshToken = _jwtUtils.GenerateRefreshToken(user.Id, ipAddress);
        revokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }

    private void removeOldRefreshTokens(User user)
    {
        // remove old inactive refresh tokens from user based on TTL in app settings
        var userTokens = GetRefreshTokens(user.Id);
        foreach (var token in userTokens)
        {
            if (!token.IsActive && token.CreatedDt.AddDays(_securitySettings.RefreshTokenTTL) <= DateTime.UtcNow)
            {
                _dataStore.Delete<RefreshToken>(token.Id);
            }
        }
    }

    private void revokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string ipAddress, string reason)
    {
        // recursively traverse the refresh token chain and ensure all descendants are revoked
        if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
        {
            var childToken = GetRefreshTokens(user.Id).SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
            if (childToken.IsActive)
                revokeRefreshToken(childToken, ipAddress, reason);
            else
                revokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
        }
    }

    private void revokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
    {
        token.RevokedDt = DateTime.UtcNow;
        token.RevokedByIp = ipAddress;
        token.RevokedReason = reason;
        token.ReplacedByToken = replacedByToken;
    }

    public IEnumerable<RefreshToken> GetRefreshTokens(int? userId)
    {
        return _dataStore.Get<RefreshToken>()
        .Where(t => t.UserId == userId);
    }
}