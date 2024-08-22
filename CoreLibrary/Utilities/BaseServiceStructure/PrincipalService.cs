using CoreLibrary.Utilities.DataAccess.Operation.EntityFramework.Abstract;
using CoreLibrary.Utilities.Localization;
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
    private ITranslateHelper _translateHelper;

    protected PrincipalService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected IEfDynamicBaseCommand DynamicCommand
    {
        get { return _dynamicBaseCommand ??= this._serviceProvider.GetRequiredService<IEfDynamicBaseCommand>(); }
    }
    
    protected ITranslateHelper Translate
    {
        get { return _translateHelper ??= this._serviceProvider.GetRequiredService<ITranslateHelper>(); }
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
    
}