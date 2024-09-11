using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Categorias;

public interface IGetByIdCategoriaUseCase
{
    Task<OperationResult<Categoria>> ExecuteAsync(Guid id);
}
