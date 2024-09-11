using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Adicionais;

public interface IGetAllAdicionalUseCase
{
    Task<OperationResult<IEnumerable<ItemAdicional>>> ExecuteAsync(int pageNumber, int pageSize);
}
