using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;

namespace System.Pdv.Application.Interfaces.Auth;

public interface IAuthUseCase
{
    Task<OperationResult<string>> ExecuteAsync(LoginDto loginDto);
}
