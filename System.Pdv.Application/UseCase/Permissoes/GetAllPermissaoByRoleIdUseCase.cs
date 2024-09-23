using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Permissoes;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Permissoes;

public class GetAllPermissaoByRoleIdUseCase : IGetAllPermissaoByRoleIdUseCase
{
    private readonly IPermissaoRepository _permissaoRepository;
    private readonly ILogger<GetAllPermissaoByRoleIdUseCase> _logger;

    public GetAllPermissaoByRoleIdUseCase(IPermissaoRepository permissaoRepository, ILogger<GetAllPermissaoByRoleIdUseCase> logger)
    {
        _permissaoRepository = permissaoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<IEnumerable<Permissao>>> ExecuteAsync(Guid id)
    {
        try
        {
            var permissoes = await _permissaoRepository.GetAllPermissaoByRoleIdAsync(id);
            if (!permissoes.Any()) return new OperationResult<IEnumerable<Permissao>> { Message = $"Nenhuma permissão encontrada com o Id - {id}", StatusCode = 404 };

            return new OperationResult<IEnumerable<Permissao>> { Result = permissoes };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar permissões pelo Id");
            return new OperationResult<IEnumerable<Permissao>> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
