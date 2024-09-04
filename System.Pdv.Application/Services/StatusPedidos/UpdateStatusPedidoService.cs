using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.DTOs;
using System.Pdv.Application.Interfaces.StatusPedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.StatusPedidos;

public class UpdateStatusPedidoService : IUpdateStatusPedidoService
{
    private readonly IStatusPedidoRepository _statusPedidoRepository;
    private readonly ILogger<UpdateStatusPedidoService> _logger;

    public UpdateStatusPedidoService(IStatusPedidoRepository statusPedidoRepository, ILogger<UpdateStatusPedidoService> logger)
    {
        _statusPedidoRepository = statusPedidoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<StatusPedido>> ExecuteAsync(Guid id, StatusPedidoDto statusPedidoDto)
    {
        try
        {
            var statusPedido = await _statusPedidoRepository.GetByIdAsync(id);
            if (statusPedido == null)
                return new OperationResult<StatusPedido> { Message = "Status de pedido não encontrado", StatusCode = 404 };

            if (await _statusPedidoRepository.GetByNameAsync(statusPedidoDto.Status) != null)
                return new OperationResult<StatusPedido> { Message = "Status de pedido já registrado", StatusCode = 409 };

            statusPedido.Status = statusPedidoDto.Status.ToUpper();

            await _statusPedidoRepository.UpdateAsync(statusPedido);

            return new OperationResult<StatusPedido> { Result = statusPedido, Message = "Status de pedido atualizado com sucesso" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao atualizar status de pedido");
            return new OperationResult<StatusPedido> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
