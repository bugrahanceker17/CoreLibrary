using CoreLibrary.Utilities.Localization;
using Microsoft.AspNetCore.Http;

namespace CoreLibrary.Utilities.Middleware;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;

    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICultureHelper cultureService)
    {
        var cultureName = context.Request.RouteValues["culture"]?.ToString();

        if (!string.IsNullOrEmpty(cultureName))
        {
            cultureService.SetCurrentCulture(cultureName);
        }
        else
        {
            cultureName = context.Request.Headers.AcceptLanguage[0]?.Split(',')[0];
            
            if (!string.IsNullOrEmpty(cultureName))
            {
                cultureService.SetCurrentCulture(cultureName);
            }
        }

        await _next(context);
    }
}