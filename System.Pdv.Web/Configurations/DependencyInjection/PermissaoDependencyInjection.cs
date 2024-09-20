using System.Pdv.Application.Interfaces.Permissoes;
using System.Pdv.Application.UseCase.Permissoes;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Repositories;

namespace System.Pdv.Web.Configurations.DependencyInjection;

public static class PermissaoDependencyInjection
{
    public static IServiceCollection AddPermissaoDependencies(this IServiceCollection services)
    {
        services.AddScoped<IPermissaoRepository, PermissaoRepository>();
        services.AddScoped<IGetAllPermissaoUseCase, GetAllPermissaoUseCase>();
        services.AddScoped<IGetAllPermissaoComRolesUseCase, GetAllPermissaoComRolesUseCase>();
        services.AddScoped<IGetAllPermissaoByRoleIdUseCase, GetAllPermissaoByRoleIdUseCase>();
        return services;
    }
}
