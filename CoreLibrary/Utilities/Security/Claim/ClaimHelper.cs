using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace CoreLibrary.Utilities.Security.Claim
{
    public static class ClaimHelper
    {
        public static void AddEmail(this ICollection<System.Security.Claims.Claim> claims, string email)
        {
            claims.Add(new System.Security.Claims.Claim(JwtRegisteredClaimNames.Email, email));
        }
        
        public static void AddUniqueName(this ICollection<System.Security.Claims.Claim> claims, string uniqueName)
        {
            claims.Add(new System.Security.Claims.Claim(JwtRegisteredClaimNames.UniqueName, uniqueName ));
        }

        public static void AddPhoneNumber(this ICollection<System.Security.Claims.Claim> claims, string phoneNumber)
        {
            claims.Add(new System.Security.Claims.Claim(JwtRegisteredClaimNames.Prn, phoneNumber));
        }

        public static void AddNameIdentifier(this ICollection<System.Security.Claims.Claim> claims, string nameIdentifier)
        {
            claims.Add(new System.Security.Claims.Claim(JwtRegisteredClaimNames.NameId, nameIdentifier));
        }
        
        public static void AddUserName(this ICollection<System.Security.Claims.Claim> claims, string userName)
        {
            claims.Add(new System.Security.Claims.Claim(JwtRegisteredClaimNames.Acr, userName));
        }

        public static void AddRoles(this ICollection<System.Security.Claims.Claim> claims, string[] roles)
        {
            roles.ToList().ForEach(role => claims.Add(new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, role)));
        }
    }
}