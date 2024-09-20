using System.Pdv.Application.Interfaces.Produtos;
using System.Pdv.Application.UseCase.Produtos;
using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Repositories;

namespace System.Pdv.Web.Configurations.DependencyInjection;

public static class ProdutoDependencyInjection
{
    public static IServiceCollection AddProdutoDependencies(this IServiceCollection services)
    {
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IGetAllProdutoUseCase, GetAllProdutoUseCase>();
        services.AddScoped<IGetByIdProdutoUseCase, GetByIdProdutoUseCase>();
        services.AddScoped<IGetProdutoByCategoriaUseCase, GetProdutoByCategoriaUseCase>();
        services.AddScoped<ICreateProdutoUseCase, CreateProdutoUseCase>();
        services.AddScoped<IUpdateProdutoUseCase, UpdateProdutoUseCase>();
        services.AddScoped<IDeleteProdutoUseCase, DeleteProdutoUseCase>();
        return services;
    }
}
