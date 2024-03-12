using CoreLibrary.Extensions;
using CoreLibrary.Models.Concrete.Entities;
using CoreLibrary.Models.Concrete.Entities.Auth;
using CoreLibrary.Utilities.DataAccess.Operation.Abstract;
using CoreLibrary.Utilities.DataAccess.Operation.EntityFramework.Abstract;
using CoreLibrary.Utilities.Security.Hashing;
using CoreLibrary.Utilities.Security.JWT;

namespace CoreLibrary.Utilities.DataAccess.Operation.EntityFramework.Concrete;

public class EfAuthOperation : IEfAuthOperation
{
    private readonly IEfDynamicBaseQuery _dynamicBaseQuery;
    private readonly IEfDynamicBaseCommand _dynamicBaseCommand;
    private readonly ITokenHelper _tokenHelper;
    
    public EfAuthOperation(IEfDynamicBaseQuery dynamicBaseQuery, IEfDynamicBaseCommand dynamicBaseCommand, ITokenHelper tokenHelper)
    {
        _dynamicBaseQuery = dynamicBaseQuery;
        _dynamicBaseCommand = dynamicBaseCommand;
        _tokenHelper = tokenHelper;
    }
    
    public async Task<(bool isSuccess, string message)> Register(RegisterRequest request)
    {
        AppUser userExists = await _dynamicBaseQuery.GetByExpressionAsync<AppUser>(c => 
            c.UserName == request.UserName || 
            c.Email == request.Email || 
            c.PhoneNumber == request.PhoneNumber
            );

        if (userExists is not null && userExists.Id != Guid.Empty)
            return (false, "");
        
        byte[] passwordHash, passwordSalt;
        HashingHelper.CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);

        AppUser user = new AppUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            EmailConfirmed = false,
            UserName = request.UserName,
            PhoneNumber = request.PhoneNumber,
            PhoneNumberConfirmed = false,
            CreatedAt = DateTime.Now,
            IsDeleted = false,
            IsStatus = false,

            PasswordSalt = passwordSalt,
            PasswordHash = passwordHash
        };

        (bool succeeded, Guid id) insertUser = await _dynamicBaseCommand.AddWithGuidIdentityAsync(user);

        if (!insertUser.succeeded)
            return (false, "");

        if (!request.RoleId.GuidIsNullOrEmpty())
        {
            AppUserRole userRole = new AppUserRole
            {
                RoleId = request.RoleId.Value,
                UserId = insertUser.id
            };
            
            (bool succeeded, Guid id) insertUserRole = await _dynamicBaseCommand.AddWithGuidIdentityAsync(userRole);

            if (!insertUserRole.succeeded)
                return (false, "");
            
        }

        return (true, "");
    }

    public async Task<(bool isSuccess, string error, string accessToken)> LogIn(LoginRequest request)
    {
        AppUser user = await _dynamicBaseQuery.GetByExpressionAsync<AppUser>(c => c.UserName.Equals(request.Value) || c.Email.Equals(request.Value));

        if (user is null)
            return (false, "", "");

        List<AppUserRole>? userRole = await _dynamicBaseQuery.GetAllByExpressionAsync<AppUserRole>(c => c.UserId == user.Id);
        
        if (!userRole.Any())
            return (false, "", "");
        
        if (!HashingHelper.VerifyPasswordHash(request.Password, user!.PasswordHash, user.PasswordSalt))
            return (false, "", "");
        
        AccessToken token = _tokenHelper.CreateToken(user, userRole.Select(c=>c.RoleId.ToString()).ToList());
        
        if(string.IsNullOrEmpty(token.Token))
            return (false, "", "");

        await _dynamicBaseCommand.AddWithGuidIdentityAsync(new AppLoginLog
        {
            Description = $"User [{user.FirstName} {user.LastName}] logged in on [{DateTime.Now:dd/MM/yyyy HH:mm:ss}]"
        });

        return (true, "", token.Token);
    }
}