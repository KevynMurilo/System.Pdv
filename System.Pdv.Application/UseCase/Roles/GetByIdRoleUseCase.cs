using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Roles;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Roles;

public class GetByIdRoleUseCase : IGetByIdRoleUseCase
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<GetByIdRoleUseCase> _logger;

    public GetByIdRoleUseCase(IRoleRepository roleRepository, ILogger<GetByIdRoleUseCase> logger)
    {
        _roleRepository = roleRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Role>> ExecuteAsync(Guid id)
    {
        try
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null) return new OperationResult<Role> { Message = "Role não encontrada", StatusCode = 404 };

            return new OperationResult<Role> { Result = role };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao pegar role por Id");
            return new OperationResult<Role> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
