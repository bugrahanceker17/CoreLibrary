using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace CoreLibrary.Utilities.Localization;

public static class LocalizationSettings
{
    public static void Initialize_LocalizationSettings(this IServiceCollection services)
    {
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("tr-TR");
 
            CultureInfo[] cultures = {
                new("tr-TR"),
                new("en-US")
            };
 
            options.SupportedCultures = cultures;
            options.SupportedUICultures = cultures;
        });
    }

    // FROM : REQUEST HEADER
    public static void Register_LocalizationSettings(this IApplicationBuilder app)
    {
        List<CultureInfo> supportedCultures = new() { new CultureInfo("en-US"), new CultureInfo("tr-TR") };
        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("tr-TR"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures,
            RequestCultureProviders = new[] { new AcceptLanguageHeaderRequestCultureProvider() }
        };

        app.UseRequestLocalization(localizationOptions);

        app.Use(async (context, next) =>
        {
            var acceptLanguageHeader = context.Request.Headers["Accept-Language"].ToString();
            if (!string.IsNullOrEmpty(acceptLanguageHeader))
            {
                try
                {
                    var cultureName = acceptLanguageHeader.Split(',').First().Split(';').First().Trim();
                    var culture = new CultureInfo(cultureName);

                    CultureInfo.CurrentCulture = culture;
                    CultureInfo.CurrentUICulture = culture;
                }
                catch (CultureNotFoundException)
                {
                    CultureInfo.CurrentCulture = new CultureInfo("tr-TR");
                    CultureInfo.CurrentUICulture = new CultureInfo("tr-TR");
                }
            }

            await next.Invoke();
        });
    }

    // FROM : REQUEST ROUTE
    // public static void Register_LocalizationSettings(this IApplicationBuilder app)
    // {
    //     List<CultureInfo> supportedCultures = new() { new CultureInfo("en-US"), new CultureInfo("tr-TR") };
    //     var localizationOptions = new RequestLocalizationOptions
    //     {
    //         DefaultRequestCulture = new RequestCulture("tr-TR"),
    //         SupportedCultures = supportedCultures,
    //         SupportedUICultures = supportedCultures
    //     };
    //     
    //     app.UseRequestLocalization(localizationOptions);
    //     
    //     app.Use(async (context, next) =>
    //     {
    //         var pathSegments = context.Request.Path.Value.Split('/');
    //         var cultureQuery = pathSegments.Length > 2 ? pathSegments[2] : "tr-TR";
    //         var culture = new CultureInfo(cultureQuery);
    //         CultureInfo.CurrentCulture = culture;
    //         CultureInfo.CurrentUICulture = culture;
    //         await next.Invoke();
    //     });
    // }
}