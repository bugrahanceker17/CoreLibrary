using CoreLibrary.Models.Concrete.Entities;

namespace CoreLibrary.Utilities.Security.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateAccessToken(AppUser user, List<string> roles);
        string CreateRefreshToken(string token);
    }
}

