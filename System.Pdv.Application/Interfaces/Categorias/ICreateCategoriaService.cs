using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Categorias;

public interface ICreateCategoriaService
{
    Task<OperationResult<Categoria>> ExecuteAsync(CategoriaDto categoriaDto);
}
