namespace dbarone_api.Entities;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class RefreshToken
{
    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    public int UserId { get; set; }     // FK to user
    public string Token { get; set; } = default!;
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public string CreatedByIp { get; set; } = default!;
    public DateTime? Revoked { get; set; }
    public string? RevokedByIp { get; set; }
    public string? ReplacedByToken { get; set; }
    public string? ReasonRevoked { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsRevoked => Revoked != null;
    public bool IsActive => !IsRevoked && !IsExpired;
}