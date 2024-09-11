using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Adicionais;

public interface IGetByIdAdicionalUseCase
{
    Task<OperationResult<ItemAdicional>> ExecuteAsync(Guid id);
}
