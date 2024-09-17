using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace System.Pdv.Web.Filters;

public class AuthorizeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Verifica se o método tem o AuthorizeAttribute ou HasPermissionAttribute
        var authAttributes = context.MethodInfo
          .GetCustomAttributes(true)
          .OfType<AuthorizeAttribute>()
          .Distinct();

        var permissionAttributes = context.MethodInfo
          .GetCustomAttributes(true)
          .OfType<HasPermissionAttribute>()
          .Distinct();

        // Aplica as regras apenas se houver Authorize ou HasPermission
        if (authAttributes.Any() || permissionAttributes.Any())
        {
            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

            // Vincula o esquema JWT
            var jwtbearerScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" } // Id tem que ser o mesmo definido no Program.cs
            };

            // Aplica o cadeado de segurança às rotas protegidas
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    [ jwtbearerScheme ] = new string[] { }
                }
            };
        }
    }
}
