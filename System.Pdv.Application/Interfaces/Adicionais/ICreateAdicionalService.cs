using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Adicionais;

public interface ICreateAdicionalService
{
    Task<OperationResult<ItemAdicional>> CreateAdicional(AdicionalDto adicionalDto);
}
