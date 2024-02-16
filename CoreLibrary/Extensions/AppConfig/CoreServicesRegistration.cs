using CoreLibrary.Aspects.DependencyResolves;
using CoreLibrary.Utilities.DataAccess.Dapper.Abstract;
using CoreLibrary.Utilities.DataAccess.Dapper.Concrete;
using CoreLibrary.Utilities.IOC;
using CoreLibrary.Utilities.MailSender;
using CoreLibrary.Utilities.Security.JWT;
using Microsoft.Extensions.DependencyInjection;

namespace CoreLibrary.Extensions.AppConfig;

public static class CoreServicesRegistration
{
    public static void AddCoreServices(this IServiceCollection services)
    {
        services.AddControllers();
        
        services.AddHttpContextAccessor();

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