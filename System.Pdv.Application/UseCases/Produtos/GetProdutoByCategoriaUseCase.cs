using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Produtos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Produtos;

public class GetProdutoByCategoriaUseCase : IGetProdutoByCategoriaUseCase
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ILogger<GetProdutoByCategoriaUseCase> _logger;

    public GetProdutoByCategoriaUseCase(
        IProdutoRepository produtoRepository,
        ILogger<GetProdutoByCategoriaUseCase> logger)
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
            return new OperationResult<IEnumerable<Produto>> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
