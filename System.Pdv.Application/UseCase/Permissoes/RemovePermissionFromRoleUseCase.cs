using System.Pdv.Application.Interfaces.Permissoes;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Permissoes;

public class RemovePermissionFromRoleUseCase : IRemovePermissionFromRoleUseCase
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissaoRepository _permissaoRepository;

    public RemovePermissionFromRoleUseCase(IRoleRepository roleRepository, IPermissaoRepository permissaoRepository)
    {
        _roleRepository = roleRepository;
        _permissaoRepository = permissaoRepository;
    }

    public async Task<bool> ExecuteAsync(Guid roleId, Guid permissaoId)
    {
        var role = await _roleRepository.GetByIdAsync(roleId);
        var permissao = await _permissaoRepository.GetByIdAsync(permissaoId);

        if (role == null || permissao == null)
        {
            return false;
        }

        if (role.Permissoes.Contains(permissao))
        {
            role.Permissoes.Remove(permissao);
            await _roleRepository.UpdateAsync(role);
        }

        return true;
    }
}