using CoreLibrary.Utilities.BaseCRUD;
using CoreLibrary.Utilities.IOC;
using Microsoft.Extensions.DependencyInjection;

namespace CoreLibrary.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencyResolvers(this IServiceCollection serviceCollection, ICoreModule[] modules)
        {
            foreach (var module in modules)
            {
                module.Load(serviceCollection);
            }

            return ServiceTool.Create(serviceCollection);
        }
        
        public static void AddGenericSingleton(this IServiceCollection services, Type interfaceType, Type implementationType, Type genericArgument)
        {
            var interfaceGenericType = interfaceType.MakeGenericType(genericArgument);
            var implementationGenericType = implementationType.MakeGenericType(genericArgument);

            services.AddSingleton(interfaceGenericType, implementationGenericType);
        }
        
        public static void AddEntityCRUDService(this IServiceCollection services, Type entityType)
        {
            var interfaceType = typeof(IBaseCRUDService<>).MakeGenericType(entityType);
            var implementationType = typeof(BaseCRUDService<>).MakeGenericType(entityType);

            services.AddSingleton(interfaceType, implementationType);
        }
    }
}