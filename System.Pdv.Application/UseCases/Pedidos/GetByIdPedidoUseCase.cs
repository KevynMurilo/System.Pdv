﻿using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Pedidos;

public class GetByIdPedidoUseCase : IGetByIdPedidoUseCase
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly ILogger<GetByIdPedidoUseCase> _logger;

    public GetByIdPedidoUseCase(IPedidoRepository pedidoRepository, ILogger<GetByIdPedidoUseCase> logger)
    {
        _pedidoRepository = pedidoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<Pedido>> ExecuteAsync(Guid id)
    {
        try
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null) return new OperationResult<Pedido> { Message = "Pedido não encontrado", StatusCode = 404 };

            return new OperationResult<Pedido> { Result = pedido };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao procurar pedido por id");
            return new OperationResult<Pedido> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
