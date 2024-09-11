using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Categorias;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Categorias;

public class GetByIdCategoriaUseCase : IGetByIdCategoriaUseCase
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly ILogger<GetByIdCategoriaUseCase> _logger;

    public GetByIdCategoriaUseCase(
        ICategoriaRepository categoriaRepository,
        ILogger<GetByIdCategoriaUseCase> logger)
    {
        _categoriaRepository = categoriaRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Categoria>> ExecuteAsync(Guid id)
    {
        try
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            if (categoria == null) return new OperationResult<Categoria> { Message = "Categoria não encontrada", StatusCode = 404 };

            return new OperationResult<Categoria> { Result = categoria };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao procurar categoria por id");
            return new OperationResult<Categoria> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
