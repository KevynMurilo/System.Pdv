using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.StatusPedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.StatusPedidos;

public class GetByIdStatusPedidoUseCase : IGetByIdStatusPedidoUseCase
{
    private readonly IStatusPedidoRepository _statusPedidoRepository;
    private readonly ILogger<GetByIdStatusPedidoUseCase> _logger;

    public GetByIdStatusPedidoUseCase(IStatusPedidoRepository statusPedidoRepository, ILogger<GetByIdStatusPedidoUseCase> logger)
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

            return new OperationResult<StatusPedido> { Result = statusPedido };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao procurar status de pedido por id");
            return new OperationResult<StatusPedido> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
