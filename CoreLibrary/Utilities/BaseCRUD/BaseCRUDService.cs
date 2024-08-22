using System.Dynamic;
using CoreLibrary.Extensions;
using CoreLibrary.Models.Abstract;
using CoreLibrary.Models.Concrete.Entities.Base;
using CoreLibrary.Utilities.DataAccess.Operation.EntityFramework.Abstract;
using CoreLibrary.Utilities.Result;

namespace CoreLibrary.Utilities.BaseCRUD;

public class BaseCRUDService<T> : IBaseCRUDService<T> where T : BaseEntity<Guid>, IEntity
{
    private readonly IEfDynamicBaseQuery _dynamicBaseQuery;
    private readonly IEfDynamicBaseCommand _dynamicBaseCommand;

    public BaseCRUDService(IEfDynamicBaseQuery dynamicBaseQuery, IEfDynamicBaseCommand dynamicBaseCommand)
    {
        _dynamicBaseQuery = dynamicBaseQuery;
        _dynamicBaseCommand = dynamicBaseCommand;
    }

    public async Task<int> Create<T>(dynamic createRequest) where T : BaseEntity<Guid>, IEntity, new()
    {
        dynamic source = new ExpandoObject() as IDictionary<string, object>;
        
        Type destinationType = typeof(T);
        
        dynamic? destination = DynamicMapper.MapToType<T>(createRequest);
        
        dynamic? res = await _dynamicBaseCommand.AddWithGuidIdentityAsync(destination);
        
        if (res is ValueTuple<bool, Guid>)
        {
            (bool, Guid) tuple = (ValueTuple<bool, Guid>)res;

            if (tuple.Item1 == true)
                return 1 ;
            
            else
                return -1;
            
        }
        else
            return -1;
        
    }

    public async Task<int> Update<T>(object updateRequest) where T : BaseEntity<Guid>, IEntity, new()
    {
        dynamic source = new ExpandoObject() as IDictionary<string, object>;
        
        Type destinationType = typeof(T);
        
        dynamic? destination = DynamicMapper.MapToType<T>(updateRequest);
        
        dynamic? res = await _dynamicBaseCommand.UpdateAsync(destination);
        
        if (res is int val)
        {
            if (val == 1)
                return 1 ;
            
            else
                return -1;
            
        }
        else
            return -1;
    }

    public async Task<int> Passive<T>(Guid id) where T : BaseEntity<Guid>, IEntity, new()
    {
        T? val = await _dynamicBaseQuery.GetByExpressionAsync<T>(c => c.Id == id);

        if (val is not null)
        {
            int result = await _dynamicBaseCommand.Passive<T>(val);

            if (result.GreaterThanZero())
                return 1;
        }
        
        return -1;
    }

    public async Task<T> GetById<T>(Guid id) where T : BaseEntity<Guid>, IEntity, new()
    {
        var result = await _dynamicBaseQuery.GetByExpressionAsync<T>(c => c.Id == id);

        if (result is not null)
            return result;

        return null;
    }
}