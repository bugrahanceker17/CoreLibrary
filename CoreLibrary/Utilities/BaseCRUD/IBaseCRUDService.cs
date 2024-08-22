using CoreLibrary.Models.Abstract;
using CoreLibrary.Models.Concrete.Entities.Base;
using CoreLibrary.Utilities.Result;

namespace CoreLibrary.Utilities.BaseCRUD;

public interface IBaseCRUDService<T> where T : BaseEntity<Guid>, IEntity
{
    Task<int> Create<T>(object createRequest) where T : BaseEntity<Guid>, IEntity, new();
    Task<int> Update<T>(object updateRequest) where T : BaseEntity<Guid>, IEntity, new();
    Task<int> Passive<T>(Guid id) where T : BaseEntity<Guid>, IEntity, new();
    Task<T> GetById<T>(Guid id) where T : BaseEntity<Guid>, IEntity, new();
}