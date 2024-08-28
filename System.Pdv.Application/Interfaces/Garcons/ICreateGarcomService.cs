using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Core.Entities;

namespace System.Pdv.Application.Interfaces.Garcons;

public interface ICreateGarcomService
{
    Task<OperationResult<Garcom>> CreateGarcom(RegisterGarcomDto garcomDto);
}
