using CoreLibrary.Models.Concrete.Entities.Auth;

namespace CoreLibrary.Utilities.DataAccess.Operation.Dapper.Abstract;

public interface IAuthOperation
{
    Task<(bool isSuccess, string message)> Register(RegisterRequest request);
    Task<(bool isSuccess, string error, string accessToken)> LogIn(LoginRequest request);
}