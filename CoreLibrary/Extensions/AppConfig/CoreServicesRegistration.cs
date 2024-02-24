using CoreLibrary.Aspects.DependencyResolves;
using CoreLibrary.Utilities.DataAccess.Dapper.Abstract;
using CoreLibrary.Utilities.DataAccess.Dapper.Concrete;
using CoreLibrary.Utilities.IOC;
using CoreLibrary.Utilities.MailSender;
using CoreLibrary.Utilities.Security.JWT;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CoreLibrary.Extensions.AppConfig;

public static class CoreServicesRegistration
{
    public static void AddCoreServices(this IServiceCollection services)
    {
        services.AddControllers();
        
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
        
        services.AddSingleton<IDynamicQuery, DynamicQuery>();
        services.AddSingleton<ITokenHelper, JwtHelper>();
        services.AddSingleton<IDynamicCommand, DynamicCommand>();
        services.AddSingleton<IMailSender, MailSender>();
        services.AddSingleton<IAuthOperation, AuthOperation>();
    }
}