using System.Data;
using CoreLibrary.Models.Concrete.Entities.Base;

namespace CoreLibrary.Utilities.DataAccess.Operation.Abstract;

public interface IDynamicBaseCommand
{
    Task<(bool succeeded, Guid id)> AddWithGuidIdentityAsync<T>(T entity) where T : BaseEntity<Guid>;
    Task<object?> AddInTransaction<T>(T entity, IDbTransaction transaction);
    Task<(bool succeeded, TIdentityType? id)> AddAsync<T, TIdentityType>(T entity) where T : BaseEntity<TIdentityType>;
    Task<int> UpdateAsync<T>(T entity) where T : BaseEntity<Guid>;
    Task UpdateInTransaction<T>(T entity, IDbTransaction transaction);
    Task<int> HardDeleteAsync<T>(T entity) where T : class;
    Task<int> Passive<T>(T entity) where T : BaseEntity<Guid>;
    IDbTransaction BeginTransaction();
    void CommitTransaction(IDbTransaction transaction);
    void RollbackTransaction(IDbTransaction transaction);
    void CloseConnection();
}