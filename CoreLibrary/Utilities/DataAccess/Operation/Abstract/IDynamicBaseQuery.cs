using System.Linq.Expressions;

namespace CoreLibrary.Utilities.DataAccess.Operation.Abstract;

public interface IDynamicBaseQuery
{
    Task<T> GetAsync<T>(object id);
    Task<T?> GetByExpressionAsync<T>(Expression<Func<T, bool>> propertyExpression) where T : class;
    Task<List<TEntity>> GetAllByExpressionAsync<TEntity>(Expression<Func<TEntity, bool>> propertyExpression = null) where TEntity : class;
    Task<List<T>> GetAllAsync<T>(string whereCondition, object? parameter = null) where T : class;
    Task<(List<T> record, int count)> GetAllByPaginationAsync<T>(int page, int pageSize, string whereCondition, string orderBy);
}