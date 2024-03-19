using CoreLibrary.Models.Abstract;

namespace CoreLibrary.Models.Concrete.DataTransferObjects;

public class RegisterResponse : IResponse
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
}