using System.Pdv.Application.Interfaces.Mesas;
using System.Pdv.Application.UseCase.Mesas;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Repositories;

namespace System.Pdv.Web.Configurations.DependencyInjection;

public static class MesaDependencyInjection
{
    public static IServiceCollection AddMesaDependencies(this IServiceCollection services)
    {
        services.AddScoped<IMesaRepository, MesaRepository>();
        services.AddScoped<IGetAllMesaUseCase, GetAllMesaUseCase>();
        services.AddScoped<IGetMesaByIdUseCase, GetMesaByIdUseCase>();
        services.AddScoped<ICreateMesaUseCase, CreateMesaUseCase>();
        services.AddScoped<IUpdateMesaUseCase, UpdateMesaUseCase>();
        services.AddScoped<IDeleteMesaUseCase, DeleteMesaUseCase>();
        return services;
    }
}
