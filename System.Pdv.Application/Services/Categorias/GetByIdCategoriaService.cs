using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Categorias;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Categorias;

public class GetByIdCategoriaService : IGetByIdCategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly ILogger<GetByIdCategoriaService> _logger;

    public GetByIdCategoriaService(ICategoriaRepository categoriaRepository, ILogger<GetByIdCategoriaService> logger)
    {
        _categoriaRepository = categoriaRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Categoria>> GetById(Guid id)
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
