using CoreLibrary.Models.Abstract;

namespace CoreLibrary.Models.Concrete.Entities.Base;

public class EntityIdentityBase<TIdentityType> : IEntity
{
    public TIdentityType Id { get; set; }
}