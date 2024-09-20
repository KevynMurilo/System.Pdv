using System.Pdv.Application.Interfaces.MetodosPagamento;
using System.Pdv.Application.UseCase.MetodosPagamento;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Repositories;

namespace System.Pdv.Web.Configurations.DependencyInjection;

public static class MetodoPagamentoDependencyInjection
{
    public static IServiceCollection AddMetodoPagamentoDependencies(this IServiceCollection services)
    {
        services.AddScoped<IMetodoPagamentoRepository, MetodoPagamentoRepository>();
        services.AddScoped<IGetAllMetodoPagamentoUseCase, GetAllMetodoPagamentoUseCase>();
        services.AddScoped<IGetByIdMetodoPagamentoUseCase, GetByIdMetodoPagamentoUseCase>();
        services.AddScoped<ICreateMetodoPagamentoUseCase, CreateMetodoPagamentoUseCase>();
        services.AddScoped<IUpdateMetodoPagamentoUseCase, UpdateMetodoPagamentoUseCase>();
        services.AddScoped<IDeleteMetodoPagamentoUseCase, DeleteMetodoPagamentoUseCase>();
        return services;
    }
}
