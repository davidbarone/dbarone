using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Dbarone.Net.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class Extensions
{
    /// <summary>
    /// Very simple record mapper.
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static TTarget MapTo<TTarget>(this object source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var sourceType = source.GetType();
        var targetType = typeof(TTarget);

        var ctor = targetType.GetConstructors().FirstOrDefault();
        if (ctor == null) throw new InvalidOperationException("Target type must have a constructor.");

        var sourceProps = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                    .ToDictionary(p => p.Name, p => p.GetValue(source));

        var ctorParams = ctor.GetParameters();
        var args = ctorParams.Select(p =>
        {
            if (sourceProps.TryGetValue(p.Name, out var value) && p.ParameterType.IsInstanceOfType(value))
                return value;
            return GetDefault(p.ParameterType);
        }).ToArray();

        return (TTarget)ctor.Invoke(args);
    }

    private static object GetDefault(Type type) =>
        type.IsValueType ? Activator.CreateInstance(type) : null;


    /// <summary>
    /// This allows for a custom extension method to be added
    /// to controllers to define the authorisation policy.
    /// The custom security framework uses filters to attach
    /// to controller methods. Out of the box, the only way
    /// to parameterise filters is via factory methods, and
    /// this this extension method wraps the logic into a
    /// simple-to-use method.
    /// </summary>
    /// <param name=""></param>
    public static RouteHandlerBuilder RequireAuthorisation(this RouteHandlerBuilder builder, string resource, char permission)
    {
        builder.AddEndpointFilter((context, next) =>
        {
            var authorised = DoAuthorisation(context, resource, permission);
            if (!authorised)
            {
                return new ValueTask<object?>(new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized });
            }
            else
            {
                return next(context);
            }
        });
        return builder;
    }

    public static RouteHandlerBuilder AllowAnonymous(this RouteHandlerBuilder builder)
    {
        builder.AddEndpointFilter((context, next) =>
        {
            var authorised = DoAuthorisation(context, "", '\0');
            if (!authorised)
            {
                return new ValueTask<object?>(new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized });
            }
            else
            {
                return next(context);
            }
        });
        return builder;
    }

    private static bool DoAuthorisation(EndpointFilterInvocationContext context, string resource, char permission)
    {
        // skip authorization if no resource / permission set
        if (resource.IsNullOrEmpty() && permission == 0)
        {
            // No auth check
            return true;
        }
        else
        {
            // authorization
            var user = (UserSafe)context.HttpContext.Items["User"];

            // TODO: Need to check claims against resource/permission here
            if (user == null) return false;

            return true;
        }
    }

    public static void AddCustomSecurity(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<SecuritySettings>(builder.Configuration.GetSection("SecuritySettings"));
        builder.Services.AddScoped<IJwtUtils, JwtUtils>();
        builder.Services.AddScoped<IUserService, UserService>();

        // configures swagger with the authorise button
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, CustomSecuritySwaggerOptions>();
    }

    /// <summary>
    /// Configures custom security in your app / api.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseCustomSecurity(this WebApplication app)
    {
        app.RegisterUserEndpoints();
        app.UseMiddleware<JwtMiddleware>();
        return app;
    }

}