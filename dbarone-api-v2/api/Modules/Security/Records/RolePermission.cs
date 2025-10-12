/// <summary>
/// Maps roles to resource/permission combinations.
/// </summary>
/// <param name="Id"></param>
/// <param name="Code"></param>
/// <param name="Description"></param>
public record RolePermission(
    int Id,
    string RoleName,
    string ResourceName,
    char PermissionCode
) : Identifiable
{ }