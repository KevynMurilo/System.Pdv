using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Mesas;

public interface IUpdateMesaService
{
    Task<OperationResult<Mesa>> ExecuteAsync(Guid id, MesaDto mesaDto);
}
