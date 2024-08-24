using CoreLibrary.Models.Abstract;
using CoreLibrary.Models.Concrete.Entities;
using CoreLibrary.Utilities.Security.JWT;

namespace CoreLibrary.Models.Concrete.DataTransferObjects;

public class LoginResponse : IResponse
{
    public bool IsSuccess { get; set; }
    public string? AccessToken { get; set; }
    public RefreshToken RefreshToken { get; set; }
    public string? Message { get; set; }
    public AppUser User { get; set; } = new();
}