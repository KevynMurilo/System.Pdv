using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Produtos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Produtos;

public class GetAllProdutoService : IGetAllProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ILogger<GetAllProdutoService> _logger;

    public GetAllProdutoService(
        IProdutoRepository produtoRepository,
        ILogger<GetAllProdutoService> logger)
    {
        _produtoRepository = produtoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<IEnumerable<Produto>>> ExecuteAsync(int pageNumber, int pageSize)
    {
        try
        {
            var produtos = await _produtoRepository.GetAllAsync(pageNumber, pageSize);
            if (!produtos.Any()) return new OperationResult<IEnumerable<Produto>> { Message = "Nenhum produto encontrado", StatusCode = 404 };

            return new OperationResult<IEnumerable<Produto>> { Result = produtos };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar produtos");
            return new OperationResult<IEnumerable<Produto>> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
