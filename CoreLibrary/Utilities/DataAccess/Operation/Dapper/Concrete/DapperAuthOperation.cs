﻿using CoreLibrary.Extensions;
using CoreLibrary.Models.Concrete.Entities;
using CoreLibrary.Models.Concrete.DataTransferObjects;
using CoreLibrary.Utilities.DataAccess.Operation.Dapper.Abstract;
using CoreLibrary.Utilities.Security.Hashing;
using CoreLibrary.Utilities.Security.JWT;
using Microsoft.AspNetCore.Http;

namespace CoreLibrary.Utilities.DataAccess.Operation.Dapper.Concrete;

public class DapperAuthOperation : IDapperAuthOperation
{
    private readonly IDapperDynamicBaseQuery _dynamicBaseQuery;
    private readonly IDapperDynamicBaseCommand _dynamicBaseCommand;
    private readonly ITokenHelper _tokenHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public DapperAuthOperation(IDapperDynamicBaseQuery dynamicBaseQuery, IDapperDynamicBaseCommand dynamicBaseCommand, ITokenHelper tokenHelper, IHttpContextAccessor httpContextAccessor)
    {
        _dynamicBaseQuery = dynamicBaseQuery;
        _dynamicBaseCommand = dynamicBaseCommand;
        _tokenHelper = tokenHelper;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<RegisterResponse> Register(RegisterRequest request)
    {
        AppUser userExists = await _dynamicBaseQuery.GetByExpressionAsync<AppUser>(c => 
            c.UserName == request.UserName || 
            c.Email == request.Email || 
            c.PhoneNumber == request.PhoneNumber
            );

        if (userExists is not null && userExists.Id != Guid.Empty)
            return new RegisterResponse { IsSuccess = false };
        
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
            CreatedAt = DateTimeOffset.Now,
            IsDeleted = false,
            IsStatus = false,

            PasswordSalt = passwordSalt,
            PasswordHash = passwordHash
        };

        (bool succeeded, Guid id) insertUser = await _dynamicBaseCommand.AddWithGuidIdentityAsync(user);

        if (!insertUser.succeeded)
            return new RegisterResponse { IsSuccess = false };

        if (!request.RoleId.GuidIsNullOrEmpty())
        {
            AppUserRole userRole = new AppUserRole
            {
                RoleId = request.RoleId.Value,
                UserId = insertUser.id
            };
            
            (bool succeeded, Guid id) insertUserRole = await _dynamicBaseCommand.AddWithGuidIdentityAsync(userRole);

            if (!insertUserRole.succeeded)
                return new RegisterResponse { IsSuccess = false };
            
        }

        return new RegisterResponse { IsSuccess = true };
    }

    public async Task<LoginResponse> LogIn(LoginRequest request)
    {
        AppUser user = await _dynamicBaseQuery.GetByExpressionAsync<AppUser>(c => c.UserName.Equals(request.Value) || c.Email.Equals(request.Value));

        if (user is null)
            return new LoginResponse { IsSuccess = false };

        List<AppUserRole>? userRole = await _dynamicBaseQuery.GetAllByExpressionAsync<AppUserRole>(c => c.UserId == user.Id);
        
        if (!userRole.Any())
            return new LoginResponse { IsSuccess = false };
        
        if (!HashingHelper.VerifyPasswordHash(request.Password, user!.PasswordHash, user.PasswordSalt))
            return new LoginResponse { IsSuccess = false };
        
        AccessToken token = _tokenHelper.CreateAccessToken(user, userRole.Select(c=>c.RoleId.ToString()).ToList());
        
        if(string.IsNullOrEmpty(token.Token))
            return new LoginResponse { IsSuccess = false };

        await _dynamicBaseCommand.AddWithGuidIdentityAsync(new AppLoginLog
        {
            Description = $"User [{user.FirstName} {user.LastName}] logged in on [{DateTimeOffset.Now:dd/MM/yyyy HH:mm:ss}]"
        });

        return new LoginResponse { IsSuccess = true, AccessToken = token.Token, User = user};
    }

    public async Task<(bool isSuccess, string message)> UpdatePassword(UpdatePasswordRequest request)
    {
        AppUser? appUser = await _dynamicBaseQuery.GetAsync<AppUser>(request.UserId);

        if (appUser is null)
            return (false, "");

        if (!request.OldPassword.Equals(request.OldPasswordCheck))
            return (false, "");

        if (!HashingHelper.VerifyPasswordHash(request.OldPassword, appUser.PasswordHash, appUser.PasswordSalt))
            return (false, "");
        
        byte[] passwordHash, passwordSalt;
        HashingHelper.CreatePasswordHash(request.NewPassword, out passwordHash, out passwordSalt);


        appUser.PasswordSalt = passwordSalt;
        appUser.PasswordHash = passwordHash;

        int result = await _dynamicBaseCommand.UpdateAsync(appUser);

        if (result.LessOrEqualToZero())
            return (false, "");
        
        return (true, "");
    }

    public bool LoginExists()
    {
        return !string.IsNullOrEmpty(_httpContextAccessor.AccessToken().accessToken);
    }

    public Guid UserId()
    {
        return string.IsNullOrEmpty(_httpContextAccessor.AccessToken().userId) ? Guid.Empty : new Guid(_httpContextAccessor.AccessToken().userId);
    }
}