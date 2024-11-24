using System.Data;
using System.Linq.Expressions;
using CoreLibrary.Models.Setting;
using CoreLibrary.Utilities.DataAccess.Operation.EntityFramework.Abstract;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CoreLibrary.Utilities.DataAccess.Operation.EntityFramework.Concrete;

public class EfDynamicBaseQuery : IEfDynamicBaseQuery
{
    private readonly IDbConnection db;
    private readonly DbContext _context;

    public EfDynamicBaseQuery(IOptions<ConfigurationValues> configuration, DbContext context)
    {
        _context = context;
        ConfigurationValues configurationValues = configuration.Value;
        db = new SqlConnection(configurationValues.Database.ConnectionString);
    }

    public async Task<T> GetAsync<T>(object id)
    {
        throw new NotImplementedException();
    }

    public async Task<T?> GetByExpressionAsync<T>(Expression<Func<T, bool>> propertyExpression) where T : class
    {
        return await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(propertyExpression);
    }

    public async Task<List<TEntity>> GetAllByExpressionAsync<TEntity>(Expression<Func<TEntity, bool>> propertyExpression = null) where TEntity : class
    {
        return propertyExpression == null ? _context.Set<TEntity>().ToList() : _context.Set<TEntity>().Where(propertyExpression).ToList();
    }

    public async Task<List<T>> GetAllAsync<T>(string whereCondition, object? parameter = null) where T : class
    {
        return _context.Set<T>().ToList();
    }

    public async Task<(List<T> record, int count)> GetAllByPaginationAsync<T>(int page, int pageSize, string whereCondition, string orderBy)
    {
        string def = "WHERE IsStatus = 1 AND IsDeleted = 0";

        string baseWhereCondition = string.IsNullOrEmpty(whereCondition)
            ? def
            : whereCondition.Contains("WHERE ")
                ? whereCondition
                : $"WHERE {whereCondition} ";

        int count = await db.RecordCountAsync<T>(baseWhereCondition);

        List<T> record = (await db.GetListPagedAsync<T>(page, pageSize, baseWhereCondition, orderBy)).ToList();

        return (record, count);
    }
}