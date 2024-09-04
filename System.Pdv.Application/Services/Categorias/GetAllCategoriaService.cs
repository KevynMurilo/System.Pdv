using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Categorias;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Categorias;

public class GetAllCategoriaService : IGetAllCategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly ILogger<GetAllCategoriaService> _logger;

    public GetAllCategoriaService(
        ICategoriaRepository categoriaRepository,
        ILogger<GetAllCategoriaService> logger)
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
