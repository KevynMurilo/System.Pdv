using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Permissoes;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.PermissaoHasRole;

public class AssignPermissionToRoleUseCase : IAssignPermissionToRoleUseCase
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissaoRepository _permissaoRepository;
    private readonly ILogger<AssignPermissionToRoleUseCase> _logger;

    public AssignPermissionToRoleUseCase(
        IRoleRepository roleRepository,
        IPermissaoRepository permissaoRepository,
        ILogger<AssignPermissionToRoleUseCase> logger)
    {
        _roleRepository = roleRepository;
        _permissaoRepository = permissaoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<bool>> ExecuteAsync(Guid roleId, Guid permissaoId)
    {
        try
        {
            var role = await _roleRepository.GetByIdAsync(roleId);
            var permissao = await _permissaoRepository.GetByIdAsync(permissaoId);

            if (role == null) return new OperationResult<bool> { Message = "Role não encontrada", StatusCode = 404 };
          
            if (permissao == null) return new OperationResult<bool> { Message = "Permissão não encontrada", StatusCode = 404 };
            
            if (!role.Permissoes.Contains(permissao))
            {
                role.Permissoes.Add(permissao);
                await _roleRepository.UpdateAsync(role);
            }

            return new OperationResult<bool> { Result = true };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao atribuir permissão à função.");
            return new OperationResult<bool> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
