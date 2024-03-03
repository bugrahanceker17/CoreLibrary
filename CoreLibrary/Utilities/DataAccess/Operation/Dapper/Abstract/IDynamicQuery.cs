using System.Linq.Expressions;

namespace CoreLibrary.Utilities.DataAccess.Operation.Dapper.Abstract;

public interface IDynamicQuery
{
    Task<T> GetAsync<T>(object id);
    Task<T> GetByExpressionAsync<T>(Expression<Func<T, bool>> propertyExpression);
    Task<List<T>> GetAllByExpressionAsync<T>(Expression<Func<T, bool>> propertyExpression);
    Task<List<T>> GetAllAsync<T>(string whereCondition, object? parameter = null);
    Task<(List<T> record, int count)> GetAllByPaginationAsync<T>(int page, int pageSize, string whereCondition, string orderBy);
}