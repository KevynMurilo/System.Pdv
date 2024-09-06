using System.Pdv.Application.Common;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Mesas;

public interface IGetAllMesaServices
{
    Task<OperationResult<IEnumerable<Mesa>>> ExecuteAsync();
}
