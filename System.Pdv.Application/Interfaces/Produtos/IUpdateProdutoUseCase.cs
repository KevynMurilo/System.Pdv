using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Produtos;

public interface IUpdateProdutoUseCase
{
    Task<OperationResult<Produto>> ExecuteAsync(Guid id, UpdateProdutoDto produtoDto); 
}
