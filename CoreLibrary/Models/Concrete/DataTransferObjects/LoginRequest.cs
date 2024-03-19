using CoreLibrary.Models.Abstract;

namespace CoreLibrary.Models.Concrete.DataTransferObjects;

public class LoginRequest : IRequest
{
    public string Value { get; set; }
    public string Password { get; set; }
}