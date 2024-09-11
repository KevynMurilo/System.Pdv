using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Produtos;

public interface IGetProdutoByCategoriaUseCase
{
    Task<OperationResult<IEnumerable<Produto>>> ExecuteAsync(Guid categoriaId, int pageNumber, int pageSize);
}
