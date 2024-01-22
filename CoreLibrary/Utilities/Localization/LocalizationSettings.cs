using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CoreLibrary.Utilities.Localization;

public static class LocalizationSettings
{
    public static void Initialize(this IServiceCollection services)
    {
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new("tr-TR");
 
            CultureInfo[] cultures = new CultureInfo[]
            {
                new("tr-TR"),
                new("en-US")
            };
 
            options.SupportedCultures = cultures;
            options.SupportedUICultures = cultures;
        });
    }
}