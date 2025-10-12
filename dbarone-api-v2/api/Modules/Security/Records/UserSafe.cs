public record UserSafe(
    int Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    bool IsEnabled,
    DateTime CreatedDt,
    string CreatedBy,
    DateTime UpdatedDt,
    string UpdatedBy
    ) : Identifiable
{
}