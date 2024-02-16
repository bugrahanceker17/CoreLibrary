namespace CoreLibrary.Utilities.Attribute;

public class PermissionControlAttribute : System.Attribute
{
    public bool MustLogin { get; set; }
    public string[]? Permissions { get; set; }

    public PermissionControlAttribute(bool mustLogin, params string[] permissions)
    {
        Permissions = permissions;
        MustLogin = mustLogin;
    }

    public PermissionControlAttribute(bool mustLogin)
    {
        MustLogin = mustLogin;
    }
}