namespace dbarone_api.Entities;
using dbarone_api.Lib.Validation;
using dbarone_api.Lib.Data;
using System.Text.Json.Serialization;

/// <summary>
/// Static resource / file entity.
/// </summary>
public class Resource
{
    /// <summary>
    /// The primary key / surrogate id of the resource.
    /// </summary>
    [Key(Order = 1)]
    [AutoGenerated(sequenceName: "ResourceSeq", onInsert: true)]
    public int Id { get; set; }

    /// <summary>
    /// The resource filename.
    /// </summary>
    [RequiredValidator]
    [StringLengthValidator(Min = 1, Max = 250)]
    public string Filename { get; set; } = default!;

    /// <summary>
    /// The resource data.
    /// </summary>
    [JsonIgnore]
    [RequiredValidator]
    public byte[] Data { get; set; } = default!;

    /// <summary>
    /// The resource content type.
    /// </summary>
    [RequiredValidator]
    [StringLengthValidator(Min = 1, Max = 250)]
    public string ContentType { get; set; } = default!;

    /// <summary>
    /// Resource status.
    /// </summary>
    public char Status { get; set; } = 'P';

    /// <summary>
    /// Datetime that the resource was created.
    /// </summary>
    [AutoGenerated(useCurrentDateTime: true, onInsert: true)]
    public DateTime CreatedDt { get; set; }

    /// <summary>
    /// User creating the resource.
    /// </summary>
    [AutoGenerated(useCurrentUser: true, onInsert: true)]
    public string CreatedBy { get; set; } = default!;

    /// <summary>
    /// Resource file size (read only).
    /// </summary>
    [NotMapped]
    public int Size => Data.Length;
}