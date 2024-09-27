using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Categorias;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Categorias;

public class CreateCategoriaUseCase : ICreateCategoriaUseCase
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly ILogger<CreateCategoriaUseCase> _logger;

    public CreateCategoriaUseCase(
        ICategoriaRepository categoriaRepository,
        ILogger<CreateCategoriaUseCase> logger)
    {
        _categoriaRepository = categoriaRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Categoria>> ExecuteAsync(CategoriaDto categoriaDto)
    {
        try
        {
            if (await _categoriaRepository.GetByNameAsync(categoriaDto.Nome) != null)
                return new OperationResult<Categoria> { Message = "Categoria já registrada", StatusCode = 409 };

            var categoria = new Categoria { Nome = categoriaDto.Nome.ToUpper() };

            await _categoriaRepository.AddAsync(categoria);

            return new OperationResult<Categoria> { Result = categoria };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao cadastrar categoria");
            return new OperationResult<Categoria> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
