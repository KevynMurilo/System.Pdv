using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Produtos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Produtos;

public class DeleteProdutoUseCase : IDeleteProdutoUseCase
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ILogger<DeleteProdutoUseCase> _logger;

    public DeleteProdutoUseCase(IProdutoRepository produtoRepository, ILogger<DeleteProdutoUseCase> logger)
    {
        _produtoRepository = produtoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Produto>> ExecuteAsync(Guid id)
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
            return new OperationResult<Produto> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
