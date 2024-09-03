using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Produtos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Produtos;

public class DeleteProdutoService : IDeleteProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ILogger<DeleteProdutoService> _logger;

    public DeleteProdutoService(IProdutoRepository produtoRepository, ILogger<DeleteProdutoService> logger)
    {
        _produtoRepository = produtoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Produto>> DeleteProduto(Guid id)
    {
        try
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null) return new OperationResult<Produto> { Message = "Produto não encontrado", StatusCode = 404 };

            await _produtoRepository.DeleteAsync(produto);

            return new OperationResult<Produto> { Result = produto, Message = "Produto deletado com sucesso" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao deletar produto");
            return new OperationResult<Produto> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
