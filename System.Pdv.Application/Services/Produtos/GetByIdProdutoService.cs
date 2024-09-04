using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Produtos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Produtos;

public class GetByIdProdutoService : IGetByIdProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ILogger<GetByIdProdutoService> _logger;

    public GetByIdProdutoService(
        IProdutoRepository produtoRepository,
        ILogger<GetByIdProdutoService> logger)
    {
        _produtoRepository = produtoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Produto>> ExecuteAsync(Guid id)
    {
        try
        {
            var categoria = await _produtoRepository.GetByIdAsync(id);
            if (categoria == null) return new OperationResult<Produto> { Message = "Produto não encontrado", StatusCode = 404 };

            return new OperationResult<Produto> { Result = categoria };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar produto por id");
            return new OperationResult<Produto> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
