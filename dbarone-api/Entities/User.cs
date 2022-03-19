namespace dbarone_api.Entities;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class User
{
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Username { get; set; } = default!;

    [JsonIgnore]
    public string Hash { get; set; } = default!;
    public string Email { get; set; } = default!;
    public char Status { get; set; }

    public DateTime CreatedDt { get; set; }
    public DateTime UpdatedDt { get; set; }
    public string CreatedBy { get; set; } = default!;
    public string? UpdatedBy { get; set; }

    //[JsonIgnore]
    //public List<RefreshToken> RefreshTokens { get; set; }
}