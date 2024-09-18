using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace System.Pdv.Web.Filters;

public class RemoveExamplesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Remove exemplos para status 404, 409 e 500
        var statusCodesToRemove = new[] { "404", "409", "500" };

        foreach (var statusCode in statusCodesToRemove)
        {
            if (operation.Responses.ContainsKey(statusCode))
            {
                operation.Responses[statusCode].Content.Clear();
            }
        }
    }
}
