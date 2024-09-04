using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Adicionais;

public interface IUpdateAdicionalService
{
    Task<OperationResult<ItemAdicional>> ExecuteAsync(Guid id, AdicionalDto adicionalDto);
}
