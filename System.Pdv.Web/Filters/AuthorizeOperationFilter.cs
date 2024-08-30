using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace System.Pdv.Web.Filters;

public class AuthorizeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {

        var authAttributes = context.MethodInfo
          .GetCustomAttributes(true)
          .OfType<AuthorizeAttribute>()
          .Distinct();

        if (authAttributes.Any())
        {

            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

            //Essa parte vincula o esquema JWT a operação que ta no program.cs
            var jwtbearerScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" } ////Id tem que ter o mesmo nome que AddSecurityDefinition no Program.cs////
            };

            //E essa parte aplica o requisito (de aparecer o cadeado) apenas em rotas que precisam do token.
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
