using CoreLibrary.Utilities.DataAccess.Operation.EntityFramework.Abstract;
using CoreLibrary.Utilities.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace CoreLibrary.Utilities.BaseServiceStructure;

public abstract class PrincipalService<T> where T: class
{
    private readonly IServiceProvider _serviceProvider;
    private IEfDynamicBaseCommand _dynamicBaseCommand;
    private IEfDynamicBaseQuery _dynamicBaseQuery;
    private static IHttpContextAccessor _httpContextAccessor;
    private IEfAuthOperation _authOperation;
    private IStringLocalizer _localizer;

    protected PrincipalService(IServiceProvider serviceProvider, IStringLocalizer<dynamic> localizer)
    {
        _serviceProvider = serviceProvider;
        _localizer = localizer;
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

    public static DataResult ThrowDataResultError(string message) => new() { ErrorMessageList = new List<string> { message } };

    protected string Translate(string key)
    {
        return _localizer[key].Value;
    }
}