using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Mesas;

public interface ICreateMesaUseCase
{
    Task<OperationResult<Mesa>> ExecuteAsync(MesaDto mesa);
}
