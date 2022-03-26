namespace dbarone_api.Models.Response;


/// <summary>
/// All responses to be wrapped in an envelope.
/// </summary>
public class ResponseEnvelope<T>
{
    public ResponseStatus Status { get; set; }
    public T Data { get; set; }
    public IEnumerable<ResponseMessage> Messages { get; set; }
    public IList<ResponseLink> Links { get; set; }
}