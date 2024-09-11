using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Categorias;

public interface IUpdateCategoriaUseCase
{
    Task<OperationResult<Categoria>> ExecuteAsync(Guid id, CategoriaDto categoria);
}
