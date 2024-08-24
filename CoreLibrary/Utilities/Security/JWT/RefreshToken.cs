namespace CoreLibrary.Utilities.Security.JWT;

public class RefreshToken
{
    public string BaseRefreshToken { get; set; }
    public string TokenHash { get; set; }
    public string TokenSalt { get; set; }
    public DateTime Expires { get; set; }
    public long ExpiresTimeStamp => ((DateTimeOffset)Expires).ToUnixTimeSeconds();
    public bool IsExpired => DateTime.UtcNow >= Expires;
}