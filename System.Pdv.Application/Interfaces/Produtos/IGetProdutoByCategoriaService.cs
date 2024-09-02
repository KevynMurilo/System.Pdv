using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Produtos;

public interface IGetProdutoByCategoriaService
{
    Task<OperationResult<IEnumerable<Produto>>> GetProdutoByCategoria(Guid categoriaId, int pageNumber, int pageSize);
}
