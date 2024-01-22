using CoreLibrary.Models.Concrete.Entities;

namespace CoreLibrary.Utilities.Security.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(AppUser user, List<string> roles); 
    }
}

