using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Pedidos;

public class GetPedidosByMesaUseCase : IGetPedidosByMesaUseCase
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly ILogger<GetPedidosByMesaUseCase> _logger;

    public GetPedidosByMesaUseCase(
        IPedidoRepository pedidoRepository,
        ILogger<GetPedidosByMesaUseCase> logger)
    {
        _pedidoRepository = pedidoRepository;
        _logger = logger;
    }

    public async Task<OperationResult<IEnumerable<Pedido>>> ExecuteAsync(int numeroMesa, string statusPedido)
    {
        try
        {
            var pedidos = await _pedidoRepository.GetPedidosByMesaAsync(numeroMesa, statusPedido);
            if (!pedidos.Any()) return new OperationResult<IEnumerable<Pedido>> { Message = $"Mesa {numeroMesa} não tem nenhum pedido vinculado", StatusCode = 404 };

            return new OperationResult<IEnumerable<Pedido>> { Result = pedidos };

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao listrar pedidos de uma mesa especifica");
            return new OperationResult<IEnumerable<Pedido>> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
