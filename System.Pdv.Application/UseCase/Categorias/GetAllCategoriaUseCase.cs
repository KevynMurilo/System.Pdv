using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Categorias;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Categorias;

public class GetAllCategoriaUseCase : IGetAllCategoriaUseCase
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly ILogger<GetAllCategoriaUseCase> _logger;

    public GetAllCategoriaUseCase(
        ICategoriaRepository categoriaRepository,
        ILogger<GetAllCategoriaUseCase> logger)
    {
        _categoriaRepository = categoriaRepository;
        _logger = logger;
    }

    public async Task<OperationResult<IEnumerable<Categoria>>> ExecuteAsync()
    {
        try
        {
            var categorias = await _categoriaRepository.GetAllAsync();
            if (!categorias.Any()) return new OperationResult<IEnumerable<Categoria>> { Message = "Nenhuma categoria encontrada", StatusCode = 404 };

            return new OperationResult<IEnumerable<Categoria>>
            {
                Result = categorias
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar categorias");
            return new OperationResult<IEnumerable<Categoria>> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
