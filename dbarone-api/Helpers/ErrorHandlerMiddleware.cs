namespace dbarone_api.Helpers;

using System.Net;
using System.Text.Json;
using dbarone_api.Lib.Validation;
using dbarone_api.Models.Response;

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
            ResponseEnvelope<object> envelope = ResponseEnvelope<object>.Create(null, error, null, null);

            switch (error)
            {
                case ValidationException e:
                    envelope = ResponseEnvelope<object>.Create(null, e, null, null);
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case AppException e:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(envelope);
            await response.WriteAsync(result);
        }
    }
}