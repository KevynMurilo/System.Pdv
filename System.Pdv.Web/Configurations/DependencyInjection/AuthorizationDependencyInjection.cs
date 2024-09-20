using System.Pdv.Application.Interfaces.Authorization;
using System.Pdv.Application.UseCase.Autorizacao;

namespace System.Pdv.Web.Configurations.DependencyInjection;

public static class AuthorizationDependencyInjection
{
    public static IServiceCollection AddAuthorizationDependencies(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationUseCase, AuthorizationUseCase>();
        return services;
    }
}
