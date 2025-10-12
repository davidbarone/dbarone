using Microsoft.Extensions.Options;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ApiSettings _apiSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<ApiSettings> apiSettings)
    {
        _next = next;
        _apiSettings = apiSettings.Value;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils, IDataStore dataStore)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (!(token is null))
        {
            var userId = jwtUtils.ValidateJwtToken(token);
            if (userId != null)
            {
                // attach user to context on successful jwt validation
                var user = dataStore.GetById<User>(userId.Value);
                if (user is not null)
                {
                    context.Items["User"] = user.MapTo<UserSafe>();
                }
            }
        }

        await _next(context);
    }
}