﻿using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Permissoes;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Permissoes;

public class GetAllPermissaoUseCase : IGetAllPermissaoUseCase
{
    private readonly IPermissaoRepository _permissaoRepository;
    private readonly ILogger<GetAllPermissaoUseCase> _logger;

    public GetAllPermissaoUseCase(IPermissaoRepository permissaoRepository, ILogger<GetAllPermissaoUseCase> logger)
    {
        _permissaoRepository = permissaoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<IEnumerable<Permissao>>> ExecuteAsync(int pageNumber, int pageSize, string recurso, string acao)
    {
        try
        {
            var permissoes = await _permissaoRepository.GetAllPermissao(pageNumber, pageSize, recurso, acao);
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
