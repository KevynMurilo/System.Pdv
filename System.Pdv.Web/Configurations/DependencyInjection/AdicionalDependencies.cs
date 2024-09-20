using System.Pdv.Application.Interfaces.Adicionais;
using System.Pdv.Application.UseCase.Adicionais;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Repositories;

namespace System.Pdv.Web.Configurations.DependencyInjection;

public static class AuthDependencyInjection
{
    public static IServiceCollection AddAuthDependencies(this IServiceCollection services)
    {
        services.AddScoped<IAdicionalRepository, AdicionalRepository>();
        services.AddScoped<IGetAllAdicionalUseCase, GetAllAdicionalUseCase>();
        services.AddScoped<IGetByIdAdicionalUseCase, GetByIdAdicionalUseCase>();
        services.AddScoped<ICreateAdicionalUseCase, CreateAdicionalUseCase>();
        services.AddScoped<IUpdateAdicionalUseCase, UpdateAdicionalUseCase>();
        services.AddScoped<IDeleteAdicionalUseCase, DeleteAdicionalUseCase>();
        return services;
    }
}
