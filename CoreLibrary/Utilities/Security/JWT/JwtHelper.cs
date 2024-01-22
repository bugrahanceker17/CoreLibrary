using System.IdentityModel.Tokens.Jwt;
using CoreLibrary.Models.Concrete.Entities;
using CoreLibrary.Models.Setting;
using CoreLibrary.Utilities.Security.Claim;
using CoreLibrary.Utilities.Security.Encryption;
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

        public AccessToken CreateToken(AppUser appUser, List<string> roles)
        {
            SecurityKey securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            SigningCredentials signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            JwtSecurityToken jwt = CreateJwtSecurityToken(_tokenOptions, appUser, signingCredentials, roles);
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            string token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = DateTimeOffset.Now.AddMinutes(_tokenOptions.AccessTokenExpiration)
            };
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
    }
}