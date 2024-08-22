using CoreLibrary.Aspects.DependencyResolves;
using CoreLibrary.Utilities.Attribute;
using CoreLibrary.Utilities.DataAccess.Operation.Dapper.Abstract;
using CoreLibrary.Utilities.DataAccess.Operation.Dapper.Concrete;
using CoreLibrary.Utilities.DataAccess.Operation.EntityFramework.Abstract;
using CoreLibrary.Utilities.DataAccess.Operation.EntityFramework.Concrete;
using CoreLibrary.Utilities.IOC;
using CoreLibrary.Utilities.Localization;
using CoreLibrary.Utilities.MailSender;
using CoreLibrary.Utilities.Security.JWT;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CoreLibrary.Extensions.AppConfig;

public static class CoreServicesRegistration 
{
    public static void AddCoreServices(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<AttributeFilter>();
        });
        
        services.AddEndpointsApiExplorer();
        
        services.AddHttpContextAccessor();
        
        services.Configure<CookiePolicyOptions>(options =>
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        });

        services.AddDependencyResolvers(new ICoreModule[] { new CoreModule() });
        
        services.AddCors(options =>
            options.AddDefaultPolicy(builder => builder
                .WithOrigins("*")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin())
            );

        services.AddSingleton<ITokenHelper, JwtHelper>();
        services.AddSingleton<IMailSender, MailSender>();
        
        services.AddTransient<IEfDynamicBaseCommand, EfDynamicBaseCommand>();
        services.AddTransient<IEfDynamicBaseQuery, EfDynamicBaseQuery>();
        services.AddTransient<IEfAuthOperation, EfAuthOperation>();
        
        services.AddTransient<IDapperAuthOperation, DapperAuthOperation>();
        services.AddTransient<IDapperDynamicBaseCommand, DapperDynamicBaseCommand>();
        services.AddTransient<IDapperDynamicBaseQuery, DapperDynamicBaseQuery>();

        services.AddSingleton<ICultureHelper, CultureHelper>();
        services.AddSingleton<ITranslateHelper, TranslateHelper>();
    }
}