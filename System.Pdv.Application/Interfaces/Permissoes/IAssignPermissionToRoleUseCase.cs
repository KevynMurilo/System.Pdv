namespace System.Pdv.Application.Interfaces.Permissoes;

public interface IAssignPermissionToRoleUseCase
{
    Task<bool> ExecuteAsync(Guid roleId, Guid permissaoId);
}
