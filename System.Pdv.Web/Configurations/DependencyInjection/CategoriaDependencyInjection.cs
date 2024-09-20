using System.Pdv.Application.Interfaces.Categorias;
using System.Pdv.Application.UseCase.Categorias;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Repositories;

namespace System.Pdv.Web.Configurations.DependencyInjection;

public static class CategoriaDependencyInjection
{
    public static IServiceCollection AddCategoriaDependencies(this IServiceCollection services)
    {
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<IGetAllCategoriaUseCase, GetAllCategoriaUseCase>();
        services.AddScoped<IGetByIdCategoriaUseCase, GetByIdCategoriaUseCase>();
        services.AddScoped<ICreateCategoriaUseCase, CreateCategoriaUseCase>();
        services.AddScoped<IUpdateCategoriaUseCase, UpdateCategoriaUseCase>();
        services.AddScoped<IDeleteCategoriaUseCase, DeleteCategoriaUseCase>();
        return services;
    }
}
