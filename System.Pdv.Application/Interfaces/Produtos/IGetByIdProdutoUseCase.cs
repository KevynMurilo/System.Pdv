using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Produtos;

public interface IGetByIdProdutoUseCase
{
    Task<OperationResult<Produto>> ExecuteAsync(Guid id);
}
