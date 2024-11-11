namespace CoreLibrary.Utilities.Security.JWT;

public class RefreshToken
{
    public string BaseRefreshToken { get; set; }
    public string TokenHash { get; set; }
    public string TokenSalt { get; set; }
    public DateTimeOffset Expires { get; set; }
    public long ExpiresTimeStamp => Expires.ToUnixTimeSeconds();
    public bool IsExpired => DateTimeOffset.UtcNow >= Expires;
}