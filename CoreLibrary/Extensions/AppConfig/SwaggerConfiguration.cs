﻿using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CoreLibrary.Extensions.AppConfig;

public static class SwaggerConfiguration
{
    public static void SwaggerConfigure(this IApplicationBuilder app, string assembly, string version)
    {
        app.UseSwagger(options =>
            {
                options.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    if (httpReq.Headers.ContainsKey("X-Forwarded-Host"))
                    {
                        string basePath = $"{assembly}";
                        string serverUrl = $"https://{httpReq.Headers["X-Forwarded-Host"]}/{basePath}";
                        swagger.Servers = new List<OpenApiServer> { new() { Url = serverUrl } };
                    }
                });

                options.RouteTemplate = "swagger/{documentName}/swagger.json";
            })
            .UseSwaggerUI(options =>
            {
                options.RoutePrefix = "swagger";
                options.SwaggerEndpoint("v1/swagger.json", $"{assembly} {version}");
            });
    }

    public static void SwaggerRegister(this IServiceCollection services, string assembly, string version)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(version, new OpenApiInfo
            {
                Title = assembly,
                Version = version,
                Description = "POWERED BY BGR",
                // Contact = new OpenApiContact()
                // {
                //     Name = "Bugrahan",
                //     Url = new Uri("https://github.com/bugrahanceker17")
                // }
            });
            c.OperationFilter<SwaggerCultureFilter>();

            OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "Enter JWT Bearer token **_only_**",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            };

            c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, Array.Empty<string>() }
            });

            // string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            // string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            // c.IncludeXmlComments(xmlPath);
        });
    }
}