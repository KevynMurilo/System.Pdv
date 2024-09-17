using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Permissoes;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Permissoes;

public class GetAllPermissionUseCase : IGetAllPermissionUseCase
{
    private readonly IPermissaoRepository _permissaoRepository;
    private readonly ILogger<GetAllPermissionUseCase> _logger;

    public GetAllPermissionUseCase(IPermissaoRepository permissaoRepository, ILogger<GetAllPermissionUseCase> logger)
    {
        _permissaoRepository = permissaoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<IEnumerable<Permissao>>> ExecuteAsync()
    {
        try
        {
            var permissoes = await _permissaoRepository.GetAllAsync();
            if (!permissoes.Any()) return new OperationResult<IEnumerable<Permissao>> { Message = "Nenhuma permissão encontrada", StatusCode = 404 };

            return new OperationResult<IEnumerable<Permissao>> { Result = permissoes };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listar permissões");
            return new OperationResult<IEnumerable<Permissao>> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
