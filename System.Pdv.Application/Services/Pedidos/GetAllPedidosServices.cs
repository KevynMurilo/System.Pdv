﻿using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.Pedidos;

public class GetAllPedidosServices : IGetAllPedidosServices
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly ILogger<GetAllPedidosServices> _logger;

    public GetAllPedidosServices(
        IPedidoRepository pedidoRepository,
        ILogger<GetAllPedidosServices> logger)
    {
        _pedidoRepository = pedidoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<IEnumerable<Pedido>>> ExecuteAsync(int pageNumber, int pageSize, string type)
    {
        try
        {
            var pedidos = await _pedidoRepository.GetPedidosAsync(pageNumber, pageSize, type);
            if (!pedidos.Any())
                return new OperationResult<IEnumerable<Pedido>> { Message = "Nenhum pedido encontrado", StatusCode = 404 };

            return new OperationResult<IEnumerable<Pedido>> { Result = pedidos };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listrar pedidos");
            return new OperationResult<IEnumerable<Pedido>> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
