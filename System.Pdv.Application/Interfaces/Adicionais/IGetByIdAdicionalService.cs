using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Adicionais;

public interface IGetByIdAdicionalService
{
    Task<OperationResult<ItemAdicional>> GetById(Guid id);
}
