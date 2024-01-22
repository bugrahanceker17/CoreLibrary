using System.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CoreLibrary.Extensions
{
    public class SwaggerCultureFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // var cultureParameter = new OpenApiParameter
            // {
            //     Name = "culture",
            //     In = ParameterLocation.Query,
            //     Required = false,
            //     Schema = new OpenApiSchema
            //     {
            //         Type = "string",
            //         Default = new OpenApiString("tr")
            //     }
            // };
            //
            // operation.Parameters.Add(cultureParameter);
            
            var cultureParameter = operation.Parameters.FirstOrDefault(p => p.Name == "culture");
            if (cultureParameter != null)
            {
                cultureParameter.Schema.Default = new OpenApiString("tr");
            }
        }
    }
}