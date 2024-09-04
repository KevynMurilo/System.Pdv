using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Adicionais;

public interface IDeleteAdicionalService
{
    Task<OperationResult<ItemAdicional>> ExecuteAsync(Guid id);
}
