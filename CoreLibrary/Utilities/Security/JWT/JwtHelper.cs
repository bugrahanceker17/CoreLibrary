using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using CoreLibrary.Models.Concrete.Entities;
using CoreLibrary.Models.Setting;
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

        public JwtHelper(IOptions<ConfigurationValues> configurationValues)
        {
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

            return new AccessToken
            {
                Token = token,
                Expiration = DateTimeOffset.Now.AddMinutes(_tokenOptions.AccessTokenExpiration),
                RefreshToken = GenerateRefreshToken()
            };
        }

        public string CreateRefreshToken(string token)
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] randomBytes = new byte[32];
            rng.GetBytes(randomBytes);
            
            return Convert.ToBase64String(randomBytes);
        }

        private JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, AppUser appUser, SigningCredentials signingCredentials, List<string> roles)
        {
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: DateTimeOffset.Now.LocalDateTime.AddMinutes(_tokenOptions.AccessTokenExpiration),
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

            claims.AddUserName(appUser.UserName);
            
            claims.AddRoles(roles.ToArray());
            
            return claims;
        }

        private RefreshToken GenerateRefreshToken(int daysValid = 15)
        {
            var refreshToken = new RefreshToken
            {
                TokenSalt = HashingHelper.GenerateSalt(),
                Expires = DateTime.UtcNow.AddDays(daysValid)
            };

            string token = GenerateSecureToken();
            refreshToken.BaseRefreshToken = token;
            refreshToken.TokenHash = HashingHelper.HashWithSalt(token, refreshToken.TokenSalt);

            return refreshToken;
        }
        
        private string GenerateSecureToken()
        {
            byte[] randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
        
        public async Task<AppUser> RenewRefreshTokenAsync(AppUser user)
        {
            var refToken = GenerateRefreshToken();
            
            user.RefreshTokenExpires = refToken.Expires;
            user.RefreshTokenHash = refToken.TokenHash;
            user.RefreshTokenSalt = refToken.TokenSalt;

            return user;
        }
    }
}