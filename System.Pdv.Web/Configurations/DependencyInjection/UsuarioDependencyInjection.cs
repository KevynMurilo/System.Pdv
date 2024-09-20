using System.Pdv.Application.Interfaces.Usuarios;
using System.Pdv.Application.UseCase.Usuarios;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Repositories;

namespace System.Pdv.Web.Configurations.DependencyInjection;

public static class UsuarioDependencyInjection
{
    public static IServiceCollection AddUsuarioDependencies(this IServiceCollection services)
    {
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IGetAllUsuarioUseCase, GetAllUsuarioUseCase>();
        services.AddScoped<IGetByIdUsuarioUseCase, GetByIdUsuarioUseCase>();
        services.AddScoped<ICreateUsuarioUseCase, CreateUsuarioUseCase>();
        services.AddScoped<IUpdateUsuarioUseCase, UpdateUsuarioUseCase>();
        services.AddScoped<IDeleteUsuarioUseCase, DeleteUsuarioUseCase>();
        return services;
    }
}
