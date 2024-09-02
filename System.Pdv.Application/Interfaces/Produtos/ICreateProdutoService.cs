using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Produtos;

public interface ICreateProdutoService
{
    Task<OperationResult<Produto>> CreateProduto(ProdutoDto produtoDto);
}
