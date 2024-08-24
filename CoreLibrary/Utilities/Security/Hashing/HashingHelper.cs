using System.Security.Cryptography;
using System.Text;
using CoreLibrary.Utilities.Security.JWT;

namespace CoreLibrary.Utilities.Security.Hashing
{
    public static class HashingHelper
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }

        }
        
        public static string HashRefreshToken(string refreshToken)
        {
            using var sha256 = SHA256.Create();
            
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(refreshToken));
            return Convert.ToBase64String(bytes);
        }
        
        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
        
        public static bool ValidateRefreshToken(string providedToken, RefreshToken storedToken)
        {
            string providedTokenHash = HashWithSalt(providedToken, storedToken.TokenSalt);

            return providedTokenHash == storedToken.TokenHash;
        }

        public static string HashWithSalt(string refreshToken, string salt)
        {
            using var sha256 = SHA256.Create();
            var saltedToken = refreshToken + salt;
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedToken));
            return Convert.ToBase64String(bytes);
        }
    } 
}

