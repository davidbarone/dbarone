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

    /// <summary>
    /// Fluent syntax for adding messages to existing ResponseEnvelope.
    /// </summary>
    /// <param name="messages"></param>
    /// <returns></returns>
    public ResponseEnvelope<T> AddMessages(IEnumerable<ResponseMessage> messages)
    {
        if (this.Messages == null)
        {
            this.Messages = new List<ResponseMessage>();
        }
        this.Messages = this.Messages.Union(messages);
        return this;
    }

    /// <summary>
    /// Fluent syntax for adding messages to existing ResponseEnvelope.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public ResponseEnvelope<T> AddMessage(ResponseMessage message)
    {
        if (this.Messages == null)
        {
            this.Messages = new List<ResponseMessage>();
        }
        this.Messages.Append(message);
        return this;
    }

    /// <summary>
    /// Create a response envelope from an exception.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public static ResponseEnvelope<T> Create(Exception e)
    {
        var validationException = e as ValidationException;
        if (validationException != null)
        {
            // Map validation results to response messages
            var mapper = ObjectMapper<ValidationResult, ResponseMessage>.Create(
                new CustomMapperStrategy<ValidationResult, ResponseMessage>()
                .Configure(from => from.Key, to => to.Source)
                .Configure(from => from.Message, to => to.Message));

            var validationMessages = mapper.MapMany(validationException.Results);
            return new ResponseEnvelope<T>
            {
                Status = new ResponseStatus
                {
                    Success = false,
                    Message = validationException.Message
                },
                Data = default,
                Messages = validationMessages
            };
        }
        else
        {
            // other exception
            return new ResponseEnvelope<T>
            {
                Status = new ResponseStatus
                {
                    Success = false,
                    Message = e.Message
                },
                Data = default,
                Messages = new List<ResponseMessage> { { ResponseMessage.CreateError(e.Message) } }
            };
        }
    }

    /// <summary>
    /// Factory method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="messages"></param>
    /// <returns></returns>
    public static ResponseEnvelope<T> Create(T? data = default)
    {
        return new ResponseEnvelope<T>
        {
            Status = new ResponseStatus
            {
                Success = true,
                Message = ""
            },
            Data = data
        };
    }
}