using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Produtos;

public interface IGetAllProdutoService
{
    Task<OperationResult<IEnumerable<Produto>>> ExecuteAsync(int pageNumber, int pageSize);
}
