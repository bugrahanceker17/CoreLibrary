using Microsoft.Extensions.DependencyInjection;

namespace CoreLibrary.Utilities.IOC
{
    public interface ICoreModule
    {
        void Load(IServiceCollection serviceCollection);
    }
}

