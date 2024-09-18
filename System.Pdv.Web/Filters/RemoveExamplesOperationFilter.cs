using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace System.Pdv.Web.Filters;

public class RemoveExamplesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var statusCodesToRemove = new[] { "400", "401", "404", "409", "500" };

        foreach (var statusCode in statusCodesToRemove)
        {
            if (operation.Responses.ContainsKey(statusCode))
            {
                operation.Responses[statusCode].Content.Clear();
            }
        }
    }
}
