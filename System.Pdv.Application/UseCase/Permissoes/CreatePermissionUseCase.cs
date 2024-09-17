using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.Permissoes;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Permissoes;

public class CreatePermissionUseCase : ICreatePermissionUseCase
{
    private readonly IPermissaoRepository _permissaoRepository;
    private readonly ILogger<CreatePermissionUseCase> _logger;

    public CreatePermissionUseCase(IPermissaoRepository permissaoRepository, ILogger<CreatePermissionUseCase> logger)
    {
        _permissaoRepository = permissaoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Permissao>> ExecuteAsync(CreatePermissionDto permissionDto)
    {
        try
        {
            var permissao = new Permissao
            {
                Recurso = permissionDto.Recurso,
                Acao = permissionDto.Acao,
            };

            await _permissaoRepository.AddAsync(permissao);

            return new OperationResult<Permissao> { Result = permissao };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao criar permissões");
            return new OperationResult<Permissao> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
