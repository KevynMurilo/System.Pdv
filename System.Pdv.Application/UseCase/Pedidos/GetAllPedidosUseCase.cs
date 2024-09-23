using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Pedidos;

public class GetAllPedidosUseCase : IGetAllPedidosUseCase
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly ILogger<GetAllPedidosUseCase> _logger;

    public GetAllPedidosUseCase(
        IPedidoRepository pedidoRepository,
        ILogger<GetAllPedidosUseCase> logger)
    {
        _pedidoRepository = pedidoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<IEnumerable<Pedido>>> ExecuteAsync(int pageNumber, int pageSize, string tipoPedido, string statusPedido)
    {
        try
        {
            var pedidos = await _pedidoRepository.GetPedidosAsync(pageNumber, pageSize, tipoPedido, statusPedido);
            if (!pedidos.Any())
                return new OperationResult<IEnumerable<Pedido>> { Message = "Nenhum pedido encontrado", StatusCode = 404 };

            return new OperationResult<IEnumerable<Pedido>> { Result = pedidos };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listrar pedidos");
            return new OperationResult<IEnumerable<Pedido>> { ReqSuccess = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
