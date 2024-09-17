using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CoreLibrary.Utilities.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        
        string method = request.Method;
        PathString endpoint = request.Path;
        
        _logger.LogInformation("Type: {Method} ** Endpoint: {Endpoint}", method, endpoint);
        
        if (request.Method == HttpMethods.Get || request.Method == HttpMethods.Delete)
        {
            IQueryCollection queryParams = request.Query;
            
            if (queryParams.Any())
            {
                Console.WriteLine("Query Parameters:");
                foreach (var param in queryParams)
                {
                    Console.WriteLine($"{param.Key}: {param.Value}");
                }
            }
            else
                Console.WriteLine("No Query Parameters present.");
            
        }
        else if (request.Method == HttpMethods.Post || request.Method == HttpMethods.Put)
        {
            request.EnableBuffering();

            using var reader = new StreamReader(request.Body, leaveOpen: true);
            string body = await reader.ReadToEndAsync();

            if (!string.IsNullOrWhiteSpace(body))
                Console.WriteLine($"Request Body: {body}");
                
            else
                Console.WriteLine("Request Body is empty or not present.");

            request.Body.Position = 0;
        }

        await _next(context);
    }
}