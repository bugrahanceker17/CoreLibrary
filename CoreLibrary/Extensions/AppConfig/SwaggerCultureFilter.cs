using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CoreLibrary.Extensions.AppConfig;

public class SwaggerCultureFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var cultureParameter = operation.Parameters.FirstOrDefault(p => p.Name == "culture");
        if (cultureParameter != null)
        {
            cultureParameter.Schema.Default = new OpenApiString("tr-TR");
            cultureParameter.Schema.Example = new OpenApiString("tr-TR");
        }
    }
}