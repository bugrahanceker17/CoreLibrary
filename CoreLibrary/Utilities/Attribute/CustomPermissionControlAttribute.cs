namespace CoreLibrary.Utilities.Attribute;

public class CustomPermissionControlAttribute : System.Attribute
{
    public bool MustLogin { get; set; }
    public string[]? Permissions { get; set; }

    public CustomPermissionControlAttribute(bool mustLogin, params string[] permissions)
    {
        Permissions = permissions;
        MustLogin = mustLogin;
    }

    public CustomPermissionControlAttribute(bool mustLogin)
    {
        MustLogin = mustLogin;
    }
}