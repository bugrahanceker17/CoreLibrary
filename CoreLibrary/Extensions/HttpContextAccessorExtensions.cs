using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace CoreLibrary.Extensions
{
    public static class HttpContextAccessorExtensions
    {
        public static (bool login, string text, string userId) LoginExists(this IHttpContextAccessor httpContextAccessor, params string[] role)
        {
           (string accessToken, string userId, List<Claim> claims, string role, long expireTime) accessTokenData = httpContextAccessor.AccessToken();

           if (string.IsNullOrEmpty(accessTokenData.userId))
                return (false, "Giriş yapılmadı", accessTokenData.userId);

           if (role.Any() && !role.Contains(accessTokenData.role))
                return (false, "Bu işlem için yetkiniz bulunmamaktadır", accessTokenData.userId);

           return (true, string.Empty, accessTokenData.userId);
        }
        
        public static (bool login, string text, string userId) LoginExistsWithExpireTime(this IHttpContextAccessor httpContextAccessor, List<string> role)
        {
           (string accessToken, string userId, List<Claim> claims, string role, long expireTime) accessTokenData = httpContextAccessor.AccessToken();
            
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(accessTokenData.expireTime);

            if (string.IsNullOrEmpty(accessTokenData.userId))
                return (false, "Giriş yapılmadı", accessTokenData.userId);
            
            if (DateTimeOffset.Now.LocalDateTime >= dateTimeOffset.DateTime.ToLocalTime())
                return (false, "Süreniz doldu. Tekrar giriş yapınız", accessTokenData.userId);

            if (!role.Contains(accessTokenData.role))
                return (false, "Bu işlem için yetkiniz bulunmamaktadır", accessTokenData.userId);

            return (true, string.Empty, accessTokenData.userId);
        }

        public static (string accessToken, string userId, List<Claim> claims, string role, long expireTime) AccessToken(this IHttpContextAccessor httpContextAccessor)
        {
            HttpContext httpContext = httpContextAccessor.HttpContext;
            HttpRequest request = httpContext?.Request;

            string accessToken = request?.Headers["Authorization"].FirstOrDefault();
            string idToken = request?.Headers["IdToken"].FirstOrDefault();

            if (string.IsNullOrEmpty(accessToken))
                if (request.Query.TryGetValue("access_token", out StringValues token))
                    accessToken = $"Bearer {token}";

            string userId = "";
            string role = "";
            long expTimeFormatTimeStamp = 0;
            
            List<Claim> claims = null;
            
            if (!string.IsNullOrEmpty(accessToken))
            {
                if (accessToken.StartsWith("Bearer")) accessToken = accessToken.Replace("Bearer ", "").Replace("\"", "").Trim();

                JwtSecurityToken securityToken = new JwtSecurityToken(accessToken);

                if (securityToken.Claims.Any())
                {
                    List<Claim> claim = securityToken.Claims.ToList();
                    userId = claim.FirstOrDefault()?.Value;
                    role = claim[2].Value;
                    
                    bool success = long.TryParse(claim[4].Value, out long number);
                    
                    if (success)
                        expTimeFormatTimeStamp = number;
                    
                }
            }

            if (string.IsNullOrEmpty(idToken)) return (accessToken, userId, claims, role, expTimeFormatTimeStamp);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.CanReadToken(idToken)) claims = tokenHandler.ReadJwtToken(idToken).Claims.ToList();

            return (accessToken, userId, claims, role, expTimeFormatTimeStamp);
        }

        public static string RefreshToken()
        {
            return "";
        }
    }
}

