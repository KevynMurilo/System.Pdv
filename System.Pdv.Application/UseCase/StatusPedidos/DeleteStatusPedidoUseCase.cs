using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.StatusPedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.StatusPedidos;

public class DeleteStatusPedidoUseCase : IDeleteStatusPedidoUseCase
{
    private readonly IStatusPedidoRepository _statusPedidoRepository;
    private readonly ILogger<DeleteStatusPedidoUseCase> _logger;

    public DeleteStatusPedidoUseCase(IStatusPedidoRepository statusPedidoRepository, ILogger<DeleteStatusPedidoUseCase> logger)
    {
        _statusPedidoRepository = statusPedidoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<StatusPedido>> ExecuteAsync(Guid id)
    {
        try
        {
            var statusPedido = await _statusPedidoRepository.GetByIdAsync(id);
            if (statusPedido == null)
                return new OperationResult<StatusPedido> { Message = "Status de pedido não encontrado", StatusCode = 404 };

            await _statusPedidoRepository.DeleteAsync(statusPedido);

            return new OperationResult<StatusPedido> { Result = statusPedido, Message = "Status de pedido deletado com sucesso" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao deletar status de pedido");
            return new OperationResult<StatusPedido> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
