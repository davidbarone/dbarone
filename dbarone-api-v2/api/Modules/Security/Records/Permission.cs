public record Permission(
    int Id,
    char PermissionCode,
    string PermissionName
) : Identifiable
{ }