using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Garcons;

public interface IGetByIdGarcomService
{
    Task<OperationResult<Garcom>> GetById(Guid id);
}
