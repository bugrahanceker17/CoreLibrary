namespace CoreLibrary.Models.Concrete.Entities;

public class EntityIdentityBase<TIdentityType>
{
    public TIdentityType Id { get; set; }
}