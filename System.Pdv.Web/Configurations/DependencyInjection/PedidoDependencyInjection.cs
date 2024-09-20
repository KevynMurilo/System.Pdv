using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Application.UseCase.Pedidos;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Repositories;
using System.Pdv.Infrastructure.Services.Printer;

namespace System.Pdv.Web.Configurations.DependencyInjection;

public static class PedidoDependencyInjection
{
    public static IServiceCollection AddPedidoDependencies(this IServiceCollection services)
    {
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IPrintPedidoByIdsUseCase, PrintPedidoByIdsUseCase>();
        services.AddScoped<IProcessarItensPedidoUseCase, ProcessarItensPedidoUseCase>();
        services.AddScoped<IValidarPedidosUseCase, ValidarPedidosUseCase>();
        services.AddScoped<IGetAllPedidosUseCase, GetAllPedidosUseCase>();
        services.AddScoped<IGetPedidosByMesaUseCase, GetPedidosByMesaUseCase>();
        services.AddScoped<IGetByIdPedidoUseCase, GetByIdPedidoUseCase>();
        services.AddScoped<ICreatePedidoUseCase, CreatePedidoUseCase>();
        services.AddScoped<IUpdatePedidoUseCase, UpdatePedidoUseCase>();
        services.AddScoped<IDeletePedidoUseCase, DeletePedidoUseCase>();
        return services;
    }
}
