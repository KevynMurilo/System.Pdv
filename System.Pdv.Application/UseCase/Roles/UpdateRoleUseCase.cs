using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Roles;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Roles;

public class UpdateRoleUseCase : IUpdateRoleUseCase
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<UpdateRoleUseCase> _logger;

    public UpdateRoleUseCase(IRoleRepository roleRepository, ILogger<UpdateRoleUseCase> logger)
    {
        _roleRepository = roleRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Role>> ExecuteAsync(Guid id, RoleDto roleDto)
    {
        try
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null) return new OperationResult<Role> { Message = "Role não encontrada", StatusCode = 404 };

            var standardRoles = new HashSet<string> { "ADMIN", "GARCOM" };
            if (standardRoles.Contains(role.Nome.ToUpper()))
                return new OperationResult<Role> { Message = "Não é possível atualizar as roles padrão", StatusCode = 400 };

            if (await _roleRepository.GetByNameAsync(roleDto.Nome) != null)
                return new OperationResult<Role> { Message = "Role já registrada", StatusCode = 404 };

            role.Nome = roleDto.Nome.ToUpper();
            role.Descricao = roleDto.Descricao;

            await _roleRepository.UpdateAsync(role);

            return new OperationResult<Role> { Result = role, Message = "Role atualizada com sucesso" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao atualizar role");
            return new OperationResult<Role> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
