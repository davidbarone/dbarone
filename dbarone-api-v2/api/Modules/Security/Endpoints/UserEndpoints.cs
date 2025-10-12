using Microsoft.AspNetCore.Http.HttpResults;

public static class UserEndpoints
{
    public static void RegisterUserEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("/users", (UserSafe user, IDataStore dataStore) =>
        {
            var next = dataStore.Insert(user);
            return Results.Ok(next);
        })
        .WithOpenApi()
        .WithTags("Security:Users");

        routeBuilder.MapGet("/users", (IDataStore dataStore) =>
        {
            var users = dataStore.Get<User>().Select(u => u.MapTo<UserSafe>());
            return Results.Ok(users);
        })
        .WithOpenApi()
        .WithTags("Security:Users")
        .RequireAuthorisation("Users", 'r');

        routeBuilder.MapGet("/users/{id:int}", (int id, IDataStore dataStore) =>
        {
            var users = dataStore.Get<UserSafe>();
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(user);
        })
        .WithOpenApi()
        .WithTags("Security:Users");

        routeBuilder.MapPut("/users/{id:int}", (int id, UserSafe user, IDataStore dataStore) =>
        {
            dataStore.Update<UserSafe>(id, user);
            return Results.Ok();
        })
        .WithOpenApi()
        .WithTags("Security:Users");

        routeBuilder.MapDelete("/users/{id:int}", (int id, IDataStore dataStore) =>
        {
            dataStore.Delete<UserSafe>(id);
            return Results.Ok("sdfs");
        })
        .WithOpenApi()
        .WithTags("Security:Users");

        routeBuilder.MapPost("/users/authenticate", (AuthenticateRequest model, IUserService userService, IDataStore dataStore, HttpRequest request, HttpContext context) =>
        {
            var response = userService.Authenticate(model, UserEndpoints.IpAddress(request, context));

            if (response == null)
                return Results.BadRequest("Username or password is incorrect");

            return Results.Ok(response);

        })
        .WithOpenApi()
        .WithTags("Security:Authentication")
        .WithDescription("Provides mechanism to authenticate to the system and get an access token.")
        .AllowAnonymous();

        routeBuilder.MapPost("users/refresh-token", (IUserService userService, IDataStore dataStore, HttpRequest request, HttpContext context, HttpResponse response) =>
        {
            var refreshToken = request.Cookies["refreshToken"];
            var result = userService.RefreshToken(refreshToken, IpAddress(request, context));
            setTokenCookie(response, result.RefreshToken);
            return Results.Ok(result);
        })
        .WithOpenApi()
        .WithTags("Security:Authentication")
        .WithDescription("Provides mechanism to authenticate to the system and get an access token.")
        .AllowAnonymous();

        routeBuilder.MapPost("users/revoke-token", (RevokeTokenRequest model, IUserService userService, IDataStore dataStore, HttpRequest request, HttpContext context, HttpResponse response) =>
        {
            // accept refresh token in request body or cookie
            var token = model.Token ?? request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return Results.BadRequest("Token is required");

            userService.RevokeToken(token, UserEndpoints.IpAddress(request, context));
            return Results.Ok("Token revoked");
        })
        .WithOpenApi()
        .WithTags("Security:Authentication")
        .WithDescription("Revokes a token.")
        .WithSummary("Revokes a token.")
        .AllowAnonymous();

        routeBuilder.MapGet("/users/{id:int}/refresh-tokens", (int id, IUserService userService) =>
        {
            var tokens = userService.GetRefreshTokens(id);
            return Results.Ok(tokens);
        })
        .WithOpenApi()
        .WithTags("Security:Authentication")
        .WithDescription("Gets the refresh tokens for a user.")
        .AllowAnonymous();

    }

    #region Helper methods

    private static void setTokenCookie(HttpResponse response, string token)
    {
        // append cookie with refresh token to the http response
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        response.Cookies.Append("refreshToken", token, cookieOptions);
    }
    private static string IpAddress(HttpRequest request, HttpContext context)
    {
        // get source ip address for the current request
        if (request.Headers.ContainsKey("X-Forwarded-For"))
            return request.Headers["X-Forwarded-For"];
        else
            return context.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }

    #endregion
}