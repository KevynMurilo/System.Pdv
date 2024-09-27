using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Categorias;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Categorias;

public class UpdateCategoriaUseCase : IUpdateCategoriaUseCase
{
    private readonly ICategoriaRepository _categoriasRepository;
    private readonly ILogger<UpdateCategoriaUseCase> _logger;

    public UpdateCategoriaUseCase(
        ICategoriaRepository categoriasRepository,
        ILogger<UpdateCategoriaUseCase> logger)
    {
        _categoriasRepository = categoriasRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Categoria>> ExecuteAsync(Guid id, CategoriaDto categoriaDto)
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
            return new OperationResult<Categoria> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }

}
