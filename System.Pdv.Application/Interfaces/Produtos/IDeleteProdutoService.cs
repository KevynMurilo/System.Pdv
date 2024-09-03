using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Produtos;

public interface IDeleteProdutoService
{
    Task<OperationResult<Produto>> DeleteProduto(Guid id);
}
