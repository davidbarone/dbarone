public record UserGroupMembership(
    int Id,
    int UserId,
    int GroupId
) : Identifiable
{ }