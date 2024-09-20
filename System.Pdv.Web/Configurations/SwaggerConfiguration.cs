using Microsoft.OpenApi.Models;
using System.Pdv.Web.Filters;

namespace System.Pdv.Web.Configurations.Swagger;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "SISTEMA PARA GERENCIAMENTO DE PEDIDOS E MESAS", Version = "v1" });
            c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Insira o token JWT no campo abaixo:",
            });

            c.OperationFilter<AuthorizeOperationFilter>();
            c.OperationFilter<RemoveExamplesOperationFilter>();
        });

        return services;
    }
}
