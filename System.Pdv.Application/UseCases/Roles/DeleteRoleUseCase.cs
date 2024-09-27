using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Roles;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Roles;

public class DeleteRoleUseCase : IDeleteRoleUseCase
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<DeleteRoleUseCase> _logger;    

    public DeleteRoleUseCase(IRoleRepository roleRepository, ILogger<DeleteRoleUseCase> logger)
    {
        _roleRepository = roleRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Role>> ExecuteAsync(Guid id)
    {
        try
        {
            var roleExists = await _roleRepository.GetByIdAsync(id);
            if (roleExists == null) return new OperationResult<Role> { Message = "Role não encontrada", StatusCode = 404 };

            var standardRoles = new HashSet<string> { "ADMIN", "GARCOM" };
            if (standardRoles.Contains(roleExists.Nome.ToUpper()))
                return new OperationResult<Role> { Message = "Não é possível excluir as roles padrão", StatusCode = 400 };
            

            await _roleRepository.DeleteAsync(roleExists);

            return new OperationResult<Role> { Result = roleExists, Message = $"Role {roleExists.Nome} deletado com sucesso" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao deletar role");
            return new OperationResult<Role> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
