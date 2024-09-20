using System.Pdv.Application.Interfaces.Clientes;
using System.Pdv.Application.UseCase.Clientes;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Repositories;

namespace System.Pdv.Web.Configurations.DependencyInjection;

public static class ClienteDependencyInjection
{
    public static IServiceCollection AddClienteDependencies(this IServiceCollection services)
    {
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IGetAllClienteUseCase, GetAllClienteUseCase>();
        services.AddScoped<IGetByIdClienteUseCase, GetByIdClienteUseCase>();
        services.AddScoped<IGetByNameClienteUseCase, GetByNameClienteUseCase>();
        return services;
    }
}
