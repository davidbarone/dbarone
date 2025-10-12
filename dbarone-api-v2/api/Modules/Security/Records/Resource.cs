/// <summary>
/// Defines a resource in the security RBAC framework.
/// </summary>
/// <param name="Id"></param>
/// <param name="Code"></param>
/// <param name="Description"></param>
public record Resource(
    int Id,
    string ResourceName
) : Identifiable
{ }