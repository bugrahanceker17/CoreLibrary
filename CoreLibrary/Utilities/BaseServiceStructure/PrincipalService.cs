using CoreLibrary.Extensions;
using CoreLibrary.Utilities.DataAccess.Operation.EntityFramework.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CoreLibrary.Utilities.BaseServiceStructure;

public abstract class PrincipalService
{
    private readonly IServiceProvider _serviceProvider;
    private IEfDynamicBaseCommand _dynamicBaseCommand;
    private IEfDynamicBaseQuery _dynamicBaseQuery;
    private static IHttpContextAccessor _httpContextAccessor;
    private IEfAuthOperation _authOperation;

    protected PrincipalService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected IEfDynamicBaseCommand DynamicCommand
    {
        get { return _dynamicBaseCommand ??= this._serviceProvider.GetRequiredService<IEfDynamicBaseCommand>(); }
    }

    protected IEfDynamicBaseQuery DynamicQuery
    {
        get { return _dynamicBaseQuery ??= this._serviceProvider.GetRequiredService<IEfDynamicBaseQuery>(); }
    }

    protected IHttpContextAccessor HttpContextAccessor
    {
        get { return _httpContextAccessor ??= this._serviceProvider.GetRequiredService<IHttpContextAccessor>(); }
    }

    protected IEfAuthOperation Auth
    {
        get { return _authOperation ??= this._serviceProvider.GetRequiredService<IEfAuthOperation>(); }
    }
}