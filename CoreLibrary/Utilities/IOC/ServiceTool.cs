using System;
using Microsoft.Extensions.DependencyInjection;

namespace  CoreLibrary.Utilities.IOC
{
    public static class ServiceTool
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static IServiceCollection Create(IServiceCollection services)
        {
            return services;
        }
    }
}