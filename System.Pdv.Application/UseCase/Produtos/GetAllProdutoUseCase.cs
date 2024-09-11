using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Produtos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Produtos;

public class GetAllProdutoUseCase : IGetAllProdutoUseCase
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ILogger<GetAllProdutoUseCase> _logger;

    public GetAllProdutoUseCase(
        IProdutoRepository produtoRepository,
        ILogger<GetAllProdutoUseCase> logger)
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
