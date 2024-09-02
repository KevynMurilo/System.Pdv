using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Categorias;

public interface IGetAllCategoriaService
{
    Task<OperationResult<IEnumerable<Categoria>>> GetAllCategorias();
}
