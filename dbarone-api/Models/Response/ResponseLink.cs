using dbarone_api.Models.Response;
using System.Text.Json.Serialization;

public class ResponseLink
{
    public string Rel { get; set; }
    public string Uri { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public HttpVerb Method { get; set; } = HttpVerb.GET;
    public string Title { get; set; }
}