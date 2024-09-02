using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Produtos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Produtos;

public class UpdateProdutoService : IUpdateProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly ILogger<UpdateProdutoService> _logger;

    public UpdateProdutoService(
        IProdutoRepository produtoRepository,
        ICategoriaRepository categoriaRepository,
        ILogger<UpdateProdutoService> logger)
    {
        _produtoRepository = produtoRepository;
        _categoriaRepository = categoriaRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Produto>> UpdateProduto(Guid id, ProdutoDto produtoDto)
    {
        try
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null) return new OperationResult<Produto> { Message = "Produto não encontrado", StatusCode = 404 };

            var categoria = await _categoriaRepository.GetByIdAsync(produtoDto.CategoriaId);
            if (categoria == null) return new OperationResult<Produto> { Message = "Categoria não encontrada", StatusCode = 404 };

            produto.Nome = produtoDto.Nome;
            produto.Descricao = produtoDto.Descricao;
            produto.Preco = produtoDto.Preco;
            produto.Disponivel = produtoDto.Disponivel;
            produto.Categoria = categoria;

            await _produtoRepository.UpdateAsync(produto);

            Console.WriteLine(produto);
            return new OperationResult<Produto> { Result = produto };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao atualizar produto");
            return new OperationResult<Produto> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
