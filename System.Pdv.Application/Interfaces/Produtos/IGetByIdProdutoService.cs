using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Produtos;

public interface IGetByIdProdutoService
{
    Task<OperationResult<Produto>> GetByIdProduto(Guid id);
}
