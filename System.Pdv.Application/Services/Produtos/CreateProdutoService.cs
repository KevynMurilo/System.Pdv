using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Produtos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Produtos;

public class CreateProdutoService : ICreateProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly ILogger<CreateProdutoService> _logger;

    public CreateProdutoService(
        IProdutoRepository produtoRepository,
        ICategoriaRepository categoriaRepository,
        ILogger<CreateProdutoService> logger)
    {
        _produtoRepository = produtoRepository;
        _categoriaRepository = categoriaRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Produto>> CreateProduto(ProdutoDto produtoDto)
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
            
            return new OperationResult<Produto> { Result = produto };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao registrar produto");
            return new OperationResult<Produto> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
