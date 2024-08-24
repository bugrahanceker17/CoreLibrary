using System.Data;
using CoreLibrary.Extensions;
using CoreLibrary.Models.Concrete.Entities.Base;
using CoreLibrary.Utilities.DataAccess.Operation.EntityFramework.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CoreLibrary.Utilities.DataAccess.Operation.EntityFramework.Concrete;

public class EfDynamicBaseCommand : IEfDynamicBaseCommand 
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DbContext _context;
    
    public EfDynamicBaseCommand(IHttpContextAccessor httpContextAccessor, DbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    public async Task<(bool succeeded, Guid id)> AddWithGuidIdentityAsync<T>(T entity) where T : BaseEntity<Guid>
    {
        string userId = _httpContextAccessor.AccessToken().userId;

        entity.Id = Guid.NewGuid();
        entity.CreatedBy = string.IsNullOrEmpty(userId) ? null : userId;
        entity.CreatedAt = DateTimeOffset.Now;
        entity.IsDeleted = false;
        entity.IsStatus = true;
        
        await _context.Set<T>().AddAsync(entity);
        
        int res = await _context.SaveChangesAsync();

        if (res > 0)
            return (true, entity.Id);

        return (false, Guid.Empty);
    }

    public Task<object?> AddInTransaction<T>(T entity, IDbTransaction transaction)
    {
        throw new NotImplementedException();
    }

    public async Task<(bool succeeded, TIdentityType? id)> AddAsync<T, TIdentityType>(T entity) where T : BaseEntity<TIdentityType>
    {
        EntityEntry<T> data = await _context.Set<T>().AddAsync(entity);
        int res = await _context.SaveChangesAsync();

        return res > 0 ? (true, data.Entity.Id) : (false, data.Entity.Id);
    }

    public async Task<int> UpdateAsync<T>(T entity) where T : BaseEntity<Guid>
    {
        int res;
        
        T? existingEntity = await _context.Set<T>().FindAsync(entity.Id);
    
        if (existingEntity is not null)
        {
            entity.UpdatedAt = DateTimeOffset.Now;
            entity.UpdatedBy = string.IsNullOrEmpty(_httpContextAccessor.AccessToken().userId) ? null : _httpContextAccessor.AccessToken().userId;
            
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            res = await _context.SaveChangesAsync();
            
            if (res > 0)
                return 1;
            
            return 0;
        }

        return -1;
    }

    public Task UpdateInTransaction<T>(T entity, IDbTransaction transaction)
    {
        throw new NotImplementedException();
    }

    public async Task<int> HardDeleteAsync<T>(T entity) where T : class
    {
        _context.Set<T>().Remove(entity);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> Passive<T>(T entity) where T : BaseEntity<Guid>
    {
        int res;
        
        T? existingEntity = await _context.Set<T>().FindAsync(entity.Id);
    
        if (existingEntity is not null)
        {
            entity.UpdatedAt = DateTimeOffset.Now;
            entity.UpdatedBy = string.IsNullOrEmpty(_httpContextAccessor.AccessToken().userId) ? null : _httpContextAccessor.AccessToken().userId;
            entity.IsDeleted = true;
            entity.IsStatus = false;
            
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            res = await _context.SaveChangesAsync();
            
            if (res > 0)
                return 1;
            
            return 0;
        }

        return -1;
    }

    public IDbTransaction BeginTransaction()
    {
        throw new NotImplementedException();
    }

    public void CommitTransaction(IDbTransaction transaction)
    {
        throw new NotImplementedException();
    }

    public void RollbackTransaction(IDbTransaction transaction)
    {
        throw new NotImplementedException();
    }

    public void CloseConnection()
    {
        throw new NotImplementedException();
    }
}