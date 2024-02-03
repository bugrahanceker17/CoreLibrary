using System.Data;
using CoreLibrary.Extensions;
using CoreLibrary.Models.Concrete.Entities.Base;
using CoreLibrary.Models.Setting;
using CoreLibrary.Utilities.DataAccess.Dapper.Abstract;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace CoreLibrary.Utilities.DataAccess.Dapper.Concrete;

public class DynamicCommand : IDynamicCommand
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDbConnection db;

    public DynamicCommand(IOptions<ConfigurationValues> configuration, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        ConfigurationValues configurationValues = configuration.Value;
        db = new SqlConnection(configurationValues.Database.ConnectionString);
    }

    public async Task<(bool succeeded, Guid userId)> AddWithGuidIdentityAsync<T>(T entity) where T : BaseEntity<Guid>
    {
        string userId = _httpContextAccessor.AccessToken().userId;

        entity.CreatedBy = string.IsNullOrEmpty(userId) ? null : userId;
        entity.CreatedAt = DateTime.Now;
        entity.IsDeleted = false;
        entity.IsStatus = true;

        Guid result = await db.InsertAsync<Guid, T>(entity);

        if (result != Guid.Empty)
            return (true, new Guid(result.ToString()));

        return (false, Guid.Empty);
    }

    public async Task<object?> AddInTransaction<T>(T entity, IDbTransaction transaction)
    {
        return await db.InsertAsync<Guid, T>(entity, transaction);
    }

    public async Task<(bool succeeded, TIdentityType userId)> AddAsync<T, TIdentityType>(T entity) where T : BaseEntity<TIdentityType>
    {
        TIdentityType result = await db.InsertAsync<TIdentityType, T>(entity);

        if (result != null)
            return (true, result);

        return (false, result);
    }

    public async Task<int> UpdateAsync<T>(T entity) where T : BaseEntity<Guid>
    {
        entity.UpdatedAt = DateTime.Now;
        entity.UpdatedBy = string.IsNullOrEmpty(_httpContextAccessor.AccessToken().userId) ? null : _httpContextAccessor.AccessToken().userId;

        int? result = await db.UpdateAsync(entity);
        return result.GetValueOrDefault();
    }

    public async Task UpdateInTransaction<T>(T entity, IDbTransaction transaction)
    {
        await db.UpdateAsync(entity, transaction);
    }

    public async Task<int> HardDeleteAsync<T>(int id)
    {
        int? result = await db.DeleteAsync<T>(id);
        return result.GetValueOrDefault();
    }

    public IDbTransaction BeginTransaction()
    {
        db.Open();
        return db.BeginTransaction();
    }

    public void CommitTransaction(IDbTransaction transaction)
    {
        transaction.Commit();
    }

    public void RollbackTransaction(IDbTransaction transaction)
    {
        if (transaction != null)
        {
            transaction.Rollback();
        }
    }

    public void CloseConnection()
    {
        if (db.State != ConnectionState.Closed)
        {
            db.Close();
        }
    }
}