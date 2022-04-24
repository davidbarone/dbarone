namespace dbarone_api.Models.Response;

/// <summary>
/// Represents an error / notification added to a response. Typically used to send validation information back to client.
/// </summary>
public class ResponseMessage
{
    /// <summary>
    /// A string label identifying the source of the message. For validation messages, this is typically the attribute name.
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// The type of message.
    /// </summary>
    public ResponseMessageType Type { get; set; } = ResponseMessageType.INFORMATION;

    /// <summary>
    /// The message.
    /// </summary>
    public string? Message { get; set; }
}
