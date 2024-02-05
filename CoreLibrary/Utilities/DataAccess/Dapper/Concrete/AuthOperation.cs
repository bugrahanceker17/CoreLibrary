using System.Data;
using CoreLibrary.Models.Concrete.Entities;
using CoreLibrary.Models.Concrete.Entities.Auth;
using CoreLibrary.Models.Setting;
using CoreLibrary.Utilities.DataAccess.Dapper.Abstract;
using CoreLibrary.Utilities.Security.Hashing;
using CoreLibrary.Utilities.Security.JWT;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace CoreLibrary.Utilities.DataAccess.Dapper.Concrete;

public class AuthOperation : IAuthOperation
{
    private readonly IDbConnection db;
    private readonly IDynamicQuery _dynamicQuery;
    private readonly IDynamicCommand _dynamicCommand;
    private readonly ITokenHelper _tokenHelper;
    
    public AuthOperation(IOptions<ConfigurationValues> configuration, IDynamicQuery dynamicQuery, IDynamicCommand dynamicCommand, ITokenHelper tokenHelper)
    {
        _dynamicQuery = dynamicQuery;
        _dynamicCommand = dynamicCommand;
        _tokenHelper = tokenHelper;
        ConfigurationValues configurationValues = configuration.Value;
        db = new SqlConnection(configurationValues.Database.ConnectionString);
    }
    
    public async Task<(bool isSuccess, string message)> Register(RegisterRequest request)
    {
        AppUser userExists = await _dynamicQuery.GetByExpressionAsync<AppUser>(c => 
            c.UserName == request.UserName || 
            c.Email == request.Email || 
            c.PhoneNumber == request.PhoneNumber
            );

        if (userExists is not null)
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

        (bool succeeded, Guid userId) insertUser = await _dynamicCommand.AddWithGuidIdentityAsync(user);

        if (!insertUser.succeeded)
            return (false, "");

        if (request.RoleId != null && request.RoleId != Guid.Empty)
        {
            AppUserRole userRole = new AppUserRole()
            {
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                IsStatus = true,
                RoleId = request.RoleId.Value,
                UserId = insertUser.userId
            };
            
            (bool succeeded, Guid userId) insertUserRole = await _dynamicCommand.AddWithGuidIdentityAsync(userRole);

            if (!insertUserRole.succeeded)
                return (false, "");
            
        }

        return (true, "");
    }

    public async Task<(bool isSuccess, string error, string accessToken)> LogIn(LoginRequest request)
    {
        AppUser user = await _dynamicQuery.GetByExpressionAsync<AppUser>(c => c.UserName.Equals(request.Value) || c.Email.Equals(request.Value));

        if (user is not null)
            return (false, "", "");

        List<AppUserRole>? userRole = await _dynamicQuery.GetAllByExpressionAsync<AppUserRole>(c => c.UserId.Equals(user.Id));
        
        if (!userRole.Any())
            return (false, "", "");
        
        if (!HashingHelper.VerifyPasswordHash(request.Password, user!.PasswordHash, user.PasswordSalt))
            return (false, "", "");
        
        AccessToken token = _tokenHelper.CreateToken(user, userRole.Select(c=>c.RoleId.ToString()).ToList());
        
        if(string.IsNullOrEmpty(token.Token))
            return (false, "", "");

        return (true, "", token.Token);
    }
}