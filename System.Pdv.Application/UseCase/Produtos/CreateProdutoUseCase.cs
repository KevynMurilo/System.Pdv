using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Produtos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Produtos;

public class CreateProdutoUseCase : ICreateProdutoUseCase
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly ILogger<CreateProdutoUseCase> _logger;

    public CreateProdutoUseCase(
        IProdutoRepository produtoRepository,
        ICategoriaRepository categoriaRepository,
        ILogger<CreateProdutoUseCase> logger)
    {
        _produtoRepository = produtoRepository;
        _categoriaRepository = categoriaRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Produto>> ExecuteAsync(ProdutoDto produtoDto)
    {
        try
        {
            var existingCategoria = await _categoriaRepository.GetByIdAsync(produtoDto.CategoriaId);
            if (existingCategoria == null) return new OperationResult<Produto> { Message = "Categoria não encontrada", StatusCode = 404 };

            var produto = new Produto
            {
                Nome = produtoDto.Nome,
                Descricao = produtoDto.Descricao,
                Preco = produtoDto.Preco,
                Disponivel = produtoDto.Disponivel,
                CategoriaId = produtoDto.CategoriaId,
            };

            await _produtoRepository.AddAsync(produto);
            
            return new OperationResult<Produto> { Result = await _produtoRepository.GetByIdAsync(produto.Id) };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao registrar produto");
            return new OperationResult<Produto> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
