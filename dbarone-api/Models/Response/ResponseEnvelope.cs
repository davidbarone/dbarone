namespace dbarone_api.Models.Response;
using dbarone_api.Lib.Validation;
using dbarone_api.Lib.ObjectMapper;

/// <summary>
/// All responses to be wrapped in an envelope.
/// </summary>
public class ResponseEnvelope<T>
{
    public ResponseStatus Status { get; set; }
    public T? Data { get; set; }
    public IEnumerable<ResponseMessage> Messages { get; set; }
    public IList<ResponseLink> Links { get; set; }

    /// <summary>
    /// Factory method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="e"></param>
    /// <param name="messages"></param>
    /// <param name="links"></param>
    /// <returns></returns>
    public static ResponseEnvelope<T> Create(T? data = default, Exception? e = null, IEnumerable<ResponseMessage>? messages = null, IList<ResponseLink>? links = null)
    {
        var validationException = e as ValidationException;
        if (validationException != null)
        {
            if (messages == null)
            {
                messages = new List<ResponseMessage>();
            }
            // Map validation results to response messages
            var mapper = ObjectMapper<ValidationResult, ResponseMessage>.Create(
                new CustomMapperStrategy<ValidationResult, ResponseMessage>()
                .Rule(from => from.Key, to => to.Source)
                .Rule(from => from.Message, to => to.Message));

            var validationMessages = mapper.MapMany(validationException.Results);
            messages = messages.Union(validationMessages);
            return new ResponseEnvelope<T>
            {
                Status = new ResponseStatus
                {
                    Success = false,
                    Message = validationException.Message
                },
                Data = default,
                Messages = messages,
                Links = links!
            };
        }
        else
        {
            return new ResponseEnvelope<T>
            {
                Status = new ResponseStatus
                {
                    Success = (e == null) ? true : false,
                    Message = e?.Message ?? ""
                },
                Data = data,
                Messages = messages!,
                Links = links!
            };
        }
    }
}