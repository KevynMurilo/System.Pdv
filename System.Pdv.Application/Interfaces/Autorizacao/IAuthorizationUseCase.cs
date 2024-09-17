namespace System.Pdv.Application.Interfaces.Authorization;

public interface IAuthorizationUseCase
{
    Task<bool> HasPermissionAsync(Guid userId, string recurso, string acao);
}
