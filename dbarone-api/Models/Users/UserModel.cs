namespace dbarone_api.Models.Users;

public class UserModel
{
    public int? Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;
    public string Email { get; set; } = default!;
}