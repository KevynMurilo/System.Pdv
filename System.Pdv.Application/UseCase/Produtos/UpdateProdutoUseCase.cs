using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Produtos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Produtos;

public class UpdateProdutoUseCase : IUpdateProdutoUseCase
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly ILogger<UpdateProdutoUseCase> _logger;

    public UpdateProdutoUseCase(
        IProdutoRepository produtoRepository,
        ICategoriaRepository categoriaRepository,
        ILogger<UpdateProdutoUseCase> logger)
    {
        _produtoRepository = produtoRepository;
        _categoriaRepository = categoriaRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Produto>> ExecuteAsync(Guid id, UpdateProdutoDto produtoDto)
    {
        try
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null)
                return new OperationResult<Produto> { Message = "Produto não encontrado", StatusCode = 404 };

            if (produtoDto.CategoriaId != Guid.Empty)
            {
                var categoria = await _categoriaRepository.GetByIdAsync(produtoDto.CategoriaId);
                if (categoria == null)
                    return new OperationResult<Produto> { Message = "Categoria não encontrada", StatusCode = 404 };

                produto.Categoria = categoria;
            }

            UpdateFromDto(produto, produtoDto);

            await _produtoRepository.UpdateAsync(produto);

            return new OperationResult<Produto> { Result = produto };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao atualizar produto");
            return new OperationResult<Produto> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }

    private void UpdateFromDto(Produto target, UpdateProdutoDto source)
    {
        if (!string.IsNullOrEmpty(source.Nome))
            target.Nome = source.Nome;

        if (!string.IsNullOrEmpty(source.Descricao))
            target.Descricao = source.Descricao;

        if (source.Preco.HasValue)
            target.Preco = source.Preco.Value;

        if (source.Disponivel.HasValue)
            target.Disponivel = source.Disponivel.Value;
    }
}
