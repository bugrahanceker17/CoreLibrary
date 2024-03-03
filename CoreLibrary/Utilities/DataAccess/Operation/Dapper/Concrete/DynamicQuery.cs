using System.Data;
using System.Linq.Expressions;
using CoreLibrary.Models.Setting;
using CoreLibrary.Utilities.DataAccess.Operation.Dapper.Abstract;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace CoreLibrary.Utilities.DataAccess.Operation.Dapper.Concrete;

public class DynamicQuery : IDynamicQuery
{
    private readonly IDbConnection db;

    public DynamicQuery(IOptions<ConfigurationValues> configuration)
    {
        ConfigurationValues configurationValues = configuration.Value;
        db = new SqlConnection(configurationValues.Database.ConnectionString);
    }

    public async Task<T> GetAsync<T>(object id)
    {
        return await db.GetAsync<T>(id);
    }

    public async Task<T> GetByExpressionAsync<T>(Expression<Func<T, bool>> propertyExpression)
    {
        return await db.GetAsync<T>(propertyExpression);
    }

    public async Task<List<T>> GetAllByExpressionAsync<T>(Expression<Func<T, bool>> propertyExpression)
    {
        return (await db.GetListAsync<T>(propertyExpression)).ToList();
    }

    public async Task<List<T>> GetAllAsync<T>(string whereCondition, object? parameter = null)
    {
        string def = "WHERE IsStatus = 1 AND IsDeleted = 0";

        string baseWhereCondition = string.IsNullOrEmpty(whereCondition)
            ? def
            : whereCondition.Contains("WHERE ")
                ? whereCondition
                : $"WHERE {whereCondition} ";

        return (await db.GetListAsync<T>(baseWhereCondition, parameter)).ToList();
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