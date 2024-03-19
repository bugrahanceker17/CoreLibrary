using CoreLibrary.Models.Concrete.DataTransferObjects;

namespace CoreLibrary.Utilities.DataAccess.Operation.Abstract;

public interface IAuthOperation
{
    Task<RegisterResponse> Register(RegisterRequest request);
    Task<LoginResponse> LogIn(LoginRequest request);
    Task<(bool isSuccess, string message)> UpdatePassword(UpdatePasswordRequest request);
}