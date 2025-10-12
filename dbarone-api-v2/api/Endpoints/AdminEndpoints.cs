using Dbarone.Net.JsonDataStore;

public static class AdminEndpoints
{
    public static void RegisterAdminEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("/init", (IDataStore dataStore) =>
        {
            AdminEndpoints.Init(dataStore);
            return Results.Ok("Init() successful.");
        })
        .WithOpenApi()
        .WithTags("Admin")
        .WithDescription("Initialises the system with default data.");
    }

    private static void Init(IDataStore dataStore)
    {
        var store = (DataStore)dataStore.GetContext(true);
        var t = store.BeginTransaction();
        t.GetCollection<Article>();
        t.AddUniqueConstraint<Article>(p => p.Id);
        t.GetCollection<Blob>();
        t.AddUniqueConstraint<Blob>(p => p.Id);

        // User table
        var users = t.GetCollection<User>();
        t.AddUniqueConstraint<User>(p => p.Id);
        users.Insert(new[] {
            new User(
                t.Next<User>(),
                "Administrator",
                "Administrator",
                "admin",
                BCrypt.Net.BCrypt.HashPassword("password"),
                "",
                true,
                DateTime.Now,
                "system",
                DateTime.Now,
                "system")
        });

        // Group table
        var groups = t.GetCollection<Group>();
        t.AddUniqueConstraint<Group>(p => p.Id);
        groups.Insert(new[] {
            new Group(t.Next<Group>(), "Administrators"),
            new Group(t.Next<Group>(), "Contributors"),
            new Group(t.Next<Group>(), "Viewers")
        });

        // Permission table
        var permissions = t.GetCollection<Permission>();
        t.AddUniqueConstraint<Permission>(p => p.Id);
        permissions.Insert(new[] {
            new Permission(t.Next<Permission>(), 'r', "Read"),      // GET
            new Permission(t.Next<Permission>(), 'w', "Write"),     // POST, PUT, PATCH
            new Permission(t.Next<Permission>(), 'x', "Execute"),
            new Permission(t.Next<Permission>(), 'd', "Delete"),    // DELETE
            new Permission(t.Next<Permission>(), 'a', "Admin")
        });

        // Resource table
        var resources = t.GetCollection<Resource>();
        t.AddUniqueConstraint<Resource>(p => p.Id);
        resources.Insert(new[] {
            new Resource(t.Next<Resource>(), "*"),
            new Resource(t.Next<Resource>(), "Articles"),
            new Resource(t.Next<Resource>(), "Blobs"),
            new Resource(t.Next<Resource>(), "Users"),
            new Resource(t.Next<Resource>(), "Roles"),
            new Resource(t.Next<Resource>(), "Resources"),
            new Resource(t.Next<Resource>(), "Permissions"),
        });

        // Role table - define the roles available. Can be work roles
        var roles = t.GetCollection<Role>();
        t.AddUniqueConstraint<Role>(p => p.Id);
        roles.Insert(new[] {
            new Role(t.Next<Role>(), "Administrator"), // Full permissions
            new Role(t.Next<Role>(), "Contributor"),   // Can manage content, but not security
            new Role(t.Next<Role>(), "Viewer")         // Read only content as an authorised viewer 
        });

        // Role Assignments table - maps roles to groups or users
        var roleAssignments = t.GetCollection<RoleAssignment>();
        t.AddUniqueConstraint<RoleAssignment>(p => p.Id);
        roleAssignments.Insert(new[] {
            new RoleAssignment(t.Next<RoleAssignment>(), "Administrator", 'g', "Administrators"),
            new RoleAssignment(t.Next<RoleAssignment>(), "Contributor", 'g', "Contributors"),
            new RoleAssignment(t.Next<RoleAssignment>(), "Viewer", 'g', "Viewers")
        });

        // Role Permissions table
        var rolePermissions = t.GetCollection<RolePermission>();
        t.AddUniqueConstraint<RolePermission>(p => p.Id);
        rolePermissions.Insert(new[] {
            new RolePermission(t.Next<RolePermission>(), "Viewer", "*", 'r'),
            new RolePermission(t.Next<RolePermission>(), "Contributor", "*", 'r'),
            new RolePermission(t.Next<RolePermission>(), "Contributor", "*", 'w'),
            new RolePermission(t.Next<RolePermission>(), "Administrator", "*", 'a')
        });

        // Role Permissions table
        var userGroupMemberships = t.GetCollection<UserGroupMembership>();
        t.AddUniqueConstraint<UserGroupMembership>(p => p.Id);
        userGroupMemberships.Insert(new[] {
            // Add user 1 (admin) into group 1 (Administrators)
            new UserGroupMembership(t.Next<UserGroupMembership>(), 1, 1)
        });

        t.Commit();
    }

}