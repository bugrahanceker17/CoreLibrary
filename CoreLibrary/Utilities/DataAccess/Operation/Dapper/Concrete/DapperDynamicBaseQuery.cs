using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using CoreLibrary.Models.Setting;
using CoreLibrary.Utilities.DataAccess.Operation.Abstract;
using CoreLibrary.Utilities.DataAccess.Operation.Dapper.Abstract;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using TableAttribute = System.ComponentModel.DataAnnotations.Schema.TableAttribute;

namespace CoreLibrary.Utilities.DataAccess.Operation.Dapper.Concrete;

public class DapperDynamicBaseQuery : IDapperDynamicBaseQuery
{
    private readonly IDbConnection db;

    public DapperDynamicBaseQuery(IOptions<ConfigurationValues> configuration)
    {
        ConfigurationValues configurationValues = configuration.Value;
        db = new SqlConnection(configurationValues.Database.ConnectionString);
    }

    public async Task<T> GetAsync<T>(object id)
    {
        return await db.GetAsync<T>(id);
    }

    
    async Task<T> IDynamicBaseQuery.GetByExpressionAsync<T>(Expression<Func<T, bool>> propertyExpression) where T : class
    {
        T? item = (await db.GetListAsync<T>()).ToList().Where(propertyExpression.Compile()).FirstOrDefault();
        return item;
    }

    public async Task<List<TEntity>> GetAllByExpressionAsync<TEntity>(Expression<Func<TEntity, bool>> propertyExpression) where TEntity : class
    {
        var query = BuildQuery(propertyExpression);
        var result = await db.QueryAsync<TEntity>(query.Sql, query.Parameters);
        return result.ToList();
    }

    public async Task<List<T>> GetAllAsync<T>(string whereCondition, object? parameter = null) where T : class
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
    
    private (string Sql, object Parameters) BuildQuery<T>(Expression<Func<T, bool>> propertyExpression)
    {
        var visitor = new SqlExpressionVisitor();
        visitor.Visit(propertyExpression);
        string tableName = GetTableName<T>();
        var sql = $"SELECT * FROM {tableName} WHERE {visitor.Sql}";
        return (sql, visitor.Parameters);
    }
    
    public class SqlExpressionVisitor : ExpressionVisitor
    {
        public string Sql { get; private set; }
        public DynamicParameters Parameters { get; } = new DynamicParameters();

        protected override Expression VisitBinary(BinaryExpression node)
        {
            // Operatörü belirle
            string operatorStr = node.NodeType switch
            {
                ExpressionType.Equal => "=",
                ExpressionType.NotEqual => "<>",
                ExpressionType.GreaterThan => ">",
                ExpressionType.GreaterThanOrEqual => ">=",
                ExpressionType.LessThan => "<",
                ExpressionType.LessThanOrEqual => "<=",
                _ => throw new NotSupportedException($"Expression type '{node.NodeType}' is not supported")
            };

            // Sol tarafı işle
            Visit(node.Left);
            Sql += $" {operatorStr} ";

            // Sağ tarafı işle
            Visit(node.Right);

            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            // Sabit değeri al
            var paramName = $"@param{Parameters.ParameterNames.Count()}";
            Parameters.Add(paramName, node.Value);
            Sql += paramName;
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            // Üye ifadesini işle
            Sql += node.Member.Name;
            return node;
        }
    }
    
    private string GetTableName<T>()
    {
        var type = typeof(T);
        var tableAttribute = type.GetCustomAttribute<TableAttribute>();
        if (tableAttribute != null)
        {
            return tableAttribute.Name;
        }
        else
        {
            // Tablo adı belirtilmemişse, varsayılan olarak sınıf adını kullan
            return type.Name;
        }
    }
}