using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Produtos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Produtos;

public class GetProdutoByCategoriaService : IGetProdutoByCategoriaService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ILogger<GetProdutoByCategoriaService> _logger;

    public GetProdutoByCategoriaService(
        IProdutoRepository produtoRepository,
        ILogger<GetProdutoByCategoriaService> logger)
    {
        _produtoRepository = produtoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<IEnumerable<Produto>>> ExecuteAsync(Guid categoriaId, int pageNumber, int pageSize)
    {
        try
        {
            var produtos = await _produtoRepository.GetProdutoByCategoria(categoriaId, pageNumber, pageSize);
            if (produtos == null || !produtos.Any())
                return new OperationResult<IEnumerable<Produto>> { Message = "Nenhum produto encontrado para a categoria especificada", StatusCode = 404 };

            return new OperationResult<IEnumerable<Produto>> { Result = produtos };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao recuperar produtos por categoria");
            return new OperationResult<IEnumerable<Produto>> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
