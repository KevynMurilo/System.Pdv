using Microsoft.Extensions.Logging;
using System.Pdv.Application.Common;
using System.Pdv.Application.Interfaces.Pedidos;
using System.Pdv.Core.Entities;
using System.Pdv.Core.Interfaces;

namespace System.Pdv.Application.UseCase.Pedidos;

public class PrintPedidoByIdUseCase : IPrintPedidoByIdUseCase
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IThermalPrinterService _thermalPrinterService;
    private readonly ILogger<PrintPedidoByIdUseCase> _logger;

    public PrintPedidoByIdUseCase(
        IPedidoRepository pedidoRepository,
        IThermalPrinterService thermalPrinterService,
        ILogger<PrintPedidoByIdUseCase> logger)
    {
        _pedidoRepository = pedidoRepository;
        _thermalPrinterService = thermalPrinterService;
        _logger = logger;
    }

    public async Task<OperationResult<Pedido>> ExecuteAsync(Guid id)
    {
        try
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null) return new OperationResult<Pedido> { Message = "Pedido não encontrado", StatusCode = 404 };

            var print = _thermalPrinterService.PrintOrder(pedido);
            var message = print
                ? "Pedido impresso com sucesso"
                : "Ocorreu um erro ao imprimir pedido";

            return new OperationResult<Pedido> { Result = pedido, Message = message };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocorreu um erro ao imprimir pedido por id");
            return new OperationResult<Pedido> { ServerOn = false, Message = $"Erro inesperado: {ex.Message}", StatusCode = 500 };
        }
    }
}
