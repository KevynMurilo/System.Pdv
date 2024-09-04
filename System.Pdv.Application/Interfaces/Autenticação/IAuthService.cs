using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;

namespace System.Pdv.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<OperationResult<string>> ExecuteAsync(LoginDto loginDto);
}
