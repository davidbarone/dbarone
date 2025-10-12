public record User(
    int Id,
    string FirstName,
    string LastName,
    string UserName,
    string PasswordHash,
    string Email,
    bool IsEnabled,
    DateTime CreatedDt,
    string CreatedBy,
    DateTime UpdatedDt,
    string UpdatedBy
    ) : Identifiable
{
}