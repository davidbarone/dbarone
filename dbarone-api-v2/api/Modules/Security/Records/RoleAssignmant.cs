public record RoleAssignment(
    int Id,
    string RoleName,
    char AssignmentType,     // 'g' or 'u'
    string AssignmentName   //GroupName or UserName
) : Identifiable
{ }