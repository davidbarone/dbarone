namespace dbarone_api.Models.Response;

public class ResponseMessage
{
    public static ResponseMessage CreateError(string message, string? source = null)
    {
        return new ResponseMessage(message, ResponseMessageType.ERROR, source);
    }

    public static ResponseMessage CreateInformation(string message, string? source = null)
    {
        return new ResponseMessage(message, ResponseMessageType.INFORMATION, source);
    }

    public static ResponseMessage CreateWarning(string message, string? source = null)
    {
        return new ResponseMessage(message, ResponseMessageType.WARNING, source);
    }

    public ResponseMessage(string message, ResponseMessageType type = ResponseMessageType.ERROR, string? source = null)
    {
        this.Message = message;
        this.Type = type;
        this.Source = source;
    }

    public string? Source { get; set; }
    public ResponseMessageType Type { get; set; }
    public string Message { get; set; }
}
