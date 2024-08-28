using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Garcons;

public interface IGetAllGarcomService
{
    Task<OperationResult<IEnumerable<Garcom>>> GetAllGarcom();
}
