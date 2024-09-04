﻿using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Adicionais;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Adicionais;

public class DeleteAdicionalService : IDeleteAdicionalService
{
    private readonly IAdicionalRepository _adicionalRepository;
    private readonly ILogger<DeleteAdicionalService> _logger;

    public DeleteAdicionalService(IAdicionalRepository adicionalRepository, ILogger<DeleteAdicionalService> logger)
    {
        _adicionalRepository = adicionalRepository;
        _logger = logger;
    }

    public async Task<OperationResult<ItemAdicional>> ExecuteAsync(Guid id)
    {
        try
        {
            var adicional = await _adicionalRepository.GetByIdAsync(id);
            if (adicional == null)
                return new OperationResult<ItemAdicional> { Message = "Adicional não encontrado", StatusCode = 404 };

            await _adicionalRepository.DeleteAsync(adicional);

            return new OperationResult<ItemAdicional> { Result = adicional, Message = "Adicional deletado com sucesso" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao deletar adicional");
            return new OperationResult<ItemAdicional> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
