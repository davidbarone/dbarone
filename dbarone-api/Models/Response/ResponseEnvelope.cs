namespace dbarone_api.Models.Response;
using dbarone_api.Lib.Validation;
using dbarone_api.Lib.ObjectMapper;

/// <summary>
/// All responses to be wrapped in an envelope.
/// </summary>
public class ResponseEnvelope<T>
{
    /// <summary>
    /// The status of the response.
    /// </summary>
    public ResponseStatus Status { get; set; }
    
    /// <summary>
    /// The data / payload response
    /// </summary>
    public T? Data { get; set; }
    
    /// <summary>
    /// Messages attached to the response (for example validation messages).
    /// </summary>
    public IEnumerable<ResponseMessage>? Messages { get; set; }

   /// <summary>
   /// REST links relating to the response. Typically used for pagination.
   /// </summary>
   public IEnumerable<Link>? Links { get; set; }

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
    /// Fluent syntax for adding a message to existing ResponseEnvelope.
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
    /// Fluent syntax for adding links to existing ResponseEnvelope.
    /// </summary>
    /// <param name="links"></param>
    /// <returns></returns>
    public ResponseEnvelope<T> AddLinks(IEnumerable<Link> links)
    {
        if (this.Links == null)
        {
            this.Links = new List<Link>();
        }
        this.Links = this.Links.Union(links);
        return this;
    }

    /// <summary>
    /// Fluent syntax for adding link to existing ResponseEnvelope.
    /// </summary>
    /// <param name="link"></param>
    /// <returns></returns>
    public ResponseEnvelope<T> AddLink(Link link)
    {
        if (this.Links == null)
        {
            this.Links = new List<Link>();
        }
        this.Links.Append(link);
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
                Messages = new List<ResponseMessage> { new ResponseMessage() { Message = e.Message, Type = ResponseMessageType.ERROR } }
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