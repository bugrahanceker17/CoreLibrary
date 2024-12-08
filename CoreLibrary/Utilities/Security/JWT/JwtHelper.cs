using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using CoreLibrary.Models.Concrete.Entities;
using CoreLibrary.Models.Setting;
using CoreLibrary.Utilities.DataAccess.Operation.EntityFramework.Abstract;
using CoreLibrary.Utilities.Security.Claim;
using CoreLibrary.Utilities.Security.Encryption;
using CoreLibrary.Utilities.Security.Hashing;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CoreLibrary.Utilities.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        private readonly ConfigurationValues _configurationValues;
        private readonly TokenOptions _tokenOptions;
        private readonly IEfDynamicBaseQuery _efDynamicBaseQuery;

        public JwtHelper(IOptions<ConfigurationValues> configurationValues, IEfDynamicBaseQuery efDynamicBaseQuery)
        {
            _efDynamicBaseQuery = efDynamicBaseQuery;
            _configurationValues = configurationValues.Value;
            _tokenOptions = _configurationValues.TokenOptions;
        }

        public AccessToken CreateAccessToken(AppUser appUser, List<string> roles)
        {
            SecurityKey securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            SigningCredentials signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            JwtSecurityToken jwt = CreateJwtSecurityToken(_tokenOptions, appUser, signingCredentials, roles);
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            string token = jwtSecurityTokenHandler.WriteToken(jwt);

            var sharedData = _efDynamicBaseQuery.GetByExpressionAsync<AppSharedSetting>(c => c.Type == 1 && c.IsDeleted == false && c.IsStatus).Result;

            int expirationValue = sharedData != null ? Convert.ToInt32(sharedData.ExtraValue) : _tokenOptions.AccessTokenExpiration;

            return new AccessToken
            {
                Token = token,
                Expiration = DateTimeOffset.Now.AddMinutes(expirationValue)
            };
        }

        private JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, AppUser appUser, SigningCredentials signingCredentials, List<string> roles)
        {
            var sharedData = _efDynamicBaseQuery.GetByExpressionAsync<AppSharedSetting>(c => c.Type == 1 && c.IsDeleted == false && c.IsStatus).Result;

            int expirationValue = sharedData != null ? Convert.ToInt32(sharedData.ExtraValue) : _tokenOptions.AccessTokenExpiration;
            
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: DateTimeOffset.Now.LocalDateTime.AddMinutes(expirationValue),
                notBefore: DateTimeOffset.Now.LocalDateTime,
                claims: SetClaims(appUser, roles),
                signingCredentials: signingCredentials
            );
            
            return jwt;
        }

        private static IEnumerable<System.Security.Claims.Claim> SetClaims(AppUser appUser, List<string> roles)
        {
            List<System.Security.Claims.Claim> claims = new();
            
            claims.AddNameIdentifier(appUser.Id.ToString());

            claims.AddUserName(appUser.UserName ?? appUser.Email);
            
            claims.AddRoles(roles.ToArray());
            
            return claims;
        }
        
        private static string GenerateSecureRefreshToken()
        {
            byte[] randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
        
        public RefreshToken CreateRefreshToken(int minutesValid = 45)
        {
            var rawToken = GenerateSecureRefreshToken();

            var salt = HashingHelper.GenerateSalt();

            string tokenHash = HashingHelper.HashWithSalt(rawToken, salt);

            return new RefreshToken
            {
                TokenSalt = salt,
                TokenHash = tokenHash,
                BaseRefreshToken = rawToken,
                Expires = DateTimeOffset.UtcNow.AddMinutes(minutesValid)
            };
        }
    }
}