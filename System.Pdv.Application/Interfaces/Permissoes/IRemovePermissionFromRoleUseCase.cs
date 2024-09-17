namespace System.Pdv.Application.Interfaces.Permissoes;

public interface IRemovePermissionFromRoleUseCase
{
    Task<bool> ExecuteAsync(Guid roleId, Guid permissaoId);
}
