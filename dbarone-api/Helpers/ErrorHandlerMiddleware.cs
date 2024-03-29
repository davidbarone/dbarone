namespace dbarone_api.Helpers;

using System.Net;
using System.Text.Json;
using dbarone_api.Lib.Validation;
using dbarone_api.Models.Response;
using System.Security;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            ResponseEnvelope<object> envelope = ResponseEnvelope<object>.Create(error);

            switch (error)
            {
                case ValidationException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case SecurityException e:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case AppException e:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case InvalidDataException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var result = JsonSerializer.Serialize(envelope, options);
            await response.WriteAsync(result);
        }
    }
}