using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Permissoes;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Permissoes;

public class GetAllPermissaoComRolesUseCase : IGetAllPermissaoComRolesUseCase
{
    private readonly IPermissaoRepository _permissaoRepository;
    private readonly ILogger<GetAllPermissaoComRolesUseCase> _logger;

    public GetAllPermissaoComRolesUseCase(IPermissaoRepository permissaoRepository, ILogger<GetAllPermissaoComRolesUseCase> logger)
    {
        _permissaoRepository = permissaoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<IEnumerable<Permissao>>> ExecuteAsync(int pageNumber, int pageSize, string recurso , string acao)
    {
        try
        {
            var permissoes = await _permissaoRepository.GetAllPermissionWithRoleAsync(pageNumber, pageSize, recurso, acao);
            if (!permissoes.Any()) return new OperationResult<IEnumerable<Permissao>> { Message = "Nenhuma permissão encontrada", StatusCode = 404 };

            return new OperationResult<IEnumerable<Permissao>> { Result = permissoes };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar permissões");
            return new OperationResult<IEnumerable<Permissao>> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
