using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Produtos;

public interface IDeleteProdutoService
{
    Task<OperationResult<Produto>> ExecuteAsync(Guid id);
}
