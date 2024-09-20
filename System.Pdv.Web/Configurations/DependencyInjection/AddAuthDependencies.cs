using System.Pdv.Application.Interfaces.Auth;
using System.Pdv.Application.Interfaces.Usuarios;
using System.Pdv.Application.UseCase.Auth;
using System.Pdv.Application.UseCase.Usuarios;

namespace System.Pdv.Web.Configurations.DependencyInjection;

public static class AdicionalDependencyInjection
{
    public static IServiceCollection AdicionalDependencies(this IServiceCollection services)
    {
        services.AddScoped<IAuthUseCase, AuthUseCase>();
        services.AddScoped<IJwtTokenGeneratorUsuario, JwtTokenGeneratorUsuario>();
        return services;
    }
}
