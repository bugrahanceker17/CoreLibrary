﻿using Microsoft.AspNetCore.Http;

namespace CoreLibrary.Utilities.Localization;

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

        await _next(context);
    }
}