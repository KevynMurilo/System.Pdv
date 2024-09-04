using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.StatusPedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.StatusPedidos;

public class CreateStatusPedidoService : ICreateStatusPedidoService
{
    private readonly IStatusPedidoRepository _statusPedidoRepository;
    private readonly ILogger<CreateStatusPedidoService> _logger;

    public CreateStatusPedidoService(IStatusPedidoRepository statusPedidoRepository, ILogger<CreateStatusPedidoService> logger)
    {
        _statusPedidoRepository = statusPedidoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<StatusPedido>> ExecuteAsync(StatusPedidoDto statusPedidoDto)
    {
        try
        {
            if (await _statusPedidoRepository.GetByNameAsync(statusPedidoDto.Status) != null)
                return new OperationResult<StatusPedido> { Message = "Status de pedido já registrado", StatusCode = 409 };

            var statusPedido = new StatusPedido { Status = statusPedidoDto.Status.ToUpper() };

            await _statusPedidoRepository.AddAsync(statusPedido);

            return new OperationResult<StatusPedido> { Result = statusPedido };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao criar status de pedido");
            return new OperationResult<StatusPedido> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
