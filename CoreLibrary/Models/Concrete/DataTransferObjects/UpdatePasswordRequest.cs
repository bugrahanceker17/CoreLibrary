using CoreLibrary.Models.Abstract;

namespace CoreLibrary.Models.Concrete.DataTransferObjects;

public class UpdatePasswordRequest : IRequest
{
    public Guid UserId { get; set; }
    public string OldPassword { get; set; }
    public string OldPasswordCheck { get; set; }
    public string NewPassword { get; set; }
}