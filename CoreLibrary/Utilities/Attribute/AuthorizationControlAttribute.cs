namespace CoreLibrary.Utilities.Attribute;

public class AuthorizationControlAttribute : System.Attribute
{
    public bool MustLogin { get; set; }
    public string[]? Permissions { get; set; }

    public AuthorizationControlAttribute(bool mustLogin, params string[] permissions)
    {
        Permissions = permissions;
        MustLogin = mustLogin;
    }

    public AuthorizationControlAttribute(bool mustLogin)
    {
        MustLogin = mustLogin;
    }
}