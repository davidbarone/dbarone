namespace dbarone_api.Entities;

using System.Text.Json.Serialization;

public class Resource
{
    public int Id { get; set; }
    public string Filename { get; set; } = default!;
    [JsonIgnore]
    public byte[] Data { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public char Status { get; set; }
    public DateTime CreatedDt { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTime UpdatedDt { get; set; }
    public string UpdatedBy { get; set; } = default!;
    public int Size => Data.Length;
}