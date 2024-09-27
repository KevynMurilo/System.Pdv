using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Categorias;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Categorias;
public class DeleteCategoriaUseCase : IDeleteCategoriaUseCase
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly ILogger<DeleteCategoriaUseCase> _logger;

    public DeleteCategoriaUseCase(
        ICategoriaRepository categoriaRepository,
        ILogger<DeleteCategoriaUseCase> logger)
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

            await _categoriaRepository.DeleteAsync(categoria);

            return new OperationResult<Categoria> { Result = categoria, Message = "Categoria deletada com sucesso!" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao deletar categoria");
            return new OperationResult<Categoria> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
