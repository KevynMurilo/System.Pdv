using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
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

    public async Task<OperationResult<bool>> ExecuteAsync(PermissionHasRoleDto permissionHasRoleDto)
    {
        try
        {
            foreach (var roleId in permissionHasRoleDto.RoleIds)
            {
                var role = await _roleRepository.GetByIdAsync(roleId);
                if (role == null)
                    return new OperationResult<bool> { Message = $"Role com ID {roleId} não encontrada", StatusCode = 404 };
                

                foreach (var permissaoId in permissionHasRoleDto.PermissaoIds)
                {
                    var permissao = await _permissaoRepository.GetByIdAsync(permissaoId);
                    if (permissao == null)
                        return new OperationResult<bool> { Message = $"Permissão com ID {permissaoId} não encontrada", StatusCode = 404 };
                    

                    if (!role.Permissoes.Contains(permissao))
                    {
                        role.Permissoes.Add(permissao);
                    }
                }

                await _roleRepository.UpdateAsync(role);
            }

            return new OperationResult<bool> { Result = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao atribuir permissões às funções.");
            return new OperationResult<bool> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
