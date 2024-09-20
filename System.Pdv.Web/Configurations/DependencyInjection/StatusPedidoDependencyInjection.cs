using System.Pdv.Application.Interfaces.StatusPedidos;
using System.Pdv.Application.UseCase.StatusPedidos;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Repositories;

namespace System.Pdv.Web.Configurations.DependencyInjection;

public static class StatusPedidoDependencyInjection
{
    public static IServiceCollection AddStatusPedidoDependencies(this IServiceCollection services)
    {
        services.AddScoped<IStatusPedidoRepository, StatusPedidoRepository>();
        services.AddScoped<IGetAllStatusPedidoUseCase, GetAllStatusPedidoUseCase>();
        services.AddScoped<IGetByIdStatusPedidoUseCase, GetByIdStatusPedidoUseCase>();
        services.AddScoped<ICreateStatusPedidoUseCase, CreateStatusPedidoUseCase>();
        services.AddScoped<IUpdateStatusPedidoUseCase, UpdateStatusPedidoUseCase>();
        services.AddScoped<IDeleteStatusPedidoUseCase, DeleteStatusPedidoUseCase>();
        return services;
    }
}
