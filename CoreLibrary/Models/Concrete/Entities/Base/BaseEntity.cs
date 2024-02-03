namespace CoreLibrary.Models.Concrete.Entities.Base;

public class BaseEntity<TIdentityType> : EntityIdentityBase<TIdentityType>
{
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsStatus { get; set; }
    public bool IsDeleted { get; set; }
}