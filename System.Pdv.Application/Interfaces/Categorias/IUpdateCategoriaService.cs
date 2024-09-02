using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Categorias;

public interface IUpdateCategoriaService
{
    Task<OperationResult<Categoria>> UpdateCategoria(Guid id, CategoriaDto categoria);
}
