namespace dbarone_api.Authorization;

using Microsoft.Extensions.Options;
using dbarone_api.Helpers;
using dbarone_api.Services;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtUtils.ValidateJwtToken(token);
        if (userId != null)
        {
            // attach user to context on successful jwt validation
            context.Items["User"] = userService.GetById(userId.Value);
        }

        await _next(context);
    }
}