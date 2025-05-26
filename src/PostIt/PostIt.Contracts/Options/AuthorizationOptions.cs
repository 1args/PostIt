namespace PostIt.Contracts.Options;

public class AuthorizationOptions
{
    public RolePermission[] Permissions { get; set; } = [];
}

public class RolePermission
{
    public string Role { get; set; } = string.Empty;
    
    public string[] Permissions { get; set; } = [];
}