using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Categorias;

public interface IGetAllCategoriaUseCase
{
    Task<OperationResult<IEnumerable<Categoria>>> ExecuteAsync();
}
