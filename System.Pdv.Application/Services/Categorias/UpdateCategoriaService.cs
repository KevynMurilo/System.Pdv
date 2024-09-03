using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Categorias;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Categorias;

public class UpdateCategoriaService : IUpdateCategoriaService
{
    private readonly ICategoriaRepository _categoriasRepository;
    private readonly ILogger<UpdateCategoriaService> _logger;

    public UpdateCategoriaService(ICategoriaRepository categoriasRepository, ILogger<UpdateCategoriaService> logger)
    {
        _categoriasRepository = categoriasRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Categoria>> UpdateCategoria(Guid id, CategoriaDto categoriaDto)
    {
        try
        {
            var categoria = await _categoriasRepository.GetByIdAsync(id);
            if (categoria == null) return new OperationResult<Categoria> { Message = "Categoria não encontrada", StatusCode = 404 };

            categoria.Nome = categoriaDto.Nome.ToUpper();

            await _categoriasRepository.UpdateAsync(categoria);

            return new OperationResult<Categoria> { Result = categoria };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao atualizar categoria");
            return new OperationResult<Categoria> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }

}
