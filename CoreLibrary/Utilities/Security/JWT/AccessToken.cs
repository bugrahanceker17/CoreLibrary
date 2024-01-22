using System;

namespace CoreLibrary.Utilities.Security.JWT
{
    public class AccessToken
    {
        public string Token { get; set; }
        public DateTimeOffset Expiration { get; set; }
    }
}