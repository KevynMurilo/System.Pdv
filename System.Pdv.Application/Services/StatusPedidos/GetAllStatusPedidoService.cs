using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.StatusPedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.Services.StatusPedidos;

public class GetAllStatusPedidoService : IGetAllStatusPedidoService
{
    private readonly IStatusPedidoRepository _statusPedidoRepository;
    private readonly ILogger<GetAllStatusPedidoService> _logger;

    public GetAllStatusPedidoService(IStatusPedidoRepository statusPedidoRepository, ILogger<GetAllStatusPedidoService> logger)
    {
        _statusPedidoRepository = statusPedidoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<IEnumerable<StatusPedido>>> ExecuteAsync()
    {
        try
        {
            var statusPedido = await _statusPedidoRepository.GetAllAsync();
            if (!statusPedido.Any())
                return new OperationResult<IEnumerable<StatusPedido>> { Message = "Nenhum status de pedido encontrado", StatusCode = 404 };

            return new OperationResult<IEnumerable<StatusPedido>> { Result = statusPedido };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao criar status de pedido");
            return new OperationResult<IEnumerable<StatusPedido>> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
