using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Produtos;

public interface IUpdateProdutoService
{
    Task<OperationResult<Produto>> ExecuteAsync(Guid id, ProdutoDto produtoDto); 
}
