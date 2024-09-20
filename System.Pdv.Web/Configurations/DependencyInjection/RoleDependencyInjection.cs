using System.Pdv.Application.Interfaces.Roles;
using System.Pdv.Application.UseCase.Roles;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Repositories;

namespace System.Pdv.Web.Configurations.DependencyInjection;

public static class RoleDependencyInjection
{
    public static IServiceCollection AddRoleDependencies(this IServiceCollection services)
    {
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IGetAllRolesUseCase, GetAllRolesUseCase>();
        services.AddScoped<IGetByIdRoleUseCase, GetByIdRoleUseCase>();
        services.AddScoped<ICreateRoleUseCase, CreateRoleUseCase>();
        services.AddScoped<IUpdateRoleUseCase, UpdateRoleUseCase>();
        services.AddScoped<IDeleteRoleUseCase, DeleteRoleUseCase>();
        return services;
    }
}
